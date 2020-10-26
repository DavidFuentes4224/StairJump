using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public Text Score;
    public static Player PlayerRef;
    public Transform Fire;

    private int m_currentScore;
    private float m_timer;
    private Vector3 m_originalFirePosition;
    [SerializeField]
    private float TIMETOREACT;
    private float m_fireSpeed;

    private void Awake()
    {
        PlayerRef = FindObjectOfType<Player>();
    }

    private void Start()
    {
        //TIMETOREACT = 3f;
        m_currentScore = 0;
        m_originalFirePosition = Fire.position;
        m_fireSpeed = Vector2.Distance(m_originalFirePosition, PlayerRef.transform.position) / TIMETOREACT;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerRef.GetIsAlive())
        {
            UpdateFirePosition();
            if(!PlayerRef.GetIsGrounded())
            {
                m_timer -= Time.deltaTime;

            }
        }
        if (m_timer < 0)
        {
            PlayerRef.KillPlayer();
        }
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void UpdateFirePosition()
    {
        //Fire.position += Vector3.up * Time.deltaTime * 2f;
        Fire.position += Vector3.up * Time.deltaTime * m_fireSpeed;
    }

    public void UpdateScore()
    {
        m_currentScore += 1;
        Score.text = $"High Score: {m_currentScore}";
        DecreaseFirePosition();
    }

    private void DecreaseFirePosition()
    {
        var decreasedPos = Fire.position - Vector3.up;
        var newPos = (decreasedPos.y > m_originalFirePosition.y) ? decreasedPos : m_originalFirePosition;
        Fire.position = newPos;
    }
}
