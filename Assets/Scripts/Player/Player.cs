using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private bool m_isGrounded;
    [SerializeField]
    private float m_rayDistance;
    [SerializeField]
    private int m_direction;
    [SerializeField]
    private bool m_isAlive;
    private Rigidbody2D m_rigidbody;
    [SerializeField]
    private bool m_isJumping;
    [SerializeField]
    private GameObject m_avatar;

    private Animator m_animator;
    private bool m_should_jump;
    private bool m_should_turn;
    private Vector3 originalStart;

    public Transform RayOrigin;
    public LayerMask CollisionMask;


    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ConfigureSettings();
        originalStart = transform.position;

        GameStateManager.PlayerDied += OnPlayerDied;
        GameStateManager.RestartGame += OnRestartGame;
        GameStateManager.StartGame += OnStartGame;
        GameStateManager.ContinueGame += OnContinueGame;

        enabled = false;
    }


    private void OnDestroy()
    {
        GameStateManager.PlayerDied -= OnPlayerDied;
        GameStateManager.RestartGame -= OnRestartGame;
        GameStateManager.StartGame -= OnStartGame;
        GameStateManager.ContinueGame -= OnContinueGame;

    }

    private void OnStartGame()
    {
        enabled = true;
    }

    private void OnRestartGame()
    {
        ConfigureSettings();
        transform.position = originalStart;
        transform.localScale = Vector3.one;
        enabled = false;
    }

    private void OnContinueGame()
    {
        OnRestartGame();
        //enabled = true;
    }


    private void ConfigureSettings()
    {
        m_direction = Directions.RIGHT;
        m_isGrounded = true;
        m_isJumping = false;
        m_isAlive = true;
        m_rayDistance = 0.55f;
        m_animator.enabled = true;
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        m_isGrounded = CheckIfGrounded();
        m_animator.SetBool("Jump", false);

        if (!m_isAlive)
            return;
        if (!m_isGrounded && !m_isJumping)
        {
            KillPlayer();
        }
        else if (m_isGrounded &&!m_isJumping)
        {
            if (Input.GetKeyDown(KeyCode.Space) || m_should_jump)
            {
                HandleJump();
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl) || m_should_turn)
            {
                HandleTurn();
            }
        }
        
    }

    public void HandleJump()
    {
        GameStateManager.OnPlayerJumped(m_direction);
        m_animator.SetBool("Jump", true);
        m_should_jump = false;
        m_should_turn = false;
    }

    public void HandleTurn()
    {
        //transform.localScale = new Vector3(m_direction, 1);
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        m_direction *= -1;
        HandleJump();
    }

    public void TryJump()
    {
        m_should_jump = true;
    }

    public void TryTurn()
    {
        m_should_turn = true;
    }

    private bool CheckIfGrounded()
    {
        var hit = Physics2D.Raycast(RayOrigin.position, -Vector2.up, m_rayDistance,CollisionMask);
        Debug.DrawRay(RayOrigin.position, -Vector2.up * m_rayDistance,Color.red);

        if (hit.collider != null && hit.collider.gameObject.layer != 9)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetIsGrounded()
    {
        return m_isGrounded;
    }

    public bool GetIsAlive()
    {
        return m_isAlive;
    }

    private void KillPlayer()
    {
        GameStateManager.OnPlayerDied();
    }

    public void EnableJumping()
    {
        m_isJumping = true;
    }

    public void DisableJumping()
    {
        m_isJumping = false;
        if (m_isGrounded) GameStateManager.OnPlayerLanded();
    }

    private void OnPlayerDied(GameStateManager.DiedEventArgs e)
    {
        m_isAlive = false;
        m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
        m_animator.enabled = false;
    }
}
