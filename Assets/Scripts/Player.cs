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
    private bool m_jumped;
    private GameStateManager m_manager;

    public Transform RayOrigin;
    public TileGenerator TileGenerator;
    public LayerMask CollisionMask;


    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_manager = FindObjectOfType<GameStateManager>();
    }

    private void Start()
    {
        m_direction = Directions.RIGHT;
        m_isGrounded = true;
        m_jumped = false;
        m_isAlive = true;
        m_rayDistance = 0.55f;
    }

    void Update()
    {
        if(m_isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TileGenerator.UpdateTiles2(m_direction);
                m_jumped = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                transform.localScale = new Vector3(m_direction * 0.1f,0.1f);
                m_direction *= -1;
                TileGenerator.UpdateTiles2(m_direction);
                m_jumped = true;
            }
        }
        
    }

    private void FixedUpdate()
    {
        m_isGrounded = CheckIfGrounded();
    }

    private void LateUpdate()
    {
        if(m_jumped && CheckIfGrounded())
        {
            m_manager.UpdateScore();
            m_jumped = false;
        }
    }

    private bool CheckIfGrounded()
    {
        var hit = Physics2D.Raycast(transform.position, -Vector2.up, m_rayDistance,CollisionMask);
        Debug.DrawRay(transform.position, -Vector2.up * m_rayDistance,Color.red);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject.layer == 9) //Fire Layer
            {
                KillPlayer();
            }
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
    }
}
