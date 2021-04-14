using System.Collections.Generic;

namespace GeneralConfigSetter.Services
{
    public static class RepositoryConfigService
    {
        const char COLON = ':';
        const string EMPTY = "";

        public static Dictionary<string, string> GetRepositories(string[] patConfigContent)
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
    }
}
