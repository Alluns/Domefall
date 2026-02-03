using ScriptableObjects;
using UnityEngine.UI;
using UnityEngine;
using Managers;
using Towers;
using Input;
using TMPro;

namespace UI
{
    public class UpgradeButton : MonoBehaviour
    {
        public TowerUpgrade upgrade;

        private void Start()
        {
            GetComponentInChildren<TMP_Text>().text = upgrade?.upgradeName;
            
            GetComponent<Button>().onClick.AddListener(OnButtonClicked);
        }
        
        private void OnButtonClicked()
        {
            Tower tower = TouchInteraction.Instance.selectedObject.GetComponent<Tower>();

            if (tower) tower.AddUpgrade(upgrade);

            UIManager.Instance.CloseLastUI();
        }
    }
}