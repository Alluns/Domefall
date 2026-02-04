using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [field: SerializeField] public List<LevelButton> Parents { get; private set; }
    [field: SerializeField] public LevelStatus Status { get; set; }
    
    public enum LevelStatus
    {
        Locked,
        Open,
        Completed
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
    }
    
    public void LoadScene(string sceneName)
    {
        if (Status != LevelStatus.Open) return;
        
        SceneManager.LoadScene(sceneName);
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
