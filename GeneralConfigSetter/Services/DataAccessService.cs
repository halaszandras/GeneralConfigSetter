using System;
using System.IO;

namespace GeneralConfigSetter.Services
{
    public static class DataAccessService
    {
        public static string[] AccessConfigContent(string filePath)
        {
            string[] fileContent = File.ReadAllLines(filePath);

            return fileContent;
        }

        public static string GetConfigFileContent(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);
            return fileContent;
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
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            bool fileExists = File.Exists(filePath);

            if (!fileExists)
            {
                File.Create(filePath).Dispose();
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
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            try
            {
                using StreamWriter streamWriter = new(filePath);
                streamWriter.Write(newContent);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
        }
    }
}
