using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace GeneralConfigSetter.Services
{
    public static class WitLockingService
    {
        public static void Lock(string[] filePaths)
        {
            foreach (string filePath in filePaths)
            {
                dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(filePath));
                dynamic processors = json.Processors;

                try
                {
                    foreach (dynamic processor in processors)
                    {
                        try
                        {
                            processor.LockSourceItem.Value = true;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

                string result = JsonConvert.SerializeObject(json, Formatting.Indented);

                File.WriteAllText(filePath, result);
            }
        }
    }
}
