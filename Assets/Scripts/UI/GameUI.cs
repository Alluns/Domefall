 using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text resourceText, versionText, healthText, speedText;
        private GameObject loseScreen, winScreen;
        private void Start()
        {
            winScreen = transform.Find("Victory Screen").gameObject;
            winScreen.gameObject.SetActive(false);
            loseScreen = transform.Find("Game Over Screen").gameObject;
            loseScreen.gameObject.SetActive(false);

            versionText.text = $"Project {Application.productName} version: {Application.version}";
        
            GameManager.Instance.onGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameManager.GameState gameState)
        {
            switch (gameState)
            {
                case GameManager.GameState.Lose:
                    loseScreen.SetActive(true);
                    break;
                case GameManager.GameState.Win:
                    winScreen.SetActive(true);
                    break;
            }
        }

        private void Update()
        {
            resourceText.text = GameManager.Instance.currentResources.ToString();
            healthText.text = Mathf.FloorToInt(GameManager.Instance.currentHp).ToString();
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void ResetProgress()
        {
            JsonSave.DeleteSave();
            ReturnToMenu();
        }

        public void ChangeSpeed()
        {
            GameManager.Instance.ChangeSpeed();
            speedText.text = $"{Mathf.FloorToInt(GameManager.Instance.gameSpeed)}x";
        }
    }
}
