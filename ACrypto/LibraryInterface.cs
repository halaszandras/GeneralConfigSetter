using System.Collections.Generic;
using System.IO;

namespace ACrypto
{
    public class LibraryInterface
    {
        public void InitializeCryptedFiles(string patConfigFilePath, Dictionary<string, string> patDictionary, string repoConfigFilePath, Dictionary<string, string> repoDictionary)
        {
            if (!File.Exists(Configuration.KEY_LOCATION))
            {
                new KeyManager().GenerateKey();
                if (File.Exists(patConfigFilePath))
                {
                    EncryptDictionary(patDictionary, patConfigFilePath);
                }
                if (File.Exists(repoConfigFilePath))
                {
                    EncryptDictionary(repoDictionary, repoConfigFilePath);
                }
            }
        }

        public void EncryptDictionary(Dictionary<string, string> dictionary, string filePath)
        {
            DataManager dataManager = new();
            Dictionary<char, string> keyDictionary = dataManager.ReadKey();
            string encodedPats = new Encrypter().EncodePats(keyDictionary, dictionary);
            dataManager.CreateFile(filePath, encodedPats);
        }

        public Dictionary<string, string> DecryptDictionary(string filePath)
        {
            Dictionary<char, string> keyDictionary = new DataManager().ReadKey();
            return new Encrypter().DecodePats(keyDictionary, filePath);
        }
    }
}
