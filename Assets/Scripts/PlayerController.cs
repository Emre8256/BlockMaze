using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 direction;
    private float distance;
    private Rigidbody2D rb;

    [SerializeField] float maxPower = 10f;
    [SerializeField] float kat = 1.3f;
    [SerializeField] GameObject arrow;
    private bool canShoot;

    [SerializeField] GameObject LevelCompletedPanel;
    [SerializeField] GameObject Star;
    [SerializeField] ParticleSystem StarExplosion;
    public static bool isGameOver;
    public static bool isStarTaken;
    private Renderer objRenderer;

    [SerializeField] ParticleSystem PlayerDead;
    private bool onGround;
    private Camera _camera;

    [SerializeField] private AudioSource dead;
    [SerializeField] private AudioSource star;
    [SerializeField] private AudioSource win;
    [SerializeField] private AudioSource jump;

    public static bool tutorialOff;
    


    private Vector3 startPosition;
    private float startRotation;

    FallingBlock fb;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.eulerAngles.z;
        rb = GetComponent<Rigidbody2D>();

        
    }

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody2D>();
        arrow.SetActive(false);
        isGameOver = false;
        isStarTaken = false;
        _camera = Camera.main;
        tutorialOff = false;
    }

    void Update()
    {
        if (Time.timeScale != 0f && !isGameOver && onGround) ClickPosition();
        if (rb.velocity.magnitude >= 1f) arrow.SetActive(false);
    }

    void Shoot()
    {
        if (arrow.activeSelf)
        {
            float power = Mathf.Clamp(distance * kat, 0, maxPower);
            rb.AddForce(direction * power, ForceMode2D.Impulse);
            canShoot = false;
            arrow.SetActive(false);
        }
    }

    void ClickPosition()
    {
        if (rb.velocity.magnitude < 1f)
            canShoot = true;

        if (canShoot)
        {
            Vector2 currentPos = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                startPos = currentPos;
                arrow.SetActive(true);
            }

            if (Input.GetMouseButton(0))
            {
                distance = Vector2.Distance(startPos, currentPos);
                direction = (startPos - currentPos).normalized;
                UpdateArrow(currentPos);
            }

            if (Input.GetMouseButtonUp(0))
            {


                endPos = currentPos;
                float dragDistance = Vector2.Distance(startPos, endPos);

                if (arrow.activeSelf && dragDistance >= 0.3f)
                {
                    // Ses sadece mesafe belirli bir deðerden fazla olduðunda çalacak
                    if (MainMenu.IsSoundOn && GameManager.jumpsound)
                    {
                        jump.Play();
                    }

                }

                if (arrow.activeSelf && dragDistance >= 1f)
                {
                    tutorialOff = true;
                }
                Shoot();

            }
        }
    }

    void UpdateArrow(Vector2 currentPos)
    {
        Vector2 arrowDirection = startPos - currentPos;
        float angle = Mathf.Atan2(arrowDirection.y, -arrowDirection.x) * Mathf.Rad2Deg;

        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        float power = Mathf.Clamp(arrowDirection.magnitude, 0, maxPower);
        float scaleFactor = power / maxPower;

        arrow.transform.localScale = new Vector3(scaleFactor * (maxPower / 40f), 0.1f, 1);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Star")
        {
            if (MainMenu.IsSoundOn) { star.Play(); }
            Star.SetActive(false);
            isStarTaken = true;
            StarExplosion.Play();
            Invoke("OpenLevelCompPanel", 1.5f);
            
        }

        if (collision.tag == "Fall")
        {
            if (!isStarTaken && !isGameOver)
            {
                
               

                if (SceneManager.GetActiveScene().buildIndex == 76 || SceneManager.GetActiveScene().buildIndex == 48)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                else {
                    if (rb != null)
                    {
                        rb.velocity = Vector2.zero;
                        rb.angularVelocity = 0f;
                    }
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    transform.position = startPosition; 
                    ResetFallingBlocks(); 
                }


                    



            }

        }

        if (collision.tag == "Thorn")
        {
            if (!isStarTaken && !isGameOver)
            {
                if (MainMenu.IsSoundOn) { dead.Play(); }
                if (MainMenu.IsVibrateOn) { Handheld.Vibrate(); }
                this.gameObject.SetActive(false);
                
                PlayerDead.transform.position = this.gameObject.transform.position;
                PlayerDead.Play();
                Invoke("Retry", 1f);
                
            }
        }

        if (collision.tag == "Falling")
        {
            if (!isStarTaken && !isGameOver)
            {
                if (MainMenu.IsSoundOn) { dead.Play(); }
                if (MainMenu.IsVibrateOn) { Handheld.Vibrate(); }
                this.gameObject.SetActive(false);
                
                PlayerDead.transform.position = this.gameObject.transform.position;
                PlayerDead.Play();
                Invoke("Retry", 1f);
                
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") onGround = true;
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") onGround = false;
    }


    void OpenLevelCompPanel()
    {
        LevelCompletedPanel.SetActive(true);
        if (MainMenu.IsSoundOn) { win.Play(); }
        isGameOver = true;
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneManager.GetActiveScene().buildIndex != 100)
        {
            LevelsName.UnlockNextLevel(currentLevelIndex);
            PlayerPrefs.Save();
        }

    }

    [SerializeField] private Transform playerHolder;
    void Retry()
    {
        if (PlatformMove.parentingyatay || PlatformUD.parentingdikey) { if (playerHolder != null) this.gameObject.transform.SetParent(playerHolder, true); }
        if (SceneManager.GetActiveScene().buildIndex == 76 || SceneManager.GetActiveScene().buildIndex == 48)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            transform.position = startPosition;
            transform.eulerAngles = new Vector3(0, 0, 0); // Rotayý sýfýrla
            gameObject.SetActive(true);

            ResetFallingBlocks();
        }
    }

    private void ResetFallingBlocks()
    {
        GameObject[] fallingBlocks = GameObject.FindGameObjectsWithTag("Falling");

        foreach (GameObject block in fallingBlocks)
        {
            FallingBlock blockScript = block.GetComponent<FallingBlock>();
            if (blockScript != null)
            {
                blockScript.ResetBlock(); // Her bloðun resetlenmesini saðla
            }
        }
    }


}