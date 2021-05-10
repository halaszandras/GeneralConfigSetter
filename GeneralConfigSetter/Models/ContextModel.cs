using System;
using System.Collections.Generic;
using System.Diagnostics;
using ACrypto;
using static GeneralConfigSetter.Services.DataAccessService;

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
            InitializeFiles();
        }

        public void InitializePats()
        {
            var patConfigFilePath = GetPatConfigFilePath();
            try
            {
                ServerPats = AccessConfigContent(patConfigFilePath);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void InitializeRepositories()
        {
            var repositoryConfigFilePath = GetRepositoryConfigFilePath();
            try
            {
                ServerRepositories = AccessConfigContent(repositoryConfigFilePath);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void InitializeFiles()
        {
            Dictionary<string, string> patConfig = new();
            string patConfigFilePath = "";
            Dictionary<string, string> repoConfig = new();
            string repositoryConfigFilePath = "";

            try
            {
                patConfigFilePath = GetPatConfigFilePath();
                patConfig = AccessRawConfigContent(patConfigFilePath);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
            
            try
            {
                repositoryConfigFilePath = GetRepositoryConfigFilePath();
                repoConfig = AccessRawConfigContent(repositoryConfigFilePath);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            if (patConfig.Count > 0 && repoConfig.Count > 0)
            {
                new LibraryInterface().InitializeCryptedFiles(patConfigFilePath, patConfig, repositoryConfigFilePath,
                    repoConfig);
            }
        }
    }
}
