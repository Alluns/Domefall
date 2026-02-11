using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action<GameState> onGameStateChanged;
    public SaveData saveData;
    public enum GameState
    {
        Playing,
        Paused,
        Upgrade,
        Lose,
        Win,
        Menu,
        Evolution
    }
    public GameState currentState;
    public float maxHp;
    [HideInInspector] public float currentHp;
    public int currentResources;
    [HideInInspector] public Bunker bunker;
    private float gameSpeed = 2.0f;
    public float GameSpeed => gameSpeed;

    private readonly List<float> gameSpeeds = new (){ 1.0f, 2.0f, 3.0f };
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            throw new Exception("2 singleton of the same [GamesManager exist]");
        }
        Instance = this;
        saveData = JsonSave.LoadData();
    }
    public void Start()
    {
        SwitchState(GameState.Playing);
        bunker = GameObject.FindGameObjectWithTag("Shelter").GetComponent<Bunker>();
        currentHp = maxHp;
    }
    
    private void Update()
    {
        switch (currentState)
        {
            case GameState.Playing:
                Time.timeScale = gameSpeed;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.Upgrade:
                Time.timeScale = 0f;
                break;
            case GameState.Lose:
                Time.timeScale = 0f;
                break;
            case GameState.Win:
                Time.timeScale = 0f;
                break;
            case GameState.Menu:
                Time.timeScale = 0f;
                break;
            case GameState.Evolution:
                Time.timeScale = 0f;
                break;
        }
    }
    public void SwitchState(int iState)
    {
        SwitchState((GameState)iState);
    }
    
    public void SwitchState(GameState aState)
    {
        if (aState == GameState.Playing)
        {
            Time.timeScale = gameSpeed;
        }
        
        currentState = aState;
        onGameStateChanged?.Invoke(currentState);
        
        if (aState == GameState.Win)
        {
            saveData.evolutionPoints++;
            JsonSave.Save(saveData);
        }
        
    }
    
    public void GetResource(int resource)
    {
        currentResources += resource;
    }
    
    public void TogglePause()
    {
        switch (currentState)
        {
            case GameState.Playing:
                SwitchState(GameState.Paused);
                break;
            case GameState.Paused:
                SwitchState(GameState.Playing);
                break;
        }
    }

    public void ChangeSpeed()
    {
        gameSpeed = gameSpeeds[(gameSpeeds.IndexOf(gameSpeed) + 1) % gameSpeeds.Count];
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene((sceneIndex + 1) % SceneManager.sceneCount);
    }
}

