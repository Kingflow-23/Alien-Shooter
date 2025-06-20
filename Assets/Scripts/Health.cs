using UnityEngine;


public class Health : MonoBehaviour
{
    public int healthValue = 10; // Amount of health to give

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = null;

        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();
        }
        else if (other.CompareTag("Bullet"))
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
            }
        }

        if (playerHealth != null)
        {
            playerHealth.Heal(healthValue);
            Debug.Log("Player healed for " + healthValue);
            Destroy(gameObject); // Destroy the health pickup
        }
    }
}
