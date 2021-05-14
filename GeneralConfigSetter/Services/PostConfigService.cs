using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeneralConfigSetter.Services
{
    public static class PostConfigService
    {
        private static JObject _json;

        static readonly Dictionary<string, string> KeyWords = new()
        {
            {"PBI", "Product Backlog Item"},
            {"PR", "Problem Report"},
            {"RQ", "Request"},
            {"CR", "Change Request"},
            {"Idef", "Internal Defect"},
            {"US", "User Story"},
            {"TechStory", "Technical Story"}
        };

        public static JObject SetJson(string route, string mode, string tag)
        {
            _json = GetJsonConfig(route);
            Set_WorkItemTypeDefinition(mode);
            SetTag(tag);
            return _json;
        }

        public static void Save(JObject json, string route)
        {
            File.WriteAllText(
                route,
                JsonConvert.SerializeObject(json, Formatting.Indented)
            );
        }

        public static String GetSourceWorkitem(string mode)
        {
            return GetValidatedModes(mode)[0];
        }

        private static void SetTag(string tag)
        {
            foreach (JToken token in _json["Processors"])
            {
                if (token.ToString().Contains("WorkItemMigrationConfig"))
                {
                    token["WIQLQueryBit"] = $"AND [System.Tags] contains '{tag}'";
                    break;
                }
            }
        }

        private static void Set_WorkItemTypeDefinition(string mode)
        {
            const string objectName = "WorkItemTypeDefinition";
            JObject definitions = (JObject)_json[objectName];

            void DeleteDefaultFields()
            {
                List<string> removeList = new();

                foreach (KeyValuePair<string, JToken> item in definitions)
                {
                    removeList.Add(item.Key);
                }

                foreach (string item in removeList)
                {
                    definitions.Property(item).Remove();
                }
            }

            string[] modeParts = GetValidatedModes(mode);
            DeleteDefaultFields();

            definitions.Add(modeParts[0], modeParts[1]);
        }

        private static string[] GetValidatedModes(string mode)
        {
            string[] modeParts = mode.Split('2');

            for (int i = 0; i < modeParts.Length; i++)
            {
                if (KeyWords.ContainsKey(modeParts[i]))
                {
                    modeParts[i] = KeyWords[modeParts[i]];
                }
            }
            return modeParts;
        }

        private static JObject GetJsonConfig(string route)
        {
            return JObject.Parse(File.ReadAllText(route));
        }
    }
}
