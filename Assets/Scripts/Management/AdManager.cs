using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour , IUnityAdsListener
{
    private string playStoreID = "4055516";
    private string appStoreID = "4055517";

    private string bannerAd = "Banner_iOS";
    private string rewardAd = "Rewarded_iOS";

    [SerializeField] private bool isTestAd = false;

    private void Awake()
    {
        GameStateManager.PlayerDied += OnPlayerDied;
        GameStateManager.StartGame += OnStartGame;
    }

    private void OnStartGame()
    {
        PlayBannerAd();
    }

    private void Start()
    {
        InitializeAd();
    }

    private void OnPlayerDied(GameStateManager.DiedEventArgs obj)
    {
        if (obj.Score > 25)
        {
            PlayRewardAd();
        }
    }

    private void InitializeAd()
    {
#if UNITY_IOS
        Debug.Log("Iphone");
        Advertisement.Initialize(appStoreID, isTestAd);
#endif
        Advertisement.Initialize(appStoreID, isTestAd);

    }

    public void PlayBannerAd()
    {
        if (!Advertisement.IsReady(bannerAd))
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
        //MUTE AUDIO HERE
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        //Switch on showresult
        //throw new System.NotImplementedException();
    }
}
