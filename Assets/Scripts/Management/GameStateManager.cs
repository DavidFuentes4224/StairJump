using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private int m_currentScore;
    [SerializeField] private int m_currentCoin;
    [SerializeField] private bool gameRunning = false;
    [SerializeField] private bool gameContinuedAlready = false;
    [SerializeField] private static bool created = false;
    [SerializeField] private const int REWARDBONUS = 10;
    [SerializeField] private const int CONTINUEAMOUNT = 10;
    [SerializeField] private const int MAXREWARDCOOLDOWN = 250;
    [SerializeField] private int RewardCooldown = 0;

    public static event Action<JumpEventArgs> PlayerJumped;
    public static event Action PlayerLanded;
    public static event Action<DiedEventArgs> PlayerDied;
    public static event Action RestartGame;
    public static event Action StartGame;
    public static event Action ContinueGame;
    public static event Action CoinPickup;
    public static event Action<RewardedEventArgs> PlayerRewarded;

    public static Player PlayerRef;


    public class JumpEventArgs : EventArgs
    {
        public int Direction { get; set; }

        public JumpEventArgs(int direction)
        {
            Direction = direction;
        }
    }

    public class RewardedEventArgs : EventArgs
    {
        public int Coins { get; }

        public RewardedEventArgs(int c)
        {
            Coins = c;
        }
    }

    public class DiedEventArgs : EventArgs
    {
        public int Score { get; set; }
        public int Coins { get; set; }
        public DiedEventArgs(int score,int coins)
        {
            Score = score;
            Coins = coins;
        }
    }

    private static GameStateManager instance;
    public static GameStateManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        PlayerRef = FindObjectOfType<Player>();
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        m_currentScore = 0;
        m_currentCoin = SaveManager.Instance.GetCoin();
        CoinPickup += M_OnCoinPickup;
        RewardCooldown = MAXREWARDCOOLDOWN;
    }

    void Update()
    {
        HandleInput();
    }

    private void OnDestroy()
    {
        CoinPickup -= M_OnCoinPickup;
    }

    public static void OnPlayerJumped(int direction)
    {
        Debug.Log("EVENT: Jumped");
        PlayerJumped?.Invoke(new JumpEventArgs(direction));
    }

    public static void OnPlayerLanded()
    {
        Debug.Log("EVENT: Landed");
        GameStateManager.instance.UpdateScore();
        Instance.HandleLand();
        PlayerLanded?.Invoke();
    }

    public static void OnPlayerDied()
    {
        Debug.Log("EVENT: Died");
        var manager = instance;
        PlayerDied?.Invoke(new DiedEventArgs(manager.m_currentScore,manager.m_currentCoin));
    }

    public static void OnRestartGame()
    {
        Debug.Log("EVENT: Restarted");
        Instance.gameContinuedAlready = false;
        RestartGame?.Invoke();
    }

    public static void OnStartGame()
    {
        Debug.Log("EVENT: Started");
        StartGame?.Invoke();
    }

    public static void OnContinueGame()
    {
        Debug.Log("EVENT: Continued");
        if (Instance.m_currentCoin > CONTINUEAMOUNT)
        {
            Instance.m_currentCoin -= CONTINUEAMOUNT;
            Instance.gameRunning = false;
            Instance.gameContinuedAlready = true;
            ContinueGame?.Invoke();
        }
    }

    public static void OnCoinPickup()
    {
        Debug.Log("EVENT: Coin picked up");
        CoinPickup?.Invoke();
    }

    public void RewardPlayer()
    {
        m_currentCoin += REWARDBONUS;
        if (RewardCooldown <= 0)
            RewardCooldown = MAXREWARDCOOLDOWN - 100;
        SaveManager.Instance.UpdateCoin(m_currentCoin);
        PlayerRewarded?.Invoke(new RewardedEventArgs(m_currentCoin));
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
        gameRunning = true;
        OnStartGame();
    }

    public void HandleRestart()
    {
        OnRestartGame();
        m_currentScore = -1;
        m_currentCoin = SaveManager.Instance.GetCoin();
        UpdateScore();
        gameRunning = false;
    }

    public void HandleQuit()
    {
        gameRunning = false;
        m_currentScore = 0;
        gameContinuedAlready = false;
    }

    private void HandleLand()
    {
        RewardCooldown -= 1;
    }

    private void UpdateScore()
    {
        m_currentScore += 1;
    }  

    public int GetHeight()
    {
        return m_currentScore;
    }

    public int GetCoins()
    {
        return m_currentCoin;
    }

    public bool GetGameCanContinue()
    {
        return !gameContinuedAlready;
    }

    public bool GetCanBeRewarded()
    {
        var canBeRewared = false;
        if (Instance.GetGameCanContinue() && (Instance.GetCoins() - CONTINUEAMOUNT < REWARDBONUS))
            canBeRewared = true;
        else if (RewardCooldown <= 0)
            canBeRewared = true;
        return canBeRewared;
    }

    private void M_OnCoinPickup()
    {
        m_currentCoin++;
    }
}
