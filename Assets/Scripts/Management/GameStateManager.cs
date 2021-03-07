using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    private int m_currentScore;
    [SerializeField]
    private int m_currentCoin;
    [SerializeField]
    private bool gameRunning = false;

    public static event Action<JumpEventArgs> PlayerJumped;
    public static event Action PlayerLanded;
    public static event Action<DiedEventArgs> PlayerDied;
    public static event Action RestartGame;
    public static event Action StartGame;
    public static event Action CoinPickup;

    public static Player PlayerRef;


    public class JumpEventArgs : EventArgs
    {
        public int Direction { get; set; }

        public JumpEventArgs(int direction)
        {
            Direction = direction;
        }
    }

    public class DiedEventArgs : EventArgs
    {
        public int Score { get; set; }
        public DiedEventArgs(int score)
        {
            Score = score;
        }
    }

    private void Awake()
    {
        PlayerRef = FindObjectOfType<Player>();
    }

    private void Start()
    {
        m_currentScore = 0;
        m_currentCoin = 0;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        PlayerLanded += M_OnPlayerLanded;
        CoinPickup += M_OnCoinPickup;
    }

    void Update()
    {
        HandleInput();
    }

    public static void OnPlayerJumped(int direction)
    {
        Debug.Log("EVENT: Jumped");
        PlayerJumped?.Invoke(new JumpEventArgs(direction));
    }

    public static void OnPlayerLanded()
    {
        Debug.Log("EVENT: Landed");
        PlayerLanded?.Invoke();
    }

    public static void OnPlayerDied()
    {
        Debug.Log("EVENT: Died");
        var manager = GameObject.FindObjectOfType<GameStateManager>();
        PlayerDied?.Invoke(new DiedEventArgs(manager.m_currentScore));
    }

    public static void OnRestartGame()
    {
        Debug.Log("EVENT: Restarted");
        RestartGame?.Invoke();
    }

    public static void OnStartGame()
    {
        Debug.Log("EVENT: Started");
        StartGame?.Invoke();
    }

    public static void OnCoinPickup()
    {
        Debug.Log("EVENT: Coin picked up");
        CoinPickup?.Invoke();
    }
    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            HandleRestart();
        }
    }

    public void HandleStartGame()
    {
        if (gameRunning)
            return;
        OnStartGame();
    }

    public void HandleRestart()
    {
        OnRestartGame();
        m_currentScore = -1;
        m_currentCoin = 0;
        UpdateScore();
        gameRunning = false;
    }

    private void UpdateScore()
    {
        m_currentScore += 1;
    }  

    public int GetHeight()
    {
        return m_currentScore;
    }

    private void M_OnPlayerLanded()
    {
        UpdateScore();
    }

    private void M_OnCoinPickup()
    {
        m_currentCoin++;
    }
}
