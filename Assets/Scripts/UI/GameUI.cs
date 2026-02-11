using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text resourceText, healthText, speedText;
        private GameObject loseScreen, winScreen;
        [HideInInspector] public GameObject joystick;

        private void Awake()
        {
            joystick = transform.Find("JoyStick").gameObject;
            joystick.SetActive(false);
        }
        private void Start()
        {
            winScreen = transform.Find("Victory Screen").gameObject;
            winScreen.gameObject.SetActive(false);
            loseScreen = transform.Find("Game Over Screen").gameObject;
            loseScreen.gameObject.SetActive(false);
        
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
            SaveData oldData = JsonSave.LoadData();
            JsonSave.DeleteSave();
            SaveData newData = JsonSave.LoadData();
            
            newData.evolutionPoints = oldData.evolutionPoints;
            newData.evolutions = oldData.evolutions;
            
            JsonSave.Save(newData);
            ReturnToMenu();
        }

        public void ChangeSpeed()
        {
            GameManager.Instance.ChangeSpeed();
            speedText.text = $"{Mathf.FloorToInt(GameManager.Instance.gameSpeed)}x";
        }

        public void PauseGame()
        {
            GameManager.Instance.TogglePause();
        }
    }
}
