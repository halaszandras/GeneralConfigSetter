using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACrypto
{
    class KeyManager
    {
        readonly Random _random = new Random();
        readonly DataManager _data = new DataManager();

        public void GenerateKey()
        {
            Dictionary<char, string> data = CreateDictionary();
            List<string> valuePairs = new List<string>();

            foreach (KeyValuePair<char, string> item in data)
            {
                valuePairs.Add(
                    $"{item.Value}" +
                    $"{Configuration.BINDER}" +
                    $"{item.Key}"
                );
            }
            _data.CreateFile(Configuration.KEY_LOCATION, String.Join(Configuration.SEPARATOR, valuePairs));

        }

        private Dictionary<char, string> CreateDictionary()
        {
            Boolean IsValidDecimal(int index)
            {
                int[] invalidDecimals = new int[] { 58, 59, 60, 61, 62, 63, 64, 91, 92, 93, 94, 95, 96 };
                return !invalidDecimals.Contains(index);
            }

            Dictionary<char, string> result = new Dictionary<char, string>();

            foreach (char c in GetShuffledKeys())
            {
                if (IsValidDecimal(c))
                {
                    result.Add(
                        c,
                        GetRandomCharacters(result)
                    );
                }
            }
            return result;
        }

        private String GetRandomCharacters(Dictionary<char, string> data)
        {
            Boolean IsValidValue(string value)
            {
                foreach (KeyValuePair<char, string> item in data)
                {
                    if (value.Equals(item.Value))
                        return false;
                }
                return true;
            }

            string result = "";
            bool check = false;

            while (!check)
            {
                result = "";
                while (result.Length != Configuration.MAX_LENGTH)
                {
                    char c = (char)_random.Next(33, 126);
                    if (IsValidCharacter(c))
                        result += c;
                }
                check = IsValidValue(result);
            }
            return result;
        }

        private Boolean IsValidCharacter(char c)
        {
            return !c.Equals(Configuration.BINDER) && !c.Equals(Configuration.SEPARATOR);
        }

        private List<char> GetShuffledKeys()
        {
            List<char> temp = new List<char>(Configuration.CHARACTERS);
            List<char> result = new List<char>();

            while (result.Count != Configuration.CHARACTERS.Count)
            {
                int index = _random.Next(temp.Count);
                result.Add(temp[index]);
                temp.RemoveAt(index);
            }
            return result;
        }
    }
}
