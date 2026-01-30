using UI;
using UnityEngine;

public class ConstructionSite : MonoBehaviour, IClickable
{
    public void Clicked()
    {
        TowerSelectionUI.Instance.site = this;
        TowerSelectionUI.Instance.gameObject.SetActive(true);
    }

    public void Selected() { }

    public void DeSelected() { }
}
