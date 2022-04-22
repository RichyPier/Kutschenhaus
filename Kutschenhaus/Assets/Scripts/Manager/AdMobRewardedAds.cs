using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Events;
using System;



[System.Serializable]
public class AdMobRewardedAd
{
    public string type;
    public string unitIdAndroid;
    public string unitIdIPhone;
    public GameEvent onSuccessEvent;
    public RewardedAd rewardedAd;
}

public class AdMobRewardedAds : MonoBehaviour
{
    [Header("Rewarded Ads")]
    [SerializeField] AdMobRewardedAd[] rewardedAds;
    string lastStartedType;

   //[SerializeField] string noConsentEvent;
   //[SerializeField] string onErrorEvent;
    [SerializeField] string privacyConsentWindow;
    [SerializeField] string adsErrorWindow;
    private void Start()
    {
        Init();
    }

    public void Init()
    {

        int adPrivacy = FindObjectOfType<SaveManager>().GetAdPrivacy();

        if (adPrivacy == -1)
        {
            Debug.Log("Consent or opt-out of personalized advertising is not specified.");
            ShowWindow(privacyConsentWindow);
            return;
        }

        if(adPrivacy == 1)
        {
            Debug.Log("non-personalized ads enabled");
            AdRequest request = new AdRequest.Builder()
            .AddExtra("npa", adPrivacy.ToString() )
            .Build();
        }
        else
        {
            Debug.Log("personalized ads enabled");
        }
 
        MobileAds.Initialize(initStatus => { });

        foreach (var ad in rewardedAds)
        {
            #if UNITY_ANDROID
                    ad.rewardedAd = CreateAndLoadRewardedAd(ad.unitIdAndroid);
            #elif UNITY_IPHONE
                    ad.rewardedAd = CreateAndLoadRewardedAd(ad.unitIdIPhone);    
            #endif
        }
    }

    public RewardedAd CreateAndLoadRewardedAd(string adUnitId)
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);

        rewardedAd.OnUserEarnedReward += OnUserEarnedReward;
        rewardedAd.OnAdClosed += OnAdClosed;
        rewardedAd.OnAdFailedToLoad += OnRewardedAdFailedToLoad;
        rewardedAd.OnAdFailedToShow += OnRewardedAdFailedToShow;

        AdRequest request = new AdRequest.Builder().Build();

        rewardedAd.LoadAd(request);
        return rewardedAd;
    }

    public void StartRewardedAd(string type)
    {
        foreach (var ad in rewardedAds)
        {
            if (ad.type == type)
            {
                if (ad != null && ad.rewardedAd.IsLoaded())
                {
                    lastStartedType = ad.type;
                    ad.rewardedAd.Show();                   
                }
                else
                {
                    ShowWindow(adsErrorWindow);
                }

                return;
            }
        }
        ShowWindow(adsErrorWindow);
    }

    public void OnUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        
        Debug.Log(
             "AdRewarded event received for "
                 + amount.ToString() + " " + type);

        foreach (var ad in rewardedAds)
        {
            if (ad.type == type)
            {
                 ad.onSuccessEvent.Raise();              
            }
        }       
    }

    public void OnRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log(
            "AdFailedToLoad event received with message: "
                             + args.LoadAdError);
        ShowWindow(adsErrorWindow);
    }

    public void OnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("AdClosed event received");
        foreach (var ad in rewardedAds)
        {
            if (ad.type == lastStartedType)
            {
                Debug.Log("Load new ad type: " + ad.type);
                #if UNITY_ANDROID
                ad.rewardedAd = CreateAndLoadRewardedAd(ad.unitIdAndroid);
                #elif UNITY_IPHONE
                ad.rewardedAd = CreateAndLoadRewardedAd(ad.unitIdIPhone);   
                #endif
            }
        }
       
    }

    public void OnRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log(
             "AdFailedToShow event received with message: "
                              + args.AdError.GetMessage());

        ShowWindow(adsErrorWindow);
    }

    void ShowWindow(string resourceName)
    {
        var resource = Resources.Load<GameObject>(resourceName);
        var parent = GameObject.Find("ManagementUI").transform;
        Instantiate(resource, parent);
    }



}

