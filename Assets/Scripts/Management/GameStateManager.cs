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

	public static event Action<JumpEventArgs> PlayerJumped;
	public static event Action PlayerLanded;
	public static event Action<DiedEventArgs> PlayerDied;
	public static event Action RestartGame;
	public static event Action StartGame;
	public static event Action ContinueGame;
	public static event Action CoinPickup;
	public static event Action<RewardedEventArgs> PlayerRewarded;

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
		InputController.JumpPerformed += Input_OnJump;
		InputController.FlipPerformed += Input_OnFlip;
		InputController.RestartPerformed += Input_OnRestart;
	}

	private void OnDestroy()
	{
		CoinPickup -= M_OnCoinPickup;
		InputController.JumpPerformed -= Input_OnJump;
		InputController.FlipPerformed -= Input_OnFlip;
		InputController.RestartPerformed -= Input_OnRestart;
	}

	private void Input_OnFlip() { if (!Instance.gameRunning) HandleStartGame(); }
	private void Input_OnJump() { if (!Instance.gameRunning) HandleStartGame(); }
	private void Input_OnRestart() => HandleRestart();

	public void RaisePlayerJumped(int direction)
	{
		Debug.Log("EVENT: Jumped");
		PlayerJumped?.Invoke(new JumpEventArgs(direction));
	}

	public void RaisePlayerLanded()
	{
		Debug.Log("EVENT: Landed");
		UpdateScore();
		Instance.HandleLand();
		PlayerLanded?.Invoke();
	}

	public void RaisePlayerDied()
	{
		Debug.Log("EVENT: Died");
		PlayerDied?.Invoke(new DiedEventArgs(m_currentScore, m_currentCoin));
	}

	public void RaiseRestartGame()
	{
		Debug.Log("EVENT: Restarted");
		Instance.gameContinuedAlready = false;
		Instance.gameRunning = false;
		RestartGame?.Invoke();
	}

	public void RaiseStartGame()
	{
		Debug.Log("EVENT: Started");
		StartGame?.Invoke();
	}

	public void RaiseContinueGame()
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

	public void RaiseCoinPickup()
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

	public void HandleStartGame()
	{
		if (Instance.gameRunning)
			return;
		Instance.gameRunning = true;
		RaiseStartGame();
	}

	public void HandleRestart()
	{
		m_currentScore = 0;
		m_currentCoin = SaveManager.Instance.GetCoin();
		Instance.gameRunning = false;
		RaiseRestartGame();
	}

	public void HandleQuit()
	{
		Instance.gameRunning = false;
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
