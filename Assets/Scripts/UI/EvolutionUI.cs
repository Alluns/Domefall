using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EvolutionUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> trees = new();
    [SerializeField] private int activeTree;
    [SerializeField] private TMP_Text evolutionPointText;

    private int evolutionPoints;

    public int EvolutionPoints
    {
        set
        {
            evolutionPoints = value;
            evolutionPointText.text = $"Points: {evolutionPoints}";
        }
    }

    private int ActiveTree
    {
        get => activeTree;
        set
        {
            trees[activeTree].SetActive(false);
            activeTree = value;
            trees[activeTree].SetActive(true);
        }
    }
    
    private void OnEnable()
    {
        EvolutionPoints = JsonSave.LoadData().evolutionPoints;
        
        if (trees.Count == 0) return;
        
        foreach (GameObject tree in trees)
        {
            tree.SetActive(false);
        }
        
        trees[ActiveTree].SetActive(true);
    }

    private void Update()
    {
        SaveData data = JsonSave.LoadData();
        EvolutionPoints = data.evolutionPoints;

        if (evolutionPoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void ShiftTree(int steps)
    {
        int nextValue = (ActiveTree + steps) % trees.Count;

        while (nextValue < 0)
        {
            nextValue += trees.Count;
        }
        
        ActiveTree = nextValue;
    }
}
