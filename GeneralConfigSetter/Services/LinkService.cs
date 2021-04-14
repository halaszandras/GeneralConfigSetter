using GeneralConfigSetter.Models;
using System;
using System.Collections.Generic;

namespace GeneralConfigSetter.Services
{
    public static class LinkService
    {
        public static void GetSourceAndTargetData(IContext context, string source, string target)
        {            
            context.SourceServerName = GetServer(source);
            context.TargetServerName = GetServer(target);

            context.SourceCollectionUrl = GetLink(source);
            context.TargetCollectionUrl = GetLink(target);

            context.SourceProjectName = GetProject(source);
            context.TargetProjectName = GetProject(target);
        }

        public static void GetTargetData(IContext context, string target)
        {            
            context.TargetServerName = GetServer(target);
            context.TargetCollectionUrl = GetLink(target);
            context.TargetProjectName = GetProject(target);
        }

        private static String GetProject(string rawLink)
        {
            List<string> parts = new(rawLink.Split("/"));

            if (parts.Count != 1)
            {
                while (parts[(parts.Count - 1) - 2] != "tfs")
                {
                    parts.RemoveAt(parts.Count - 1);
                }
                return parts[^1].Replace("%20", " ")
                                .Replace("%28", "(")
                                .Replace("%29", ")");
            }
            return "<Empty Project!!!>";
        }

        private static String GetLink(string rawLink)
        {
            List<string> parts = new(rawLink.Split("/"));

            if (parts.Count != 1)
            {
                while (parts[(parts.Count - 1) - 1] != "tfs")
                {
                    parts.RemoveAt(parts.Count - 1);
                }
                return String.Join('/', parts);
            }
            return "<Empty Collection!!!>";
        }

        private static String GetServer(string rawLink)
        {
            if (rawLink.Contains('/'))
            {
                List<string> parts = new(rawLink.Split("."));
                return parts[0].Split("//")[1];
            }
            return "defaultKey";
        }
    }
}
