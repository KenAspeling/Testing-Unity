using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;
    private float scoreMultiplier = 10f;
    public float thrustForce = 4f;
    Rigidbody2D rb;
    public GameObject Booster;
    public UIDocument uiDocument;
    private Label scoreText;
    private Label highscoreText;
    public GameObject explosion;
    private Button restartButton;
    public GameObject laserPrefab;
    public float shootForce = 1000f;
    public Transform firepoint;
    public float shootDelay = 0.3f;
    private float timePass = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        highscoreText = uiDocument.rootVisualElement.Q<Label>("HighscoreLabel");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
        highscoreText.text = "High Score: " + PlayerPrefs.GetFloat("HighScore",0);
    }

    // Update is called once per frame
    void Update()
    {
        updateScore();
        movePlayer();
        shootLaser();
    }

    void updateScore()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
    }

    void movePlayer()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            //Calculate mouse direction
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;

            //Move player
            transform.up = direction;
            rb.AddForce(direction * thrustForce);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Booster.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Booster.SetActive(false);
        }
    }
    
    void shootLaser()
    {
        timePass += Time.deltaTime;
        if (Keyboard.current.spaceKey.isPressed && (timePass>shootDelay))
        {
            Vector3 spawnPos = firepoint ? firepoint.position : transform.position;
            Quaternion spawnRot = firepoint ? firepoint.rotation : transform.rotation;

            // Create the laser
            GameObject laser = Instantiate(laserPrefab, spawnPos, spawnRot);

            // Add force in the forward direction
            Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(firepoint.up * shootForce, ForceMode2D.Impulse);
            }
            timePass = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;

        if (score > PlayerPrefs.GetFloat("HighScore", 0))
        {
            PlayerPrefs.SetFloat("HighScore", score);
            PlayerPrefs.Save();
            highscoreText.text = "High Score: " + score;
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
