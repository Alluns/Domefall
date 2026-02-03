using Towers;
using UnityEngine;

public class ConstructionSite : MonoBehaviour, IClickable
{
    [SerializeField] private Tower baseTower;
    
    public void Clicked()
    {
        if (baseTower.towerStats.upgradeCost > GameManager.Instance.currentResources) return;

        Instantiate(baseTower, transform.position, transform.rotation, transform.parent);
        gameObject.SetActive(false);
    }

    public void Selected() { }

    public void DeSelected() { }
}
