using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformUD : MonoBehaviour
{
    [SerializeField] private float speed = 2f;  // Platformun hareket h�z�
    [SerializeField] private float distance = 5f;  // Platformun hareket edece�i uzakl�k
    


    [SerializeField] private Transform playerHolder;
    [SerializeField] GameObject player;

    private Vector3 startPosition;
    public static bool parentingdikey;

    void Start()
    {
        startPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController.isGameOver)
        {
            // Platformun pozisyonunu PingPong fonksiyonu ile d�zenli olarak yukar� ve a�a�� hareket ettir
            float newY = Mathf.PingPong(Time.time * speed, distance) - (distance / 2f);
            transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
        }

        
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.transform.position.y > transform.position.y)
            {
                parentingdikey = true;
                collision.transform.SetParent(this.gameObject.transform, true);
                
            }
        }

    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        // Oyuncu platformdan ayr�ld���nda, �ocu�u olmaktan ��kar
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.activeSelf)
            {
                parentingdikey=false;
                collision.transform.SetParent(playerHolder, true);
                
                
            }

        }
    }



   
}
