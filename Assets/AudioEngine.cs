using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEngine : MonoBehaviour
{
    [SerializeField]
    private AudioSource jumpsource = null;
    [SerializeField]
    private AudioSource coinsource = null;
    [SerializeField]
    private AudioSource failsource = null;


    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.PlayerJumped += OnPlayerJumped;
        GameStateManager.PlayerDied += OnPlayerDied;
        GameStateManager.CoinPickup += OnCoinPickup;
    }

    

    private void OnDestroy()
    {
        GameStateManager.PlayerJumped -= OnPlayerJumped;
        GameStateManager.PlayerDied -= OnPlayerDied;
        GameStateManager.CoinPickup -= OnCoinPickup;

    }

    private void OnPlayerDied(GameStateManager.DiedEventArgs e)
    {
        failsource.Play();
    }

    private void OnPlayerJumped(GameStateManager.JumpEventArgs obj)
    {
        jumpsource.Play();
    }

    private void OnCoinPickup()
    {
        coinsource.Play();
    }
}
