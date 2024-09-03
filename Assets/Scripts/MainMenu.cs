using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject soundWaveImg;
    [SerializeField] private GameObject vibrateWaveImg;
    [SerializeField] private AudioSource NormalButton;
    [SerializeField] private GameObject Canclick;

    public static bool IsSoundOn = true;
    public static bool IsVibrateOn = true;


    void Start()
    {
        // Cihazýn ekran yenileme hýzýný al
        double refreshRate = Screen.currentResolution.refreshRateRatio.value;
        
        // FPS ayarýný ekran yenileme hýzýna göre ayarla
        Application.targetFrameRate = Mathf.RoundToInt((float)refreshRate);

        Time.timeScale = 1f;

        if (!IsSoundOn) soundWaveImg.SetActive(true);
        if (!IsVibrateOn) vibrateWaveImg.SetActive(true);

        // PlayerPrefs'te daha önce bir deðer olup olmadýðýný kontrol edin
        if (!PlayerPrefs.HasKey("LevelUnlocked_1"))
        {
            // Ýlk kez açýlýþta, seviyelerin varsayýlan durumunu ayarla
            InitializeLevelUnlocks();
        } 
    }


    void Update()
    {
        if(SplashScreen.isFinished) Destroy(Canclick);  
    }

    public void goLevelsMenu()
    {
        SceneManager.LoadScene("Levels");
        if (IsSoundOn)
            NormalButton.Play();
    }

    public void soundButton()
    {
        if (!soundWaveImg.activeSelf)
        {
            soundWaveImg.SetActive(true);
            IsSoundOn = false;
        }
        else
        {
            soundWaveImg.SetActive(false);
            IsSoundOn = true;
            NormalButton.Play();
        }
    }

    public void vibrateButton()
    {
        if (!vibrateWaveImg.activeSelf)
        {
            vibrateWaveImg.SetActive(true);
            IsVibrateOn = false;
            if (IsSoundOn)
                NormalButton.Play();
        }
        else
        {
            vibrateWaveImg.SetActive(false);
            IsVibrateOn = true;
            if (IsSoundOn)
                NormalButton.Play();
        }
    }

    public void OnPlayButtonClicked()
    {
        // En son aktif olan level'i alýyoruz, eðer kaydedilmediyse default olarak Level 1'i alýr
        int lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 1);

        // En son aktif olan level'i yüklüyoruz
        SceneManager.LoadScene("Level" + lastUnlockedLevel);
    }

    void InitializeLevelUnlocks()
    {
        // Level 1 her zaman açýlmýþ olmalý
        PlayerPrefs.SetInt("LevelUnlocked_1", 1);
        PlayerPrefs.SetInt("LastUnlockedLevel", 1);

        // Diðer seviyeleri kilitle
        for (int i = 2; i <= 100; i++)
        {
            PlayerPrefs.SetInt("LevelUnlocked_" + i, 0);
        }

        PlayerPrefs.Save(); // Deðiþiklikleri kaydet
    }
}
