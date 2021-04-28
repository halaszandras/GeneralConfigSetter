using GeneralConfigSetter.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneralConfigSetter.Services

{
    public static class ConfigUpdateService
    {
        public static void UpdateGeneralConfig(IContext context, string generalConfigFilePath)
        {
            dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(generalConfigFilePath));

            json.Source.Collection = context.SourceCollectionUrl;
            json.Source.Project = context.SourceProjectName;
            json.Source.PersonalAccessToken = context.ServerPats.FirstOrDefault(x => x.Key.ToLower() == context.SourceServerName.ToLower()).Value;

            json.Target.Collection = context.TargetCollectionUrl;
            json.Target.Project = context.TargetProjectName;
            json.Target.PersonalAccessToken = context.ServerPats.FirstOrDefault(x => x.Key.ToLower() == context.TargetServerName.ToLower()).Value;

            string result = UpdateMigrationInbox(context, json);

            File.WriteAllText(generalConfigFilePath, result);
        }

        public static void UpdateDeleterConfig(IContext context, string deleterConfigFilePath)
        {
            dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(deleterConfigFilePath));

            json.Target.Collection = context.TargetCollectionUrl;
            json.Target.Project = context.TargetProjectName;
            json.Target.PersonalAccessToken = context.ServerPats.FirstOrDefault(x => x.Key.ToLower() == context.TargetServerName.ToLower()).Value;
            var processors = json.Processors;
            var mainProcessor = processors.First;

            foreach (var item in mainProcessor)
            {
                if (item.Name == "WIQLQueryBit")
                {
                    item.First.Value = context.QueryText;
                }
            }

            //processorChildren.First.ChildrenTokens[2] = context.QueryText;

            string result = UpdateMigrationInbox(context, json);

            File.WriteAllText(deleterConfigFilePath, result);
        }

        private static string UpdateMigrationInbox(IContext context, dynamic json)
        {
            string result = JsonConvert.SerializeObject(json, Formatting.Indented);
            string subject = "\"\\\\MigrationInbox\"";

            return result.Replace(subject, $"\"\\\\MigrationInbox\\\\{context.WorkItemProjectName.Trim()}\"");
        }

        public static void UpdateAttachmentConfig(IContext context, string attachmentConfigFilePath)
        {
            //NEED IMPLEMENTATION
            System.Xml.XmlDocument xmlDoc = new();
            xmlDoc.Load(attachmentConfigFilePath);
            System.Xml.XmlNode settings = xmlDoc.
                SelectSingleNode("/configuration/applicationSettings/AttachmentMigrator.Properties.Settings") as System.Xml.XmlElement;

            List<string> fieldNames = new()
            {
                "SourceCollectionUrl",
                "SourceProjectName",
                "SourceSharePath",
                "TargetCollectionUrl",
                "TargetProjectName",
                "TargetSharePath",
                "TargetQueryBit"
            };

            if (settings != null)
            {
                foreach (System.Xml.XmlNode childNode in settings.ChildNodes)
                {
                    foreach (string fieldName in fieldNames)
                    {
                        if (childNode.OuterXml.ToLower().Contains(fieldName.ToLower()))
                        {
                            switch (fieldName)
                            {
                                case "SourceCollectionUrl":
                                    childNode.FirstChild.InnerText = context.SourceCollectionUrl;
                                    break;
                                case "SourceProjectName":
                                    childNode.FirstChild.InnerText = context.SourceProjectName;
                                    break;
                                case "SourceSharePath":
                                    childNode.FirstChild.InnerText = context.ServerRepositories.FirstOrDefault(x => x.Key.ToLower() == context.SourceServerName.ToLower()).Value;
                                    break;
                                case "TargetCollectionUrl":
                                    childNode.FirstChild.InnerText = context.TargetCollectionUrl;
                                    break;
                                case "TargetProjectName":
                                    childNode.FirstChild.InnerText = context.TargetProjectName;
                                    break;
                                case "TargetSharePath":
                                    childNode.FirstChild.InnerText = context.ServerRepositories.FirstOrDefault(x => x.Key.ToLower() == context.TargetServerName.ToLower()).Value;
                                    break;
                                case "TargetQueryBit":
                                    childNode.FirstChild.InnerText = context.QueryText;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }

            using var stringWriter = new StringWriter();
            System.Xml.XmlWriterSettings xmlWriterSettings = new()
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = System.Xml.NewLineHandling.Replace,
                Encoding = Encoding.UTF8
            };
            using var xmlTextWriter = System.Xml.XmlWriter.Create(stringWriter, xmlWriterSettings);
            xmlDoc.WriteTo(xmlTextWriter);
            xmlTextWriter.Flush();
            string result = stringWriter.GetStringBuilder().ToString();

            File.WriteAllText(attachmentConfigFilePath, result);
        }
    }
}
