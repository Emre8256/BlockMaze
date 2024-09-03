using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Turning : MonoBehaviour
{
    [SerializeField] int TurningSpeed;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (!PlayerController.isGameOver)
            transform.Rotate(Vector3.forward * TurningSpeed * Time.deltaTime);
    }
}
