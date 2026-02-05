using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [field: SerializeField] public List<LevelButton> Parents { get; private set; }
    [field: SerializeField] public LevelStatus Status { get; set; }
    [field: SerializeField] public int LevelIndex { get; private set; }
    
    public enum LevelStatus
    {
        Locked,
        Open,
        Completed
    }

    private void OnEnable()
    {
        if (JsonSave.LoadData().levelsCompleted.Contains(LevelIndex))
        {
            Status = LevelStatus.Completed;
        }
    }

    private void Start()
    {
        if (Status == LevelStatus.Completed) return;

        if (Parents.Count == 0)
        {
            Status = LevelStatus.Open;
            return;
        }

        Status = Parents.Exists(p => p.Status == LevelStatus.Completed) ? LevelStatus.Open : LevelStatus.Locked;

        GetComponent<Button>().interactable = Status switch
        {
            LevelStatus.Locked or LevelStatus.Completed => false,
            LevelStatus.Open => true,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public void LoadScene()
    {
        if (Status != LevelStatus.Open) return;
        
        SceneManager.LoadScene(LevelIndex);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Parents.Count == 0) return;
        
        Gizmos.color = Color.rebeccaPurple;
        
        foreach (LevelButton parent in Parents)
        {
            if (parent == null) continue;
            
            Gizmos.DrawLine(transform.position, parent.transform.position);
        }
    }
#endif
}
