using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Health Pickup Spawning")]
    public GameObject healthPickupPrefab;
    public int maxHealthPickups = 20;
    public float healthMinSpawnDist = 50f;
    public float healthMaxSpawnDist = 100f;

    [Header("UI Elements")]
    public Button restartButton;
    public TextMeshProUGUI winMessage;
    public TextMeshProUGUI loseMessage;
    public TextMeshProUGUI scoreText; 

    [Header("Snow FX")]
    public GameObject snowEffectPrefab;

    [Header("Goal Settings")]
    public GameObject goalPrefab;
    public float goalSpawnMinDist = 50f;
    public float goalSpawnMaxDist = 100f;

    private GameObject spawnedGoal;
    private bool goalSpawned = false;

    [Header("Enemy Spawning")]
    public GameObject[] enemyPrefabs;     // Array of enemy prefabs (assign in inspector)
    public float spawnInterval = 3f;      // Seconds between spawns
    public int maxEnemies = 15;
    public float spawnRadius = 70;
    public Transform playerTransform;     // Drag the player object here in inspector

    private int currentEnemyCount = 0;
    private int score = 0;

    void Start()
    {
        // Snow VFX
        if (snowEffectPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(0, 20f, 0);
            Instantiate(snowEffectPrefab, spawnPosition, Quaternion.identity);
        }

        AudioManager.instance.PlayMusic("Main");

        restartButton.gameObject.SetActive(false);
        winMessage.gameObject.SetActive(false);
        loseMessage.gameObject.SetActive(false);

        score = 0;
        UpdateScoreUI();

        SpawnHealthPickups();
        SpawnGoal(); 

        StartCoroutine(SpawnEnemiesRoutine());
    }

    void SpawnHealthPickups()
    {
        if (healthPickupPrefab == null || playerTransform == null) return;

        for (int i = 0; i < maxHealthPickups; i++)
        {
            Vector3 randomDir = Random.onUnitSphere;
            randomDir.y = 0;

            float distance = Random.Range(healthMinSpawnDist, healthMaxSpawnDist);
            Vector3 spawnPos = playerTransform.position + randomDir.normalized * distance;

            Instantiate(healthPickupPrefab, spawnPos, Quaternion.identity);
        }
    }

    void SpawnGoal()
    {
        if (goalPrefab == null || playerTransform == null) return;

        Vector3 randomDir = Random.onUnitSphere;
        randomDir.y = 0;

        float distance = Random.Range(goalSpawnMinDist, goalSpawnMaxDist);
        Vector3 spawnPos = playerTransform.position + randomDir.normalized * distance;

        spawnedGoal = Instantiate(goalPrefab, spawnPos, Quaternion.identity);
        spawnedGoal.SetActive(false); // keep it inactive until score hits threshold
        goalSpawned = true;
    }

    // Enemy Spawning Routine
    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || playerTransform == null)
            return;

        Vector3 randomDir = Random.onUnitSphere;
        randomDir.y = 0;
        Vector3 spawnPos = playerTransform.position + randomDir.normalized * spawnRadius;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemyCount++;
    }

    public void EnemyDied(int scoreValue)
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
        AddScore(scoreValue);
    }

    void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score); 
        UpdateScoreUI();

        if (score >= 20 && goalSpawned && spawnedGoal != null && !spawnedGoal.activeSelf)
        {
            spawnedGoal.SetActive(true);
            Debug.Log("Goal activated!");
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Kills: " + score;
        }
    }   

    // UI Controls
    public void ShowWinMessage()
    {
        winMessage.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        AudioManager.instance.PlaySFX("Win");

        // Make sure the cursor is visible and unlocked when the game ends
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowLoseMessage()
    {
        loseMessage.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        AudioManager.instance.PlaySFX("GameOver");

        // Make sure the cursor is visible and unlocked when the game ends
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
