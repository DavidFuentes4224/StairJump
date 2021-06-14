using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.RestartGame += OnRestartGame; ;
        GameStateManager.StartGame += OnStartGame; ;
#if UNITY_IOS
    enabled = false;
#endif
    }
    private void OnDestroy()
    {
        GameStateManager.RestartGame -= OnRestartGame;
        GameStateManager.StartGame -= OnStartGame;
    }

    private void OnStartGame()
    {
        enabled = false;
    }

    private void OnRestartGame()
    {
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameStateManager.Instance.HandleRestart();
        }
        else if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameStateManager.Instance.HandleStartGame();
        }
    }
}
