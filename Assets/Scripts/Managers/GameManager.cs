using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [HideInInspector] public int evolutionPoints;
    [HideInInspector] public Bunker bunker;
    public float gameSpeed = 1.0f;
    private readonly List<float> gameSpeeds = new (){ 1.0f, 2f, 3.0f };
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            throw new System.Exception("2 singleton of the same [GamesManager exist]");
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
                evolutionPoints++;
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
        currentState = aState;
        onGameStateChanged?.Invoke(currentState);
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

