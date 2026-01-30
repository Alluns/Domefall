using System;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                throw new Exception("2 singleton of the same [GamesManager exist]");
            }
            Instance = this;
        }
    }
}
