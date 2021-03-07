using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public float TimeToReact = 3f;
    public float LavaDecreaseAmount = 1f;

    [SerializeField]
    private Transform playerStart = null;
    private Vector3 m_originalFirePosition;
    private float m_fireSpeed;
    private bool playerAlive = true;

    private void Awake()
    {
        GameStateManager.PlayerDied += OnPlayerDied;
        GameStateManager.PlayerJumped += OnPlayerJumped;
        GameStateManager.RestartGame += OnRestartGame;
        GameStateManager.StartGame += OnStartGame;

    }

    private void Start()
    {
        m_originalFirePosition = transform.position;
        m_fireSpeed = Vector2.Distance(m_originalFirePosition, playerStart.position) / TimeToReact;
        enabled = false;
    }

    void Update()
    {
        if (playerAlive)
        {
            UpdateFirePosition();
        }
    }

    private void OnStartGame()
    {
        enabled = true;
    }

    private void OnRestartGame()
    {
        transform.position = m_originalFirePosition;
        playerAlive = true;
        enabled = false;
    }

    private void OnPlayerDied(GameStateManager.DiedEventArgs e)
    {
        playerAlive = false;
    }

    private void OnPlayerJumped(GameStateManager.JumpEventArgs e)
    {
        DecreaseFirePosition();
    }

    private void UpdateFirePosition()
    {
        transform.position += Vector3.up * Time.deltaTime * m_fireSpeed;
    }

    public void DecreaseFirePosition()
    {
        var decreasedPos = transform.position - (Vector3.up * LavaDecreaseAmount);
        var newPos = (decreasedPos.y > m_originalFirePosition.y) ? decreasedPos : m_originalFirePosition;
        transform.position = newPos;
    }
}
