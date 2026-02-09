using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Linq;

public class EvolutionNode : MonoBehaviour
{
    public bool isStartNode;
    
    [Header("Upgrades")]
    [SerializeField] private string description;
    [SerializeField] private EvolutionType type;
    [SerializeField] private List<EvolutionAttribute> attributes;

    [Header("Sprites")]
    [SerializeField] private Sprite owned;
    [SerializeField] private Sprite available;
    [SerializeField] private Sprite locked;
    
    [Header("Relationships")]
    public List<EvolutionNode> children = new();
    public List<EvolutionNode> conflicts = new();

    private NodeState State
    {
        get => state;
        set
        {
            state = value;
            UpdateSprite();
        }
    }

    private NodeState state;

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
        if (saveData.evolutions.Exists(e => e.name == name)) return NodeState.Owned;
        
        if (conflicts.Any(conflict => conflict.State == NodeState.Owned))
        {
            return NodeState.Locked;
        }
        
        if (parent == null || parent.State == NodeState.Owned) return NodeState.Available;
        
        return NodeState.Locked;
    }

    public void OnUpgrade()
    {
        if (state != NodeState.Available) return;
        
        SaveData saveData = JsonSave.LoadData();
        
        if (saveData.evolutionPoints <= 0) return;

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
        public EvolutionType type;
        public List<EvolutionAttribute> attributes;
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

    [Serializable] public class EvolutionAttribute
    {
        public UpgradeType upgradeType;
        public float additive;
        public float multiplicative = 1;
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
