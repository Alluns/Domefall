using System;
using TMPro;
using UnityEngine;

public class EvolutionConfirmationUI : MonoBehaviour
{
    [SerializeField] private TMP_Text title, description;

    public static EvolutionConfirmationUI Instance;
    
    public EvolutionNode node;

    public string Title
    {
        get => title.text;
        set => title.text = value;
    }

    public string Description
    {
        get => description.text;
        set => description.text = value;
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            throw new Exception("2 singleton of the same [EvolutionConfirmationUI exist]");
        }
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Evolve()
    {
        node.BuyEvolution();
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
