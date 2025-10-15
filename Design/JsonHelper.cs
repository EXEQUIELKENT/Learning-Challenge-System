using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public static class JsonHelper
{
    // Method to read JSON from a file
    public static Dictionary<string, object> ReadJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
        else
        {
            return new Dictionary<string, object>(); // Return empty if file doesn't exist
        }
    }

    // Method to write JSON to a file
    public static void WriteJson(string filePath, Dictionary<string, object> data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}

