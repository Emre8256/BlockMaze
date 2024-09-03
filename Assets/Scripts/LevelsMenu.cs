using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages;
    [SerializeField] public Button nextButton; // Ýleri butonu
    [SerializeField] public Button prevButton; // Geri butonu

    [SerializeField] private List<Image> PageCircles;
    [SerializeField] private Color activeDotColor; // Aktif nokta rengi
    [SerializeField] private Color inactiveDotColor; // Pasif nokta rengi

    [SerializeField] private AudioSource Nextpage;
    [SerializeField] private AudioSource NormalButton;

    [SerializeField] GameObject Unlock;
    private const int maxLevel = 100;  // Maksimum seviye sayýsý



    private int currentPage = 0;
    void Start()
    {
        // Cihazýn ekran yenileme hýzýný al
        double refreshRate = Screen.currentResolution.refreshRateRatio.value;

        // FPS ayarýný ekran yenileme hýzýna göre ayarla
        Application.targetFrameRate = Mathf.RoundToInt((float)refreshRate);

        UpdatePage();
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
        // Oyunun baþýnda PlayerPrefs'teki LastUnlockedLevel deðerini kontrol et

        

    }

    private void Awake()
    {
        if(PlayerPrefs.GetInt("LastUnlockedLevel") == 100) Destroy(Unlock);

        if (Application.internetReachability == NetworkReachability.NotReachable) Unlock.SetActive(false);
    }

    
    
    

    void UpdatePage()
    {
        // Tüm panelleri kapat
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        // Aktif paneli aç
        pages[currentPage].SetActive(true);

        // Noktalarý güncelle
        for (int i = 0; i < PageCircles.Count; i++)
        {
            if (i == currentPage)
            {
                SetDotColor(PageCircles[i], activeDotColor);
            }
            else
            {
                SetDotColor(PageCircles[i], inactiveDotColor);
            }
        }

        // Butonlarý güncelle
        prevButton.interactable = currentPage > 0;
        nextButton.interactable = currentPage < pages.Count - 1;
    }

    void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            currentPage++;
            if (MainMenu.IsSoundOn) { Nextpage.Play(); }
            UpdatePage();
        }
    }

    void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            if (MainMenu.IsSoundOn) { Nextpage.Play(); }
            UpdatePage();
        }
    }

    void SetDotColor(Image dot, Color color)
    {
        Color newColor = color;
        newColor.a = dot.color.a; // Alpha deðerini koru
        dot.color = newColor;
    }

    public void goMainMenu()
    {
        if (MainMenu.IsSoundOn) { NormalButton.Play(); }
        SceneManager.LoadScene("MainMenu");
    }

    

    
}
