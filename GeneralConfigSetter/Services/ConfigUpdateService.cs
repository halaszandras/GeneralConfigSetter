using System;
using GeneralConfigSetter.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace GeneralConfigSetter.Services

{
    public static class ConfigUpdateService
    {
        public static void UpdateGeneralConfig(IContext context, string generalConfigFilePath)
        {
            UpdateMigrationInbox(context, generalConfigFilePath);

            dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(generalConfigFilePath));

            json.Source.Collection = context.SourceCollectionUrl;
            json.Source.Project = context.SourceProjectName;
            json.Source.PersonalAccessToken = context.ServerPats.FirstOrDefault(x => x.Key.ToLower() == context.SourceServerName.ToLower()).Value;

            json.Target.Collection = context.TargetCollectionUrl;
            json.Target.Project = context.TargetProjectName;
            json.Target.PersonalAccessToken = context.ServerPats.FirstOrDefault(x => x.Key.ToLower() == context.TargetServerName.ToLower()).Value;

            string result = JsonConvert.SerializeObject(json, Formatting.Indented);

            File.WriteAllText(generalConfigFilePath, result);
        }

        public static void UpdateTestItemsConfig(IContext context,
                                                 bool isTestItemsProcessorEnabled,
                                                 bool isTestVariablesEnabled,
                                                 bool isTestConfigurationsEnabled,
                                                 bool isTestPlansAndSuitesProcessorEnabled,
                                                 string testItemsConfigFilePath)
        {
            JObject json = JObject.Parse(File.ReadAllText(testItemsConfigFilePath));

            foreach (JToken token in json["Processors"])
            {
                if (token.ToString().Contains("WorkItemMigrationConfig"))
                {
                    token["WIQLQueryBit"] = context.QueryText; ;
                    token["Enabled"] = isTestItemsProcessorEnabled;
                }

                else if (token.ToString().Contains("TestVariablesMigrationConfig"))
                    token["Enabled"] = isTestVariablesEnabled;

                else if (token.ToString().Contains("TestConfigurationsMigrationConfig"))
                    token["Enabled"] = isTestConfigurationsEnabled;

                else if (token.ToString().Contains("TestPlansAndSuitesMigrationConfig"))
                {
                    token["Enabled"] = isTestPlansAndSuitesProcessorEnabled;
                    token["TestPlanQueryBit"] = context.TestPlanNamesText;
                }
            }

            string result = JsonConvert.SerializeObject(json, Formatting.Indented);

            File.WriteAllText(testItemsConfigFilePath, result);
        }

        public static void UpdateDeleterConfig(IContext context, string deleterConfigFilePath)
        {
            UpdateMigrationInbox(context, deleterConfigFilePath);

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

            string result = JsonConvert.SerializeObject(json, Formatting.Indented);

            File.WriteAllText(deleterConfigFilePath, result);
        }

        private static void UpdateMigrationInbox(IContext context, string filePath)
        {
            string[] fileContent = File.ReadAllLines(filePath);
            for (var index = 0; index < fileContent.Length; index++)
            {
                string line = fileContent[index];
                if (line.Contains("MigrationInbox"))
                {
                    fileContent[index] = $"\"TargetPath\": \"\\\\MigrationInbox\\\\{context.WorkItemProjectName.Trim()}\"";
                }
            }

            string jsonString = string.Join("", fileContent);

            File.WriteAllText(filePath, jsonString);
        }

        public static void UpdateAttachmentConfig(IContext context, string attachmentConfigFilePath)
        {
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

        public static void UpdateBugFamilyConfig(IContext context, string attachmentConfigFilePath)
        {
            context.SourceServerName = "venus";
            context.SourceCollectionUrl = "https://venus.tfs.siemens.net/tfs/TIA";
            context.SourceProjectName = "TIA";

            System.Xml.XmlDocument xmlDoc = new();
            xmlDoc.Load(attachmentConfigFilePath);
            System.Xml.XmlNode settings = xmlDoc.
                SelectSingleNode("/configuration/appSettings") as System.Xml.XmlElement;

            List<string> fieldNames = new()
            {
                "SourceCollectionURL",
                "SourceTeamProject",
                "TargetCollectionURL",
                "TargetTeamProject",
                "QueryBits",
                "WorkItemIds",
                "SourcePersonalAccessToken",
                "TargetPersonalAccessToken"
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
                                case "SourceCollectionURL":
                                    childNode.Attributes[1].Value = context.SourceCollectionUrl;
                                    break;
                                case "SourceTeamProject":
                                    childNode.Attributes[1].Value = context.SourceProjectName;
                                    break;
                                case "TargetCollectionURL":
                                    childNode.Attributes[1].Value = context.TargetCollectionUrl;
                                    break;
                                case "TargetTeamProject":
                                    childNode.Attributes[1].Value = context.TargetProjectName;
                                    break;
                                case "QueryBits":
                                    childNode.Attributes[1].Value = context.QueryText;
                                    break;
                                case "WorkItemIds":
                                    childNode.Attributes[1].Value = "";
                                    break;
                                case "SourcePersonalAccessToken":
                                    childNode.Attributes[1].Value = context.ServerPats.FirstOrDefault(x => x.Key.ToLower() == context.SourceServerName.ToLower()).Value;
                                    break;
                                case "TargetPersonalAccessToken":
                                    childNode.Attributes[1].Value = context.ServerPats.FirstOrDefault(x => x.Key.ToLower() == context.TargetServerName.ToLower()).Value;
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

        public static string CreateAttachmentQueryBit(string rawTags)
        {
            List<string> tags = new(rawTags.Split(';'));
            tags.RemoveAt(tags.Count - 1);
            string result = "";
            if (tags.Count >= 2)
            {
                List<string> tagSentences = new List<string>();
                result = "AND (";
                foreach (string tag in tags)
                {
                    tagSentences.Add($"[System.Tags] contains '{tag}'");
                }
                result += string.Join(" OR ", tagSentences) + ")";
            }
            else if (tags.Count == 1)
            {
                result = $"AND [System.Tags] contains '{rawTags.Trim(';')}'";
            }
            return "AND NOT [System.Tags] contains 'TRANSFERRED_ATTACHMENTS_MIGRATED' " +
                    "AND NOT [System.Tags] contains 'TRANSFERRED_ATTACHMENTS_PROCESSED' " + result;
        }

        public static string CreateTestItemsQueryBit(string rawTags)
        {
            List<string> tags = new(rawTags.Split(';'));
            tags.RemoveAt(tags.Count - 1);
            string result = "";
            if (tags.Count >= 2)
            {
                List<string> tagSentences = new List<string>();
                result = "AND (";
                foreach (string tag in tags)
                {
                    tagSentences.Add($"[System.Tags] contains '{tag}'");
                }
                result += string.Join(" OR ", tagSentences) + ")";
            }
            else if (tags.Count == 1)
            {
                result = $"AND [System.Tags] contains '{rawTags.Trim(';')}'";
            }
            return result;
        }

        public static string CreateTestPlansQueryBit(string rawNameQueue)
        {
            List<string> tags = new(rawNameQueue.Split(';'));
            tags.RemoveAt(tags.Count - 1);
            string result = "";
            if (tags.Count >= 2)
            {
                List<string> tagSentences = new List<string>();
                foreach (string tag in tags)
                {
                    tagSentences.Add($"PlanName='{tag}'");
                }
                result += string.Join(" OR ", tagSentences);
            }
            else if (tags.Count == 1)
            {
                result = $"PlanName='{rawNameQueue.Trim(';')}'";
            }
            return result;
        }

        public static string CreateDeleterQueryBit(string rawTags)
        {
            List<string> tags = new(rawTags.Split(';'));
            tags.RemoveAt(tags.Count - 1);
            string result = "";
            if (tags.Count >= 2)
            {
                List<string> tagSentences = new List<string>();
                result = "AND (";
                foreach (string tag in tags)
                {
                    tagSentences.Add($"[System.Tags] contains '{tag}'");
                }
                result += string.Join(" OR ", tagSentences) + ")";
            }
            else if (tags.Count == 1)
            {
                result = $"AND [System.Tags] contains '{rawTags.Trim(';')}'";
            }
            return "AND [System.WorkItemType] NOT IN ('Test Suite', 'Test Plan', 'Test Case') " + result;
        }

        public static string ValidateInput(string input)
        {
            string output = Regex.Replace(input, "(?<!\r)\n", "\r\n")
                                 .Replace(" ", "");

            return output;
        }

        public static string ValidateUpdateInput(string input)
        {
            
            var helper = input.ToCharArray().ToList();
            helper.Reverse();
            for (var index = 0; index < helper.Count; index++)
            {
                var character = helper[index];
                if (character is '\r' or '\n')
                {
                    helper.RemoveAt(index);
                    index--;
                }
                else
                {
                    break;
                }
            }

            helper.Reverse();
            var output = string.Join("",helper);
            return output;
        }
    }
}
