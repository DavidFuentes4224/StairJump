using UnityEngine;

public class Lava : MonoBehaviour
{
	[SerializeField]
	private Transform m_playerStart = null;
	[SerializeField]
	private AnimationCurve m_speedCurve;
	[SerializeField]
	private float m_lavaDecreaseAmount = 2.0f;


	public void DecreaseFirePosition()
	{
		var decreasedPos = transform.position - (Vector3.up * m_lavaDecreaseAmount);
		var newPos = (decreasedPos.y > m_originalFirePosition.y) ? decreasedPos : m_originalFirePosition;
		transform.position = newPos;
	}

	private void Awake()
	{
		GameStateManager.PlayerDied += OnPlayerDied;
		GameStateManager.PlayerJumped += OnPlayerJumped;
		GameStateManager.RestartGame += OnRestartGame;
		GameStateManager.StartGame += OnStartGame;
		GameStateManager.ContinueGame += OnContinueGame;
	}

	private void Start()
	{
		m_originalFirePosition = transform.position;
		m_fireSpeed = Vector2.Distance(m_originalFirePosition, m_playerStart.position) / c_timeToReact;
		enabled = false;
	}

	void Update()
	{
		if (!m_playerDead)
			UpdateFirePosition();
	}

	private void OnDestroy()
	{
		GameStateManager.PlayerDied -= OnPlayerDied;
		GameStateManager.PlayerJumped -= OnPlayerJumped;
		GameStateManager.RestartGame -= OnRestartGame;
		GameStateManager.StartGame -= OnStartGame;
		GameStateManager.ContinueGame -= OnContinueGame;
	}

	private void OnStartGame()
	{
		this.enabled= true;
	}

	private void OnRestartGame()
	{
		transform.position = m_originalFirePosition;
		m_playerDead = false;
		enabled = false;
		m_speedCurveValue = 0;
	}

	private void OnContinueGame()
	{
		OnRestartGame();
	}

	private void OnPlayerDied(DiedEventArgs e)
	{
		m_playerDead = true;
	}

	private void OnPlayerJumped(JumpEventArgs e)
	{
		DecreaseFirePosition();
	}

	private void UpdateFirePosition()
	{
		m_speedCurveValue += (Time.deltaTime * c_timeToPeakCurveValue);
		m_speedCurveValue = Mathf.Min(m_speedCurveValue, 1.0f);
		var speedModifier = m_speedCurve.Evaluate(m_speedCurveValue);
		transform.position += Vector3.up * Time.deltaTime * (m_fireSpeed * speedModifier);
	}

	const float c_timeToReact = 3.0f;
	const float c_timeToPeakCurveValue = 1 / 15f; // 15 Seconds
	
	bool m_playerDead;
	float m_speedCurveValue;
	float m_fireSpeed;
	Vector3 m_originalFirePosition;
}
