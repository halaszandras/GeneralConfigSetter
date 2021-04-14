using System.Collections.Generic;

namespace GeneralConfigSetter.Models
{
    public interface IContext
    {
        string QueryText { get; set; }
        Dictionary<string, string> ServerPats { get; set; }
        Dictionary<string, string> ServerRepositories { get; set; }
        string SourceCollectionUrl { get; set; }
        string SourceProjectName { get; set; }
        string SourceServerName { get; set; }
        string TargetCollectionUrl { get; set; }
        string TargetProjectName { get; set; }
        string TargetServerName { get; set; }
        string WorkItemProjectName { get; set; }

        void InitializePats();
        void InitializeRepositories();
    }
}