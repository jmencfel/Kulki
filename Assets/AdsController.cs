using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;


public class AdsController : MonoBehaviour
{
    private InterstitialAd interstitial;
    public static AdsController instance;
    private bool shouldRestart = false;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
            Destroy(this);

        instance = this;
        DontDestroyOnLoad(this);
       
        MobileAds.Initialize(initStatus => { });
    }
    void Start()
    {
        RequestInterstitial();
    }

    // Update is called once per frame
    void Update()
    {
       if(shouldRestart)
        {
            shouldRestart = false;
            PlayerPrefs.SetInt("RollingScore", 0); 
        }
    }
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-9583790882886535/3807431922";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-9583790882886535/3807431922";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }
    public void ShowAd()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);

        RequestInterstitial();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
        shouldRestart = true;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        shouldRestart = true;
        RequestInterstitial();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
}
