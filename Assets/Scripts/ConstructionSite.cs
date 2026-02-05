using System.Collections;
using Towers;
using UnityEngine;

public class ConstructionSite : MonoBehaviour, IClickable
{
    [SerializeField] private Tower baseTower;
    private GameObject notEnoughCog;

    private void Start()
    {
        notEnoughCog = GameObject.Find("NotEnoughCog");
        notEnoughCog.SetActive(false);
    }
    public void Clicked()
    {
        if (baseTower.towerStats.upgradeCost > GameManager.Instance.currentResources)
        {
            StartCoroutine(InsufficientResources());
            return;
        }

        GameManager.Instance.currentResources -= Mathf.FloorToInt(baseTower.towerStats.upgradeCost);
        Instantiate(baseTower, transform.position, transform.rotation, transform.parent);
        gameObject.SetActive(false);
    }

    public void Selected() { }

    public void DeSelected() { }

    IEnumerator InsufficientResources()
    {
        notEnoughCog?.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        notEnoughCog?.SetActive(false);
    }
}
