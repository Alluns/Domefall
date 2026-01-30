using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UI
{
    public class UpgradePickerUI : MonoBehaviour
    {
        public static UpgradePickerUI Instance;

        public Tower selectedTower;
        private List<UpgradePool.Upgrade> randomUpgrades;
        private Button[] upgrades;
        UpgradePool pool;
        private string[] cheating = new[]
        {
            "+10 damage",
            "+3 range",
            "+1 attack speed"
        };
        
        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                throw new System.Exception("2 singleton of the same [GamesManager exist]");
            }
            Instance = this;
            upgrades = GetComponentsInChildren<Button>();
            pool = GetComponent<UpgradePool>();
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            // Get random upgrades
            randomUpgrades = pool.GetRandomUpgrades(upgrades.Length);
            Debug.Log(upgrades.Length);
            Debug.Log(randomUpgrades.Count);
            for (int i = 0; i < randomUpgrades.Count; i++)
            {
                Debug.Log(i);
                upgrades[i].onClick.RemoveAllListeners();
                upgrades[i].onClick.AddListener(UpgradeButtonClicked);
                upgrades[i].GetComponentInChildren<TMP_Text>().text = $"+{randomUpgrades[i].upgradeAmount} {randomUpgrades[i].stat}";
                int capturedIndex = i;
                upgrades[i].onClick.AddListener(() => UpgradeTower(selectedTower, randomUpgrades[capturedIndex]));
            }

        }

        private void UpgradeButtonClicked()
        {

            gameObject.SetActive(false);
        }

        public void UpgradeTower(Tower tower, UpgradePool.Upgrade upgrade)
        {
            //Change later !!!
            if (upgrade.stat == UpgradePool.Upgrade.StatTypes.dmg)
            {
                tower.damage += upgrade.upgradeAmount;
            }
            else if (upgrade.stat == UpgradePool.Upgrade.StatTypes.range)
            {
                tower.range += upgrade.upgradeAmount;
            }
            else if (upgrade.stat == UpgradePool.Upgrade.StatTypes.attackSpeed)
            {
                tower.attackSpeed += upgrade.upgradeAmount;
            }
        }

    }
}
