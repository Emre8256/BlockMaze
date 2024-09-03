using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    [SerializeField] float speed = 2f;  // Platformun hareket hýzý
    [SerializeField] float distance = 5f;  // Platformun hareket edeceði uzaklýk

    private Vector2 startPosition;
    [SerializeField] private Transform playerHolder;

    [SerializeField] GameObject player;

    public static bool parentingyatay;


    void Start()
    {
        startPosition = transform.position;  // Platformun baþlangýç pozisyonunu kaydet
           
    }

    
    void Update()
    {
        if (!PlayerController.isGameOver)
        {
            // Platformun pozisyonunu PingPong fonksiyonu ile düzenli olarak ileri geri hareket ettir
            float newX = Mathf.PingPong(Time.time * speed, distance) - (distance / 2f);
            transform.position = new Vector3(startPosition.x + newX, startPosition.y, transform.position.z);
        }

        
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.transform.position.y > transform.position.y )
            {
                parentingyatay = true;
                collision.transform.SetParent(this.gameObject.transform, true);
                
            }
        }
       
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        // Oyuncu platformdan ayrýldýðýnda, çocuðu olmaktan çýkar
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.activeSelf)
            {
                parentingyatay = false;
                collision.transform.SetParent(playerHolder,true);
                
                
            }

        }
    }


}
