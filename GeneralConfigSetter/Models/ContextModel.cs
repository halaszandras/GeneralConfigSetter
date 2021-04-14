using System.Collections.Generic;
using static GeneralConfigSetter.Services.DataAccessService;
using static GeneralConfigSetter.Services.PatService;
using static GeneralConfigSetter.Services.RepositoryConfigService;

namespace GeneralConfigSetter.Models
{
    public class ContextModel : IContext
    {
        public Dictionary<string, string> ServerPats { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ServerRepositories { get; set; } = new Dictionary<string, string>();
        public string WorkItemProjectName { get; set; }
        public string QueryText { get; set; }
        public string SourceServerName { get; set; }
        public string SourceCollectionUrl { get; set; }
        public string SourceProjectName { get; set; }
        public string TargetServerName { get; set; }
        public string TargetCollectionUrl { get; set; }
        public string TargetProjectName { get; set; }

        public ContextModel()
        {

        }

        public void InitializePats()
        {
            var patConfigFilePath = GetPatConfigFilePath();
            var patConfigContent = AccessConfigContent(patConfigFilePath);
            ServerPats = GetPats(patConfigContent);
        }

        public void InitializeRepositories()
        {
            var repositoryConfigFilePath = GetRepositoryConfigFilePath();
            var repositoryConfigContent = AccessConfigContent(repositoryConfigFilePath);
            ServerRepositories = GetRepositories(repositoryConfigContent);
        }
    }
}
