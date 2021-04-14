using System;
using System.Collections.Generic;
using System.IO;

namespace GeneralConfigSetter.Services
{
    public static class PatService
    {
        const char COLON = ':';
        const string EMPTY = "";

        public static Dictionary<string, string> GetPats(string[] patConfigContent)
        {
            Dictionary<string, string> result = new();
            result.Add("defaultKey", "<Invalid link!!!>");

            for (var i = 0; i < patConfigContent.Length; i++)
            {
                if (patConfigContent[i] != EMPTY)
                {
                    if (patConfigContent[i].Contains(COLON))
                    {
                        result.Add(patConfigContent[i].Trim(COLON), patConfigContent[i + 1]);
                    }
                }
            }
            return result;
        }

        public static int PatComparedToToday(string filePath)
        {
            DateTime lastModification = File.GetLastWriteTime(filePath).AddDays(10);
            int compareValue = DateTime.Now.CompareTo(lastModification);
            return compareValue;
        }
    }
}
