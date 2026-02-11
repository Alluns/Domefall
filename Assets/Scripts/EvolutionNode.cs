using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;
using System;

public class EvolutionNode : MonoBehaviour
{
    public bool isStartNode;
    
    [Header("Upgrades")]
    [SerializeField] private string description;
    [SerializeField] private EvolutionType type;
    [SerializeField] private List<UpgradeAttribute> attributes;

    [Header("Sprites")]
    [SerializeField] private Sprite owned;
    [SerializeField] private Sprite available;
    [SerializeField] private Sprite locked;
    
    [Header("Relationships")]
    public List<EvolutionNode> children = new();
    public List<EvolutionNode> conflicts = new();

    private NodeState state;
    private NodeState State
    {
        get => state;
        set
        {
            state = value;
            UpdateSprite();
        }
    }
    
    private void Awake()
    {
        State = NodeState.Unassigned;
    }

    private void Start()
    {
        if (isStartNode)
        {
            UpdateTree(JsonSave.LoadData(), null);
        }
    }

    private void UpdateTree(SaveData saveData, EvolutionNode node)
    {
        if (State != NodeState.Unassigned) return;
        
        State = CalculateState(saveData, node);
        
        if (children.Count == 0) return;

        foreach (EvolutionNode child in children)
        {
            child.UpdateTree(saveData, this);
        }
    }

    private NodeState CalculateState(SaveData saveData, EvolutionNode parent)
    {
        if (saveData.evolutions.Exists(e => e.name == name && e.type == type)) return NodeState.Owned;
        
        if (conflicts.Any(conflict => conflict.State == NodeState.Owned))
        {
            return NodeState.Locked;
        }
        
        if (parent == null || parent.State == NodeState.Owned) return NodeState.Available;
        
        return NodeState.Locked;
    }

    public void SelectEvolution()
    {
        if (state != NodeState.Available) return;
        
        SaveData saveData = JsonSave.LoadData();
        
        if (saveData.evolutionPoints <= 0) return;

        EvolutionConfirmationUI.Instance.gameObject.SetActive(true);
        
        EvolutionConfirmationUI.Instance.node = this;
        EvolutionConfirmationUI.Instance.Title = name;
        EvolutionConfirmationUI.Instance.Description = description;
    }
    
    public void BuyEvolution()
    {
        SaveData saveData = JsonSave.LoadData();

        saveData.evolutionPoints--;
        State = NodeState.Owned;
        
        foreach (EvolutionNode child in children)
        {
            child.State = NodeState.Available;
        }

        foreach (EvolutionNode conflict in conflicts)
        {
            conflict.State = NodeState.Locked;
        }

        Evolution evolution = new()
        {
            name = name,
            description = description,
            type = type,
            attributes = attributes
        };
        
        saveData.evolutions.Add(evolution);
        
        JsonSave.Save(saveData);
    }

    private void UpdateSprite()
    {
        gameObject.GetComponent<Image>().sprite = state switch
        {
            NodeState.Owned => owned,
            NodeState.Available => available,
            NodeState.Locked => locked,
            _ => gameObject.GetComponent<Image>().sprite
        };
    }

    private enum NodeState
    {
        Unassigned,
        Owned,
        Available,
        Locked
    }
    
    [Serializable] public struct Evolution
    {
        public string name;
        public string description;
        public EvolutionType type;
        public List<UpgradeAttribute> attributes;
    }
    
    public enum EvolutionType
    {
        Shelter,
        Generic,
        Ground,
        Air,
        AirAndGround,
        Economy
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (children.Count > 0)
        {
            Gizmos.color = Color.yellowNice;
            foreach (EvolutionNode child in children)
            {
                if (child == null) continue;
                Gizmos.DrawLine(transform.position, child.transform.position);
            }
        }
        
        if (conflicts.Count > 0)
        {
            Gizmos.color = Color.softRed;
            foreach (EvolutionNode conflict in conflicts)
            {
                if (conflict == null) continue;
                Gizmos.DrawLine(transform.position, conflict.transform.position);
            }
        }
    }
#endif
}
