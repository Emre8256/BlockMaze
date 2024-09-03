using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.tutorialOff)
        {
            tutorial.SetActive(false);
        }
    }
}
