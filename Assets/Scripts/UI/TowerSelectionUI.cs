using Towers;
using UnityEngine;

namespace UI
{
    public class TowerSelectionUI : MonoBehaviour
    {
        public static TowerSelectionUI Instance;
        
        [SerializeField] private Tower[] towers;

        public ConstructionSite site;
        
        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                throw new System.Exception("2 singleton of the same [GamesManager exist]");
            }
            Instance = this;
            
            gameObject.SetActive(false);
        }

        public void BuildTower(Tower tower)
        {
            Instantiate(tower, site.transform.position, site.transform.rotation);
            
            site.gameObject.SetActive(false);
            
            CloseMenu();
        }
        
        public void CloseMenu()
        {
            gameObject.SetActive(false);
        }
    }
}
