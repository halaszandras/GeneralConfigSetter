using System;
using System.Collections.Generic;
using System.IO;

namespace ACrypto
{
    class DataManager
    {
        public void CreateFile(string filename, string content)
        {
            CheckFileAndFolder(filename);
            File.SetAttributes(filename, FileAttributes.Normal);
            File.WriteAllText(filename, content);
            File.SetAttributes(filename, FileAttributes.Hidden);
        }

        public String GetEncodedPats(string patFilePath)
        {
            return File.ReadAllText(patFilePath);
        }

        public Dictionary<char, string> ReadKey()
        {
            if (!File.Exists(Configuration.KEY_LOCATION))
            {
                new KeyManager().GenerateKey();
            }

            Dictionary<char, string> result = new Dictionary<char, string>();

            string key = File.ReadAllText(Configuration.KEY_LOCATION);
            string[] splittedKey = key.Split(Configuration.SEPARATOR);

            foreach (string item in splittedKey)
            {
                string[] temp = item.Split(Configuration.BINDER);
                result.Add(
                    Char.Parse(temp[1]),
                    temp[0]
                );
            }
            return result;
        }

        public static void CheckFileAndFolder(string filePath)
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
    }
}
