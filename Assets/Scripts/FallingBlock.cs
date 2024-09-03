using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    [SerializeField] Transform P1, P2;
    [SerializeField] Transform Player;
    

    private Vector3 startPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();

    }

    void Start()
    {
         rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.position.x > P1.position.x && Player.position.x < P2.position.x && transform.position.y > Player.position.y)
        {
            rb.gravityScale = 1f;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Fall")
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    public void ResetBlock()
    {
        // Konumu sýfýrla
        transform.position = startPosition;

        // Hýzý sýfýrla
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            // Gravitasyonu 0 yap
            rb.gravityScale = 0f;
        }
    }
}
