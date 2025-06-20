using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterHealth : MonoBehaviour
{
    public GameObject healthBarPrefab;
    private Transform healthBarUI;
    private Image healthFill;
    private MonsterMotor motor;
    public float maxHealth = 100f;
    private float currentHealth;
    private GameManager gameManager; 
     private MonsterMovement monsterMovement;
     private bool isDead = false;

    void Start()
    {
        motor = GetComponent<MonsterMotor>(); // Ensure your monster has a motor component
        currentHealth = maxHealth;

        monsterMovement = GetComponent<MonsterMovement>();

        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();  // Finds the GameManager in the scene
        }

        // Instantiate health bar prefab
        GameObject hb = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2.5f, Quaternion.identity);

        // Ensure canvas is world space and not scaled weird
        hb.transform.localScale = Vector3.one * 0.01f; // Adjust based on your prefab scale
        healthBarUI = hb.transform;

        // Optionally parent to monster so it moves with it
        healthBarUI.SetParent(transform);  // You can remove this if you want manual positioning

        // Find the image component named "Fill" inside the prefab
        healthFill = hb.transform.Find("Fill")?.GetComponent<Image>();

        if (healthFill == null)
            Debug.LogWarning("Health fill image not found. Make sure your prefab has a child named 'Fill'.");
    }

    void Update()
    {
        if (healthBarUI != null)
        {
            // Position above monster
            healthBarUI.position = transform.position + Vector3.up * 2f;

            // Always face the camera
            healthBarUI.LookAt(Camera.main.transform);
            healthBarUI.Rotate(0, 180, 0);  // So it doesn't appear flipped
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthFill != null)
            healthFill.fillAmount = currentHealth / maxHealth;

        if (currentHealth > 0)
        {
            StartCoroutine(PlayHitAndRun()); 
        }
        else
        {
            motor.PlayDeath(); // ☠️ Death animation
            Die();
        }
    }

    private IEnumerator PlayHitAndRun()
    {
        motor.PlayHit(); // 💥 Play hit reaction animation
        yield return new WaitForSeconds(1f); // Wait for 1 second
        motor.PlayRun(); // 🏃‍♂️ Resume running animation
    }

    void Die()
    {
        if (isDead) return;
        isDead = true; // Prevent multiple calls to Die

        // Hide health bar and deactivate
        if (healthBarUI != null)
            healthBarUI.gameObject.SetActive(false);

        if (gameManager != null && monsterMovement != null)
        {
            gameManager.EnemyDied(monsterMovement.scoreValue);  // Pass the scoreValue to the GameManager
        }
        AudioManager.instance.PlaySFX("MonsterDeath"); // Play death sound

        Destroy(gameObject, 3f); // Destroy the monster
    }

    public bool IsDead => isDead;
}
