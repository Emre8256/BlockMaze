using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class GecisReklam : MonoBehaviour
{
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            LoadInterstitialAd();
        });
        
    }

    private enum AdResource
    {
        nothing,
        nextlevel,
        playagain,
        backtomenu
    }

    private AdResource mevcutreklam = AdResource.nothing;


    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-2674547753698287/4379459321";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId = "unused";
#endif

    private InterstitialAd _interstitialAd;

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
                RegisterEventHandlers(ad); // Event handler'larý kaydedin
                RegisterReloadHandler(ad); // Reload handler'ý kaydedin
            });
    }

    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            _interstitialAd.Destroy();

            if (mevcutreklam == AdResource.nextlevel)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (mevcutreklam == AdResource.playagain)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (mevcutreklam == AdResource.backtomenu)
            {
                SceneManager.LoadScene("MainMenu");
            }

            
            LoadInterstitialAd();
            
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // Destroy and load a new ad after the current one is closed.
            _interstitialAd.Destroy();
            LoadInterstitialAd();
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
    {
            Debug.Log("Interstitial Ad full screen content closed.");

        // Destroy the current ad to free resources.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        // Reload the ad so that we can show another as soon as possible.
        LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }
            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
    }

    public void ShowInterstitialAdForNextLevel()
    {
        mevcutreklam = AdResource.nextlevel;

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // Reklam yüklü mü kontrol et.
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                ShowInterstitialAd();
            }
            else
            {
                // Eðer reklam yüklü deðilse direkt olarak next level iþlemini yap.
                Debug.Log("Reklam yüklü deðil, doðrudan sonraki seviyeye geçiliyor.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        else
        {
            // Ýnternet baðlantýsý yoksa direkt olarak next level iþlemini yap
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ShowInterstitialAdForReplay()
    {
        mevcutreklam = AdResource.playagain;

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // Reklam yüklü mü kontrol et.
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                ShowInterstitialAd();
            }
            else
            {
                // Eðer reklam yüklü deðilse direkt olarak replay iþlemini yap.
                Debug.Log("Reklam yüklü deðil, doðrudan yeniden baþlatýlýyor.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            // Ýnternet baðlantýsý yoksa direkt olarak replay iþlemini yap
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ShowInterstitialAdForMainMenu()
    {
        mevcutreklam = AdResource.backtomenu;

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // Reklam yüklü mü kontrol et.
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                ShowInterstitialAd();
            }
            else
            {
                // Eðer reklam yüklü deðilse direkt olarak ana menüye dön.
                Debug.Log("Reklam yüklü deðil, doðrudan ana menüye dönülüyor.");
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
        {
            // Ýnternet baðlantýsý yoksa direkt olarak ana menüye dön
            SceneManager.LoadScene("MainMenu");
        }
    }

}
