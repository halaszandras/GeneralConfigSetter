using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ACrypto;
using static GeneralConfigSetter.Services.ConfigUpdateService;

namespace GeneralConfigSetter.Services
{
    public static class DataAccessService
    {
        public static Dictionary<string, string> AccessConfigContent(string filePath)
        {
            Dictionary<string, string> fileContent = new ACrypto.LibraryInterface().DecryptDictionary(filePath);

            return fileContent;
        }

        public static Dictionary<string, string> AccessRawConfigContent(string filePath)
        {
            Dictionary<string, string> configDictionary = PatService.GetPats(File.ReadAllLines(filePath));
            configDictionary.Remove("defaultKey");
            return configDictionary;
        }

        public static string GetConfigFileContent(string filePath)
        {
            StringBuilder stringBuilder = new();

            Dictionary<string, string> fileContentDictionary = AccessConfigContent(filePath);
            foreach (var stringPair in fileContentDictionary)
            {
                stringBuilder.Append($"{stringPair.Key}:{Environment.NewLine}{stringPair.Value}{Environment.NewLine}{Environment.NewLine}");
            }
            
            return stringBuilder.ToString();
        }

        public static string GetPatConfigFilePath()
        {
            string runningPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $"{runningPath}Resources\\PatConfig.txt";

            CheckConfigFileAndFolder(filePath);

            return filePath;
        }

        public static string GetRepositoryConfigFilePath()
        {
            string runningPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $"{runningPath}Resources\\RepositoryConfig.txt";

            CheckConfigFileAndFolder(filePath);

            return filePath;
        }

        public static void CheckConfigFileAndFolder(string filePath)
        {
            bool folderExists = Directory.Exists(Path.GetDirectoryName(filePath));

            if (!folderExists)
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                directoryInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            bool fileExists = File.Exists(filePath);

            if (!fileExists)
            {
                File.Create(filePath).Dispose();
                File.SetAttributes(filePath, FileAttributes.Hidden);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newContent"></param>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="IOException"/>
        public static void UpdateConfigFile(string filePath, string newContent)
        {
            string validNewContent = ValidateUpdateInput(newContent);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            try
            {
                new LibraryInterface().EncryptDictionary(PatService.GetPats(validNewContent), filePath);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
        }
    }
}
