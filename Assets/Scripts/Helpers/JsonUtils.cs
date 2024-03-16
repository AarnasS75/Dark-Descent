using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public static class JsonUtils
{
    // Write object to JSON file in persistent data path
    public static void WriteToJsonFile<T>(T obj, string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string jsonData = JsonConvert.SerializeObject(obj, Formatting.Indented);
        File.WriteAllText(filePath, jsonData);
    }

    // Read object from JSON file in persistent data path
    public static T ReadFromJsonFile<T>(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return default;
        }
    }
}
