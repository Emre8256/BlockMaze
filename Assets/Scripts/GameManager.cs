using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private ParticleSystem[] arrow;
    [SerializeField] private AudioSource NormalButton;

    static public bool jumpsound;

    void Start()
    {
        // Cihazýn ekran yenileme hýzýný al
        double refreshRate = Screen.currentResolution.refreshRateRatio.value;

        // FPS ayarýný ekran yenileme hýzýna göre ayarla
        Application.targetFrameRate = Mathf.RoundToInt((float)refreshRate);

        Time.timeScale = 1.0f;
        jumpsound = true;
    }


    void Update()
    {
        if (PlayerController.isGameOver)
        {
            if (arrow.Length > 0)
            {
                for (int i = 0; i < arrow.Length; i++)
                {
                    if (arrow[i] != null)
                    {
                        arrow[i].gameObject.SetActive(false);
                    }
                }
            }
        }


    }

    public void OpenPausePanel()
    {
        if (MainMenu.IsSoundOn) { NormalButton.Play(); }
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        jumpsound = false;
        for (int i = 0; i < arrow.Length; i++)
        {
            if (arrow[i] != null)
            {
                arrow[i].gameObject.SetActive(false);
            }
        }
    }

    public void Resume()
    {
        if (MainMenu.IsSoundOn) { NormalButton.Play(); }
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        Invoke("OpenjumpSoundWithDelay", 1f);
        for (int i = 0; i < arrow.Length; i++)
        {
            if (arrow[i] != null)
            {
                arrow[i].gameObject.SetActive(true);
            }
        }
    }

    public void goMainMenu()
    {
        if (MainMenu.IsSoundOn) { NormalButton.Play(); }
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        if (MainMenu.IsSoundOn) { NormalButton.Play(); }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        if (MainMenu.IsSoundOn) { NormalButton.Play(); }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OpenjumpSoundWithDelay()
    {
        jumpsound = true;
    }
}
