using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Transform TeleportLocation;
    [SerializeField] bool TersMi;


    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (TersMi)
            {
                // Oyuncunun Rigidbody2D bile�enini al
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Oyuncunun mevcut h�z�n� al
                    Vector2 currentVelocity = rb.velocity;

                    // Oyuncuyu teleport et
                    collision.transform.position = TeleportLocation.position;

                    // Yeni pozisyona oyuncunun mevcut h�z�n� uygula
                    rb.velocity = -currentVelocity;
                }
            }

            if (!TersMi)
            {
                collision.transform.position = TeleportLocation.position;
            }
        }
    }
}
