using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;
using GoogleMobileAds;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;



public class AdsInitializer : MonoBehaviour
{
    public void Start()
    {
        rewardbutton.SetActive(false);
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            LoadRewardedAd();
        });

        
    }

      // These ad units are configured to always serve test ads.
     #if UNITY_ANDROID
      private string _adUnitId = "ca-app-pub-2674547753698287/9658507057";
     #elif UNITY_IPHONE
     private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
     #else
     private string _adUnitId = "unused";
     #endif

    private RewardedAd rewardedAd;
    private int maxLevel = 100;
    [SerializeField] GameObject rewardbutton;

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterEventHandlers(ad);
                RegisterReloadHandler(ad); // Bu fonksiyon çaðrýlýr.

                // Reklam yüklendiðinde butonu etkinleþtir
                rewardbutton.SetActive(true);

                // Ýlk sarsýntý efekti
                ShakeButton();

                // Her 3 saniyede bir sarsýntý efekti ekle
                InvokeRepeating("ShakeButton", 3f, 3f);
            });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));               
                UnlockNextLevel();
                // Reklam gösterildikten sonra Destroy() çaðrýlýr.
                
                SceneManager.LoadScene("Levels");
            });
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");

        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void UnlockNextLevel()
    {
        int LastLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 1);
        int nextLevelIndex = LastLevel + 1;

        if (nextLevelIndex <= maxLevel)
        {
            PlayerPrefs.SetInt("LevelUnlocked_" + nextLevelIndex, 1);

            PlayerPrefs.SetInt("LastUnlockedLevel", nextLevelIndex);
            

            PlayerPrefs.Save(); // Deðiþiklikleri kaydet
        }

    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            rewardedAd.Destroy();
            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            rewardedAd.Destroy();
            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }

    private void ShakeButton()
    {
        if (rewardbutton.activeSelf)
        {
            rewardbutton.transform.DOShakePosition(1f, 10, 10, 90, false, true);
        }
    }
}