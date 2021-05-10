using System;
using System.Collections.Generic;
using System.Linq;

namespace ACrypto
{
    class Encrypter
    {
        DataManager _data = new DataManager();

        public String EncodePats(Dictionary<char, string> keyDictionary, Dictionary<string, string> patDictionary)
        {
            String EncodeString(string content)
            {
                string result = "";
                foreach (char c in content)
                {
                    if (!Configuration.SPECIAL_CHARACTERS.Contains(c))
                    {
                        if (c.Equals('\\'))
                        {
                            result += keyDictionary['/'];
                            continue;
                        }

                        result += Char.IsLower(c)
                            ? keyDictionary[Char.ToUpper(c)]
                            : Reverse(keyDictionary[c]);
                    }
                    else
                    {
                        result += keyDictionary[c];
                    }
                }
                return result;
            }

            List<string> dataPack = new List<string>();

            foreach (KeyValuePair<string, string> item in patDictionary)
            {
                dataPack.Add(
                    EncodeString(item.Value) + Configuration.BINDER + EncodeString(item.Key)
                );
            }
            return String.Join(Configuration.SEPARATOR, dataPack);
        }

        public Dictionary<string, string> DecodePats(Dictionary<char, string> keyDictionary, string patFilePath)
        {
            String[] DecodeData(string[] data)
            {
                for (int n = 0; n < 2; n++)
                {
                    foreach (KeyValuePair<char, string> item in keyDictionary)
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (!Configuration.SPECIAL_CHARACTERS.Contains(item.Key))
                            {
                                if (n == 0) //LowerCase
                                    data[i] = data[i].Replace(item.Value, $"{Char.ToLower(item.Key)}");
                                else //UpperCase
                                    data[i] = data[i].Replace(Reverse(item.Value), $"{item.Key}");
                                continue;
                            }

                            if (!item.Key.Equals('/'))
                            {
                                data[i] = data[i].Replace(item.Value, $"{item.Key}");
                            }
                            else
                            {
                                data[i] = data[i].Replace(item.Value, "\\");
                            }
                        }
                    }
                }
                return data;
            }

            Dictionary<string, string> pats = new Dictionary<string, string>();
            string encodedPats = _data.GetEncodedPats(patFilePath);
            if (encodedPats.Length > 0)
            {
                string[] dataSplit = encodedPats.Split(Configuration.SEPARATOR);

                foreach (string item in dataSplit)
                {
                    string[] temp = item.Split(Configuration.BINDER);
                    string[] decodeTmp = DecodeData(temp);

                    pats.Add(
                        decodeTmp[1],
                        decodeTmp[0]
                    );
                }
            }
            
            return pats;
        }

        private String Reverse(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
