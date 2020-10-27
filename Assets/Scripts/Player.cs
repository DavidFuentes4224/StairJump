using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private GameStateManager m_manager;
    private Animator m_animator;

    public Transform RayOrigin;
    public TileGenerator TileGenerator;
    public LayerMask CollisionMask;


    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_manager = FindObjectOfType<GameStateManager>();
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_direction = Directions.RIGHT;
        m_isGrounded = true;
        m_isJumping = false;
        m_isAlive = true;
        m_rayDistance = 0.55f;
        m_animator.enabled = true;
    }

    void Update()
    {
        m_isGrounded = CheckIfGrounded();
        m_animator.SetBool("Jump", false);

        if (!m_isGrounded && !m_isJumping)
        {
            KillPlayer();
        }
        else if (m_isGrounded &&!m_isJumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleJump();
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                transform.localScale = new Vector3(m_direction * 0.1f, 0.1f);
                m_direction *= -1;
                HandleJump();
            }
        }
        
    }

    private void HandleJump()
    {
        m_manager.DecreaseFirePosition();
        m_animator.SetBool("Jump", true);
        TileGenerator.UpdateTiles(m_direction);
    }

    private bool CheckIfGrounded()
    {
        var hit = Physics2D.Raycast(transform.position, -Vector2.up, m_rayDistance,CollisionMask);
        Debug.DrawRay(transform.position, -Vector2.up * m_rayDistance,Color.red);

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

    public void KillPlayer()
    {
        m_isAlive = false;
        m_rigidbody.simulated = true;
        m_animator.enabled = false;
    }

    public void OnPlayerJumped()
    {
        m_isJumping = true;
    }

    public void OnPlayerLanded()
    {
        m_isJumping = false;

        if (m_isGrounded) m_manager.UpdateScore();
        
    }
}
