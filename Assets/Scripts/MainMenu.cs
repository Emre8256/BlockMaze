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
        // Cihaz�n ekran yenileme h�z�n� al
        double refreshRate = Screen.currentResolution.refreshRateRatio.value;
        
        // FPS ayar�n� ekran yenileme h�z�na g�re ayarla
        Application.targetFrameRate = Mathf.RoundToInt((float)refreshRate);

        Time.timeScale = 1f;

        if (!IsSoundOn) soundWaveImg.SetActive(true);
        if (!IsVibrateOn) vibrateWaveImg.SetActive(true);

        // PlayerPrefs'te daha �nce bir de�er olup olmad���n� kontrol edin
        if (!PlayerPrefs.HasKey("LevelUnlocked_1"))
        {
            // �lk kez a��l��ta, seviyelerin varsay�lan durumunu ayarla
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
        // En son aktif olan level'i al�yoruz, e�er kaydedilmediyse default olarak Level 1'i al�r
        int lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 1);

        // En son aktif olan level'i y�kl�yoruz
        SceneManager.LoadScene("Level" + lastUnlockedLevel);
    }

    void InitializeLevelUnlocks()
    {
        // Level 1 her zaman a��lm�� olmal�
        PlayerPrefs.SetInt("LevelUnlocked_1", 1);
        PlayerPrefs.SetInt("LastUnlockedLevel", 1);

        // Di�er seviyeleri kilitle
        for (int i = 2; i <= 100; i++)
        {
            PlayerPrefs.SetInt("LevelUnlocked_" + i, 0);
        }

        PlayerPrefs.Save(); // De�i�iklikleri kaydet
    }
}
