using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEngine : MonoBehaviour
{
    [SerializeField]
    private AudioSource jumpsource;
    [SerializeField]
    private AudioSource coinsource;
    [SerializeField]
    private AudioSource failsource;


    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.PlayerJumped += OnPlayerJumped;
        GameStateManager.PlayerDied += OnPlayerDied;
    }

    private void OnPlayerDied(GameStateManager.DiedEventArgs e)
    {
        failsource.Play();
    }

    private void OnPlayerJumped(GameStateManager.JumpEventArgs obj)
    {
        jumpsource.Play();
    }
}
