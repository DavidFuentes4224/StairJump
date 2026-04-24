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
		GameStateManager.Instance.PlayerJumped += OnPlayerJumped;
		GameStateManager.Instance.PlayerDied += OnPlayerDied;
		GameStateManager.Instance.CoinPickup += OnCoinPickup;
	}

	

	private void OnDestroy()
	{
		GameStateManager.Instance.PlayerJumped -= OnPlayerJumped;
		GameStateManager.Instance.PlayerDied -= OnPlayerDied;
		GameStateManager.Instance.CoinPickup -= OnCoinPickup;

	}

	private void OnPlayerDied(DiedEventArgs e)
	{
		failsource.Play();
	}

	private void OnPlayerJumped(JumpEventArgs obj)
	{
		jumpsource.Play();
	}

	private void OnCoinPickup()
	{
		coinsource.Play();
	}
}
