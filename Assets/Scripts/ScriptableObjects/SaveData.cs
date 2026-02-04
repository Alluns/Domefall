using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Scriptable Objects/SaveData")]
public class SaveData : ScriptableObject
{
    
    public int evoPoints;
    
    public HashSet<string> upgrades = new();
    public List<int> levelsCompleted;

}
