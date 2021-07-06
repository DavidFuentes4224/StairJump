using UnityEngine;

public class AudioEngine : MonoBehaviour
{
    [SerializeField]
    private AudioSource jumpsource = null;
    [SerializeField]
    private AudioSource coinsource = null;
    [SerializeField]
    private AudioSource failsource = null;
    private bool isMuted = false;


    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.PlayerJumped += OnPlayerJumped;
        GameStateManager.PlayerDied += OnPlayerDied;
        GameStateManager.CoinPickup += OnCoinPickup;
        isMuted = SaveManager.Instance.IsMuted();
    }

    

    private void OnDestroy()
    {
        GameStateManager.PlayerJumped -= OnPlayerJumped;
        GameStateManager.PlayerDied -= OnPlayerDied;
        GameStateManager.CoinPickup -= OnCoinPickup;

    }

    private void OnPlayerDied(GameStateManager.DiedEventArgs e)
    {
        if(!isMuted) failsource.Play();
    }

    private void OnPlayerJumped(GameStateManager.JumpEventArgs obj)
    {
        if (!isMuted) jumpsource.Play();
    }

    private void OnCoinPickup()
    {
        if (!isMuted) coinsource.Play();
    }
}
