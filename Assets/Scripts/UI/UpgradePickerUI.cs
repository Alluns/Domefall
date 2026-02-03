using Random = UnityEngine.Random;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Towers;
using Input;

namespace UI
{
    public class UpgradePickerUI : MonoBehaviour
    {
        public GameObject upgradeButton;
        
        private const int UpgradeCount = 3;
        
        private void OnEnable()
        {
            GenerateUpgrades(TouchInteraction.Instance.selectedObject.GetComponent<Tower>().towerStats.upgradePool);
        }

        private void OnDisable()
        {
            foreach (Transform child in transform.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }

        private void GenerateUpgrades(List<TowerUpgrade> upgrades)
        {
            List<TowerUpgrade> tempUpgrades = new (upgrades);
            
            for (int i = 0; i < UpgradeCount; i++)
            {
                if (tempUpgrades.Count == 0) break;

                GameObject button = Instantiate(upgradeButton, transform.Find("Upgrades"));
                TowerUpgrade randomUpgrade = tempUpgrades[Random.Range(0, tempUpgrades.Count)];
                
                button.GetComponent<UpgradeButton>().upgrade = randomUpgrade;
                
                tempUpgrades.Remove(randomUpgrade);
            }
        }
    }
}
