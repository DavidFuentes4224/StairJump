using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour , IUnityAdsListener
{
    private string playStoreID = "4055516";
    private string appStoreID = "4055517";

    private string bannerAd = "Banner_iOS";
    private string rewardAd = "Rewarded_iOS";

    [SerializeField] private bool isTestAd = false;
    [SerializeField] private static bool created = false;

    private static AdManager instance;
    public static AdManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        GameStateManager.StartGame += OnStartGame;
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnStartGame()
    {
        PlayBannerAd();
    }

    private void Start()
    {
        Advertisement.AddListener(this);
        InitializeAd();
    }

    private void InitializeAd()
    {
#if UNITY_IOS
        Debug.Log("Iphone");
        Advertisement.Initialize(appStoreID, isTestAd);
#endif
#if UNITY_ANDROID
    Debug.Log("Iphone");
    Advertisement.Initialize(appStoreID, isTestAd);
#endif
    }

    public void PlayBannerAd()
    {
        if (!Advertisement.IsReady(bannerAd) || Advertisement.isShowing)
            return;
        
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);

        Advertisement.Banner.Show(bannerAd);
    }

    public void PlayRewardAd()
    {
        if (!Advertisement.IsReady(rewardAd))
            return;
        Advertisement.Show(rewardAd);
    }

    public void OnUnityAdsReady(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Ad finished" + showResult);
        if(placementId == rewardAd)
        {
            switch (showResult)
            {
                case ShowResult.Failed:
                    GameStateManager.Instance.RewardPlayer();
                    break;
                case ShowResult.Skipped:
                    break;
                case ShowResult.Finished:
                    GameStateManager.Instance.RewardPlayer();
                    break;
            }
        }  
    }
}
