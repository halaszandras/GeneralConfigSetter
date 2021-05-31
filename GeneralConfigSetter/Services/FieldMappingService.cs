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
            "TechStory2Feature",
            "TechStory2PBI",
            "US2Feature",
            "Issue2Bug",
            "Issue2Impediment",
            "Issue2PBI",
            "Bug2Bug",
            "RQ2Bug_SINUMERIK",
            "RQ2Bug_SINAMICS",
            "Issue2Task"
        };

        public static readonly List<string> DiFaPaModes = new()
        {
            "Feature2PBI",
            "Feature2Feature",
            "US2PBI",
            "Issue2Bug",
            "Issue2Impediment",
            "Issue2PBI",
            "Bug2Bug",
            "Task2Task",
            "Requirement2Epic",
            "Requirement2Global Epic"
        };

        public static readonly List<string> DiMcModes = new()
        {
            "Feature2PBI",
            "Feature2Feature",
            "US2PBI",
            "US2Feature",
            "TechStory2PBI",
            "TechStory2Feature",
            "Opportunity2Opportunity",
            "Idef2Bug",
            "Issue2Bug",
            "Issue2Impediment",
            "Issue2PBI",
            "Issue2Task",
            "Bug2Bug",
            "RQ2Bug_SINUMERIK",
            "RQ2Bug_SINAMICS"
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
            return mode switch
            {
                "Feature2PBI" => Feature2Pbi(strToken),
                "Feature2Feature" => Feature2Feature(strToken),
                "US2PBI" => US2PBI(strToken),
                "TechStory2PBI" => TechStory2PBI(strToken),
                "Opportunity2Opportunity" => Opportunity2Opportunity(strToken),
                "Idef2Bug" => Idef2Bug(strToken),
                "TechStory2Feature" => TechStory2Feature(strToken),
                "US2Feature" => US2Feature(strToken),
                "Issue2Bug" => Issue2Bug(strToken),
                "Issue2Impediment" => Issue2Impediment(strToken),
                "Issue2PBI" => Issue2PBI(strToken),
                "Bug2Bug" => Bug2Bug(strToken),
                "RQ2Bug_SINUMERIK" => RQ2Bug_SINUMERIK(strToken),
                "RQ2Bug_SINAMICS" => RQ2Bug_SINAMICS(strToken),
                "Issue2Task" => Issue2Task(strToken),
                _ => throw new Exception(),
            };
        }

        private static bool TechStory2Feature(string strToken)
        {
            return strToken.Contains("Technical Story") && strToken.Contains("Target WIT: Feature");
        }

        private static bool US2Feature(string strToken)
        {
            return strToken.Contains("User Story") && strToken.Contains("Target WIT: Feature");
        }

        private static bool Idef2Bug(string strToken)
        {
            return strToken.Contains("Internal Defect") && strToken.Contains("target WIT: Bug");
        }

        private static bool Opportunity2Opportunity(string strToken)
        {
            return Regex.Matches(strToken, "Opportunity").Count == 2;
        }

        private static bool US2PBI(string strToken)
        {
            return strToken.Contains("User Story") && strToken.Contains("PBI");
        }

        private static bool TechStory2PBI(string strToken)
        {
            return strToken.Contains("Technical Story") && strToken.Contains("PBI");
        }

        private static bool Feature2Feature(string strToken)
        {
            foreach (string part in strToken.Split(';'))
            {
                if (Regex.Matches(part, "Feature").Count == 2 && !part.Contains("PBI"))
                    return true;
            }
            return false;
        }

        private static bool Feature2Pbi(string strToken)
        {
            return strToken.Contains("Feature") && strToken.Contains("PBI");
        }

        private static bool Issue2Bug(string strToken)
        {
            return strToken.Contains("Issue2Bug");
        }

        private static bool Issue2Impediment(string strToken)
        {
            return strToken.Contains("Issue2Impediment");
        }

        private static bool Issue2PBI(string strToken)
        {
            return strToken.Contains("Issue2PBI");
        }

        private static bool Issue2Task(string strToken)
        {
            return strToken.Contains("Issue2Task");
        }

        private static bool Bug2Bug(string strToken)
        {
            return strToken.Contains("Bug2Bug");
        }

        private static bool RQ2Bug_SINUMERIK(string strToken)
        {
            return strToken.Contains("Request") && strToken.Contains("SINUMERIK");
        }

        private static bool RQ2Bug_SINAMICS(string strToken)
        {
            return strToken.Contains("Request") && strToken.Contains("SINAMICS");
        }
    }
}