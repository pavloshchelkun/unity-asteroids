using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public Text scoreLabel;
    public Text livesLabel;
    public GameObject gameOverLabel;
    public GameObject restartButton;

    public GameObject playerPrefab;
    public GameObject asteroidPrefab;

    public int lives;
    public float timeToRestart;

    public int startAsteroidCount;
    public float timeToGenerateAsteroid;

    private int score;
    private bool gameOver;
    private float restartTime;

    private int asteroids;

    public void AddScore(int newScore)
    {
        score += newScore;
        UpdateScore();
    }

    public void OnPlayerKilled()
    {
        lives--;
        UpdateLives();

        if (lives <= 0)
        {
            gameOver = true;
            gameOverLabel.SetActive(true);
            restartTime = Time.time + timeToRestart;
        }
        else
        {
            Instantiate(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation);
        }
    }

    public void CreateAsteroid(Vector3 position, int size)
    {
        position = ClampAsteroidPosition(position);
        asteroids++;
        Asteroid asteroid = ((GameObject)Instantiate(asteroidPrefab, position, asteroidPrefab.transform.rotation)).GetComponent<Asteroid>();
        asteroid.SetSize(size);
    }

    public void OnAsteroidDestroyed()
    {
        asteroids--;

        // Check for next level
        if (asteroids <= 0)
        {
            lives++;
            score += 100 * startAsteroidCount * lives;
            startAsteroidCount++;
            GenerateLevel();
        }
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void GenerateAsteroid()
    {
        CreateAsteroid(GetRandomSidePosition(), 3);
    }

    private void GenerateLevel()
    {
        gameOverLabel.SetActive(false);
        restartButton.SetActive(false);

        UpdateScore();
        UpdateLives();

        for (int i = 0; i < startAsteroidCount; i++)
        {
            GenerateAsteroid();
        }
    }

    private Vector3 GetRandomSidePosition()
    {
        int side = Random.Range(1, 5);
        float x = 0f;
        float y = 0f;

        // Screen side
        switch (side)
        {
            case 1: // Left
                y = Random.Range(0f, 1f);
                break;
            case 2: // Right
                x = 1f;
                y = Random.Range(0f, 1f);
                break;
            case 3: // Top
                x = Random.Range(0f, 1f);
                y = 1f;
                break;
            case 4: // Bottom
                x = Random.Range(0f, 1f);
                break;
        }

        Vector3 position = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0.0f));
        position.y = 0f;

        return position;
    }

    // Helps to keep object position inside the screen space
    private Vector3 ClampAsteroidPosition(Vector3 position)
    {
        Vector3 point = Camera.main.WorldToViewportPoint(position);

        point.x = Mathf.Clamp(point.x, 0f, 1f);
        point.y = Mathf.Clamp(point.y, 0f, 1f);

        position = Camera.main.ViewportToWorldPoint(point);
        position.y = 0f;

        return position;
    }

    private void Awake()
    {
        // Singleton initialization
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void UpdateScore()
    {
        scoreLabel.text = "Score: " + score;
    }

    private void UpdateLives()
    {
        livesLabel.text = "Lives: " + lives;
    }

    private void Start()
    {
        GenerateLevel();
    }

    private void Update()
    {
        // If game is over, check for restart
        if (gameOver && Time.time > restartTime)
        {
            restartButton.SetActive(true);
            gameOver = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
