using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using static GeneralConfigSetter.Services.PostConfigService;

namespace GeneralConfigSetter.Services
{
    public static class FieldMappingService
    {
        public static readonly List<string> SupportedModes = new()
        {
            "Feature2PBI",
            "Feature2Feature",
            "US2PBI",
            "Opportunity2Opportunity",
            "Idef2Bug",
            "TechStory2Feature"
        };

        public static readonly List<string> DiFaPaModes = new()
        {
            "Feature2PBI",
            "Feature2Feature",
            "US2PBI"
        };

        public static readonly List<string> DiMcModes = new()
        {
            "Feature2PBI",
            "Feature2Feature",
            "US2PBI",
            "Opportunity2Opportunity",
            "Idef2Bug",
            "TechStory2Feature"
        };

        public static void Do(string configRoute, string mode, string tag)
        {
            JObject json = SetJson(configRoute, mode, tag);
            string targetProp = "WorkItemTypeName",
                   sourceWorkitemType = GetSourceWorkitem(mode);

            if (SupportedModes.Contains(mode))
            {
                foreach (JToken token in json["FieldMaps"])
                {
                    if (token.ToString().Contains(sourceWorkitemType))
                    {
                        foreach (JProperty prop in token)
                        {
                            if (prop.Name.Equals(targetProp))
                            {
                                if (RunMethod(mode, prop.Value))
                                {
                                    token[targetProp] = sourceWorkitemType;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            Save(json, configRoute);
        }

        private static bool RunMethod(string mode, JToken token)
        {
            string strToken = token.ToString();
            switch (mode)
            {
                case "Feature2PBI": return Feature2Pbi(strToken);
                case "Feature2Feature": return Feature2Feature(strToken);
                case "US2PBI": return US2PBI(strToken);
                case "Opportunity2Opportunity": return Opportunity2Opportunity(strToken);
                case "Idef2Bug": return Idef2Bug(strToken);
                case "TechStory2Feature": return TechStory2Feature(strToken);
                default: throw new Exception();
            }
        }

        private static bool TechStory2Feature(string strToken)
        {
            return strToken.Contains("Technical Story") && strToken.Contains("Feature");
        }

        private static bool Idef2Bug(string strToken)
        {
            return strToken.Contains("Internal Defect") && strToken.Contains("Bug");
        }

        private static bool Opportunity2Opportunity(string strToken)
        {
            return Regex.Matches(strToken, "Opportunity").Count == 2;
        }

        private static bool US2PBI(string strToken)
        {
            return strToken.Contains("User Story") && strToken.Contains("PBI");
        }

        private static bool Feature2Feature(string strToken)
        {
            foreach (string part in strToken.Split(';'))
            {
                if (Regex.Matches(part, "Feature").Count == 2)
                    return true;
            }
            return false;
        }

        private static bool Feature2Pbi(string strToken)
        {
            return strToken.Contains("Feature") && strToken.Contains("PBI");
        }
    }
}
