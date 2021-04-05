using GeneralConfigSetter.Services;
using System.Collections.Generic;
using static GeneralConfigSetter.Services.DataAccessService;

namespace GeneralConfigSetter.Models
{
    public class ContextModel : IContext
    {
        public Dictionary<string, string> ServerPats { get; set; } = new Dictionary<string, string>();
        public string WorkItemProjectName { get; set; }
        public string SourceServerName { get; set; }
        public string SourceCollectionUrl { get; set; }
        public string SourceProjectName { get; set; }
        public string TargetServerName { get; set; }
        public string TargetCollectionUrl { get; set; }
        public string TargetProjectName { get; set; }

        public ContextModel()
        {

        }

        public void Initialize()
        {
            var patConfigFilePath = GetPatConfigFilePath();
            var patConfigContent = AccessPatConfigContent(patConfigFilePath);
            ServerPats = PatService.GetPats(patConfigContent);
        }
    }
}
