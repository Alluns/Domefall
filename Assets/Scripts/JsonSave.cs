using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using Unity.Collections;
using UnityEditor.Embree;
using UnityEngine;

public class JsonSave : MonoBehaviour
{
    string path;
    string fileName = "Gp2_SaveData";
    
    
    public void SaveData()
    {
        string jsonString = JsonUtility.ToJson(GameManager.Instance.saveData);
        MakePath();
        File.WriteAllText(path, jsonString);
    }
    public void LoadData()
    {
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            GameManager.Instance.saveData = JsonUtility.FromJson<SaveData>(jsonString);
        }
        else SaveData();
    }
    void MakePath()
    {
        path = Application.persistentDataPath + "/" + fileName + ".json";
    }
}
