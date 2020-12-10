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
    public Lava LavaRef;
    public Transform Bttn_Restart;

    private int m_currentScore;
    private float m_timer;

    private void Awake()
    {
        PlayerRef = FindObjectOfType<Player>();
    }

    private void Start()
    {
        m_currentScore = 0;
        Bttn_Restart.gameObject.SetActive(false);
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    void Update()
    {
        HandleInput();
        if (!PlayerRef.GetIsAlive())
        {
            Bttn_Restart.gameObject.SetActive(true);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateScore()
    {
        m_currentScore += 1;
        Score.text = $"High Score: {m_currentScore}";
    }  

    public void DecreaseFirePosition()
    {
        LavaRef.DecreaseFirePosition();
    }
}
