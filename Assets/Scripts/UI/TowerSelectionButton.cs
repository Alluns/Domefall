using Input;
using Managers;
using ScriptableObjects;
using Towers;
using UnityEngine;

namespace UI
{
    public class TowerSelectionButton : MonoBehaviour
    {
        [SerializeField] private GameObject towerPrefab;

        public void SelectTower()
        {
            Tower tower = TouchInteraction.Instance.selectedObject.GetComponent<Tower>();

            Tower newTower = Instantiate(towerPrefab, tower.transform.position, tower.transform.rotation, tower.transform.parent).GetComponent<Tower>();

            foreach (TowerUpgrade upgrade in tower.Upgrades)
            {
                newTower.AddUpgrade(upgrade);
            }
            
            Destroy(tower.gameObject);
            
            GameManager.Instance.SwitchState(GameManager.GameState.Playing);
            UIManager.Instance.CloseLastUI();
        }
    }
}
