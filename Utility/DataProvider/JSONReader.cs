using System;
using System.IO;
using System.Text.Json;


namespace SpecFlowBDDFramework.Utility.DataProvider
{
    public class JSONFileReader
    {
        public static T ReadJsonFile<T>(string jsonFilepath)
        {
            if (!File.Exists(jsonFilepath))
            {
                throw new FileNotFoundException($"The file at {jsonFilepath} was not found.");
            }

            try
            {
                string jsonString = File.ReadAllText(jsonFilepath);
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to read or deserialize JSON file.", ex);
            }
        }

    }
}
