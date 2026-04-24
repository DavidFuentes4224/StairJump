using System;
using UnityEngine;

public sealed class GameStateManager : ManagerBase<GameStateManager>
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

	public event Action<JumpEventArgs> PlayerJumped;
	public event Action PlayerLanded;
	public event Action<DiedEventArgs> PlayerDied;
	public event Action RestartGame;
	public event Action StartGame;
	public event Action ContinueGame;
	public event Action CoinPickup;
	public event Action<RewardedEventArgs> PlayerRewarded;

	public Player PlayerRef;

	protected override void Awake()
	{
		base.Awake();
		PlayerRef = FindObjectOfType<Player>();
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

	public void OnPlayerJumped(int direction)
	{
		Debug.Log("EVENT: Jumped");
		PlayerJumped?.Invoke(new JumpEventArgs(direction));
	}

	public void OnPlayerLanded()
	{
		Debug.Log("EVENT: Landed");
		UpdateScore();
		Instance.HandleLand();
		PlayerLanded?.Invoke();
	}

	public void OnPlayerDied()
	{
		Debug.Log("EVENT: Died");
		PlayerDied?.Invoke(new DiedEventArgs(m_currentScore, m_currentCoin));
	}

	public void OnRestartGame()
	{
		Debug.Log("EVENT: Restarted");
		Instance.gameContinuedAlready = false;
		RestartGame?.Invoke();
	}

	public void OnStartGame()
	{
		Debug.Log("EVENT: Started");
		StartGame?.Invoke();
	}

	public void OnContinueGame()
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

	public void OnCoinPickup()
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

public sealed class JumpEventArgs : EventArgs
{
	public JumpEventArgs(int direction)
	{
		Direction = direction;
	}
	public int Direction { get; }
}

public sealed class RewardedEventArgs : EventArgs
{
	public RewardedEventArgs(int c)
	{
		Coins = c;
	}
	public int Coins { get; }
}

public sealed class DiedEventArgs : EventArgs
{
	public DiedEventArgs(int score, int coins)
	{
		Score = score;
		Coins = coins;
	}
	public int Score { get; }
	public int Coins { get; }
}
