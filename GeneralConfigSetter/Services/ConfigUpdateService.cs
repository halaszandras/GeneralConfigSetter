using GeneralConfigSetter.Models;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

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

            string result = JsonConvert.SerializeObject(json, Formatting.Indented);
            string subject = "\"\\\\MigrationInbox\"";

            result = result.Replace(subject, $"\"\\\\MigrationInbox\\\\{context.WorkItemProjectName}\"");

            File.WriteAllText(generalConfigFilePath, result);
        }

        public static void UpdateDeleterConfig(IContext context, string generalConfigFilePath)
        {
            dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(generalConfigFilePath));

            json.Target.Collection = context.TargetCollectionUrl;
            json.Target.Project = context.TargetProjectName;
            json.Target.PersonalAccessToken = context.ServerPats.FirstOrDefault(x => x.Key.ToLower() == context.TargetServerName.ToLower()).Value;
            json.Processors.WIQLQueryBit = context.QueryText;

            string result = JsonConvert.SerializeObject(json, Formatting.Indented);
            string subject = "\"\\\\MigrationInbox\"";

            result = result.Replace(subject, $"\"\\\\MigrationInbox\\\\{context.WorkItemProjectName}\"");

            File.WriteAllText(generalConfigFilePath, result);
        }

        public static void UpdateAttachmentConfig(IContext context, string generalConfigFilePath)
        {
            //NEED IMPLEMENTATION
        }
    }
}
