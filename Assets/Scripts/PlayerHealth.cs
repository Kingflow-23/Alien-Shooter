using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;
    public Image healthBar; 
    public TextMeshProUGUI gameLostText;
    private Animator anim;
    public GameManager gameController;

    void Start()
    {
        gameLostText.gameObject.SetActive(false);
        currentHealth = maxHealth;
        UpdateHealthUI();
        anim = GetComponent<Animator>();  // Ensure your Player has an Animator component.
    }

    // Call this method to apply damage to the player.
    public void TakeDamage(float amount)
    {
        Debug.Log("Taking damage: " + amount);
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        anim.SetTrigger("Hit"); // Play hit animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Call this method to heal the player.
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    // Update the UI health bar based on the current health.
    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            float fillValue = currentHealth / maxHealth;
            healthBar.fillAmount = fillValue;
        }
        else
        {
            Debug.LogWarning("Health bar image is not assigned!");
        }
    }

    // Handle player death.
    void Die()
    {
        if (anim != null)
        {
            anim.SetTrigger("Death");
        }

        gameController.ShowLoseMessage();
    }
}
