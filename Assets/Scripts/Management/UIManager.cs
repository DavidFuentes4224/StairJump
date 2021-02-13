using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject TapToStart;
    [SerializeField] private GameObject ScorePanel;
    [SerializeField] private Text HighScore;
    [SerializeField] private GameStateManager m_manager;

    void Start()
    {
        //Seperate UI behaviors into this file (GameStateManager)
        //Subscribe to events
        GameStateManager.StartGame += OnStartGame;
        GameStateManager.PlayerDied += OnPlayerDied;
        GameStateManager.PlayerLanded += OnPlayerLanded;
        GameStateManager.RestartGame += OnRestartGame;
        ScorePanel.SetActive(false);
        TapToStart.SetActive(true);
    }

    

    private void OnStartGame()
    {
        TapToStart.SetActive(false);
        SetText();
    }

    private void OnRestartGame()
    {
        ScorePanel.SetActive(false);
        TapToStart.SetActive(true);
    }

    private void OnPlayerLanded()
    {
        SetText();
    }

    private void SetText()
    {
        HighScore.text = $"High Score: {m_manager.GetHeight()}";
    }

    private void OnPlayerDied()
    {
        TapToStart.SetActive(false);
        ScorePanel.SetActive(true);
    }
}
