using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Managers
{
    public interface IMenu {}
    
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
                    transform.Find("Upgrade Menu").GetComponent<UpgradePickerUI>().gameObject.SetActive(true);
                    menus.Push(transform.Find("Upgrade Menu").gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(menu), menu, null);
            }
        }

        public void CloseLastUI()
        {
            menus.Pop()?.gameObject.SetActive(false);
        }
    }

    public enum Menus
    {
        UpgradeMenu,
    }
}
