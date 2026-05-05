using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private float m_rayDistance;
	[SerializeField]
	private int m_direction;
	[SerializeField]
	private bool m_isAlive;
	private Rigidbody2D m_rigidbody;
	[SerializeField]
	private GameObject m_avatar;

	private Animator m_animator;
	private Vector3 m_originalStart;

	public Transform RayOrigin;
	public LayerMask TileCollisionMask;
	public LayerMask LavaCollisionMask;


	private void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		m_originalStart = transform.position;
		Init();

		GameStateManager.PlayerDied += OnPlayerDied;
		GameStateManager.RestartGame += OnRestartGame;
		GameStateManager.StartGame += OnStartGame;
		GameStateManager.ContinueGame += OnContinueGame;

		InputController.JumpPerformed += TryJump;
		InputController.FlipPerformed += TryTurn;
	}

	private void OnDestroy()
	{
		GameStateManager.PlayerDied -= OnPlayerDied;
		GameStateManager.RestartGame -= OnRestartGame;
		GameStateManager.StartGame -= OnStartGame;
		GameStateManager.ContinueGame -= OnContinueGame;

		InputController.JumpPerformed -= TryJump;
		InputController.FlipPerformed -= TryTurn;
	}

	private void OnStartGame()
	{
		enabled = true;
	}

	private void OnRestartGame()
	{
		Init();
	}

	private void OnContinueGame()
	{
		OnRestartGame();
		//enabled = true;
	}


	private void Init()
	{
		transform.position = m_originalStart;
		transform.localScale = Vector3.one;
		m_direction = Directions.RIGHT;
		m_isJumping = false;
		m_isAlive = true;
		m_rayDistance = 0.55f;
		m_animator.enabled = true;
		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
		m_animator.ResetTrigger("Jump");

		enabled = false;
	}

	void Update()
	{
		if (!m_isAlive)
			return;
		if ((!m_isJumping && !CheckIfGrounded()) || IsTouchingLava())
			GameStateManager.Instance.RaisePlayerDied();
	}

	public void TryJump()
	{
		if (!m_isAlive || m_isJumping)
			return;
		DoJump();
	}

	public void TryTurn()
	{
		if (!m_isAlive || m_isJumping)
			return;
		DoTurn();
	}

	// Animation event.
	public void OnLand()
	{
		m_isJumping = false;
		GameStateManager.Instance.RaisePlayerLanded();
	}

	private bool CheckIfGrounded()
	{
		var hit = Physics2D.Raycast(RayOrigin.position, -Vector2.up, m_rayDistance, TileCollisionMask);
		return hit.collider != null && hit.collider.gameObject.layer != 9;
	}

	private bool IsTouchingLava()
	{
		var hit = Physics2D.Raycast(RayOrigin.position, -Vector2.up, m_rayDistance, LavaCollisionMask);
		return hit.collider != null;
	}

	private void OnPlayerDied(DiedEventArgs e)
	{
		m_isAlive = false;
		m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
		m_animator.enabled = false;
	}

	private void DoTurn()
	{
		m_direction *= -1;
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		DoJump();
	}

	private void DoJump()
	{
		m_isJumping = true;
		GameStateManager.Instance.RaisePlayerJumped(m_direction);
		m_animator.SetTrigger("Jump");
	}

	bool m_isJumping;
}
