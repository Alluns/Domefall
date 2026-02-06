using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        private Stack<GameObject> menus = new();

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                throw new Exception("2 singleton of the same [GamesManager exist]");
            }
            Instance = this;
        }

        public void OpenUI(Menus menu)
        {
            switch (menu)
            {
                case Menus.UpgradeMenu:
                    transform.Find("Upgrade Menu").gameObject.SetActive(true);
                    menus.Push(transform.Find("Upgrade Menu").gameObject);
                    break;
                case Menus.TowerSelectionMenu:
                    transform.Find("Tower Selection").gameObject.SetActive(true);
                    menus.Push(transform.Find("Tower Selection").gameObject);
                    break;
                case Menus.PauseScreen:
                    transform.Find("Pause Screen").gameObject.SetActive(true);
                    menus.Push(transform.Find("Pause Screen").gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(menu), menu, null);
            }
        }

        public void OpenUI(int menuIndex)
        {
            OpenUI((Menus) menuIndex);
        }

        public void CloseLastUI()
        {
            menus.Pop()?.gameObject.SetActive(false);
        }
    }

    [Serializable] public enum Menus
    {
        UpgradeMenu,
        TowerSelectionMenu,
        PauseScreen
    }
}
