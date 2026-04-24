using UnityEngine;

public class Lava : MonoBehaviour
{
	[SerializeField]
	private Transform m_playerStart = null;
	private Vector3 m_originalFirePosition;
	private float m_fireSpeed;

	public void DecreaseFirePosition()
	{
		var decreasedPos = transform.position - (Vector3.up * c_lavaDecreaseAmount);
		var newPos = (decreasedPos.y > m_originalFirePosition.y) ? decreasedPos : m_originalFirePosition;
		transform.position = newPos;
	}

	private void Awake()
	{
		GameStateManager.Instance.PlayerDied += OnPlayerDied;
		GameStateManager.Instance.PlayerJumped += OnPlayerJumped;
		GameStateManager.Instance.RestartGame += OnRestartGame;
		GameStateManager.Instance.StartGame += OnStartGame;
		GameStateManager.Instance.ContinueGame += OnContinueGame;

	}

	private void Start()
	{
		m_originalFirePosition = transform.position;
		m_fireSpeed = Vector2.Distance(m_originalFirePosition, m_playerStart.position) / c_timeToReact;
		enabled = false;
	}

	void Update()
	{
		if (m_playerDead)
		{
			UpdateFirePosition();
		}
	}

	private void OnDestroy()
	{
		GameStateManager.Instance.PlayerDied -= OnPlayerDied;
		GameStateManager.Instance.PlayerJumped -= OnPlayerJumped;
		GameStateManager.Instance.RestartGame -= OnRestartGame;
		GameStateManager.Instance.StartGame -= OnStartGame;
		GameStateManager.Instance.ContinueGame -= OnContinueGame;

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
		transform.position += Vector3.up * Time.deltaTime * m_fireSpeed;
	}

	const float c_timeToReact = 3f;
	const float c_lavaDecreaseAmount = 1f;
	bool m_playerDead;
}
