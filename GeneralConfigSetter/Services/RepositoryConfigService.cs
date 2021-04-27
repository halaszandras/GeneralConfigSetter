using System.Collections.Generic;

namespace GeneralConfigSetter.Services
{
    public static class RepositoryConfigService
    {
        const char COLON = ':';
        const string EMPTY = "";

        public static Dictionary<string, string> GetRepositories(string[] repositoryConfigContent)
        {
            Dictionary<string, string> result = new();
            result.Add("defaultKey", "111LinkIsNull111");

            for (var i = 0; i < repositoryConfigContent.Length; i++)
            {
                if (repositoryConfigContent[i] != EMPTY)
                {
                    if (repositoryConfigContent[i].Contains(COLON))
                    {
                        result.Add(repositoryConfigContent[i].Trim(COLON), repositoryConfigContent[i + 1]);
                    }
                }
            }
            return result;
        }
    }
}
