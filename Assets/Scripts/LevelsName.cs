using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsName : MonoBehaviour
{
    [SerializeField] Button LevelButton;
    [SerializeField] GameObject Locked;
    private const int maxLevel = 100;


    void Start()
    {
        if (LevelButton != null)
        {
            LevelButton.onClick.AddListener(LoadLevel);
        }
    }
    private void Awake()
    {
        LockedControl();

    }

    void Update()
    {

    }

    void LoadLevel()
    {
        SceneManager.LoadScene(LevelButton.name);
    }

    void LockedControl()
    {
        // Bu level'in kilidi açýlmýþ mý kontrol ediyoruz.
        int levelIndex = int.Parse(LevelButton.name.Replace("Level", "")); // Level ismini sayýya çeviriyoruz
        if (PlayerPrefs.GetInt("LevelUnlocked_" + levelIndex) == 1)
        {
            Locked.SetActive(false); // Eðer kilidi açýlmýþsa Locked imajýný kapat
        }

        // Locked durumu aktif mi deðil mi kontrol ediyoruz.
        LevelButton.interactable = !Locked.activeSelf;
    }

    public static void UnlockNextLevel(int currentLevelIndex)
    {
        int nextLevelIndex = currentLevelIndex + 1;
        

        // En son açýlan level bilgisini kaydediyoruz
        if( PlayerPrefs.GetInt("LevelUnlocked_" + nextLevelIndex) !=1)
        PlayerPrefs.SetInt("LastUnlockedLevel", nextLevelIndex);

        PlayerPrefs.SetInt("LevelUnlocked_" + nextLevelIndex, 1);

        PlayerPrefs.Save();
    }

   


}
