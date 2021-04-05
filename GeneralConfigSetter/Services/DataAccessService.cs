using System;
using System.IO;

namespace GeneralConfigSetter.Services
{
    public static class DataAccessService
    {
        public static string[] AccessPatConfigContent(string filePath)
        {
            string[] fileContent = File.ReadAllLines(filePath);

            return fileContent;
        }

        public static string GetPatConfigRawContent(string filePath)
        {
            string rawContent = File.ReadAllText(filePath);
            return rawContent;
        }

        public static string GetPatConfigFilePath()
        {
            string runningPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $"{runningPath}Resources\\PatConfig.txt";

            CheckPatConfigFileAndFolder(filePath);

            return filePath;
        }

        public static void CheckPatConfigFileAndFolder(string filePath)
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

        public static void UpdatePatConfig(string filePath, string newContent)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using StreamWriter streamWriter = new(filePath);
            streamWriter.Write(newContent);
        }
    }
}
