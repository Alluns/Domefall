using System.IO;
using UnityEngine;

public static class JsonSave
{
    private static string Path => Application.persistentDataPath + "/" + FileName + ".json";

    private const string FileName = "Gp2_SaveData";

    public static void Save(SaveData data)
    {
        string jsonString = JsonUtility.ToJson(data);
        File.WriteAllText(Path, jsonString);
    }

    public static SaveData LoadData()
    {
        if (!File.Exists(Path)) Save(new SaveData());
        
        string jsonString = File.ReadAllText(Path);
        
        return JsonUtility.FromJson<SaveData>(jsonString);
    }

    public static void DeleteSave()
    {
        Save(new SaveData());
    }
}