using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text resourceText, versionText, healthText;
        private GameObject loseImage, winImage;
        private Image heatBar;
        private void Start()
        {
            loseImage = transform.Find("Lose Image").gameObject;
            loseImage.gameObject.SetActive(false);
            winImage = transform.Find("Victory Image").gameObject;
            winImage.gameObject.SetActive(false);

            heatBar = transform.Find("HeatBar").GetComponent<Image>();

            versionText.text = $"Project {Application.productName} version: {Application.version}";
        
            GameManager.Instance.onGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameManager.GameState gameState)
        {
            switch (gameState)
            {
                case GameManager.GameState.Lose:
                    loseImage.SetActive(true);
                    break;
                case GameManager.GameState.Win:
                    winImage.SetActive(true);
                    break;
            }
        }

        private void Update()
        {
            resourceText.text = GameManager.Instance.currentResources.ToString();
            healthText.text = Mathf.FloorToInt(GameManager.Instance.currentHp).ToString();
            heatBar.fillAmount = GameManager.Instance.bunker.currentHeat / Mathf.Max(GameManager.Instance.bunker.maxHeat, 1);
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
    }
}
