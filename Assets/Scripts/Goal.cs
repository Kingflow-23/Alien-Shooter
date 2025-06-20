using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameManager gameManager; // Assign this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.CompareTag("Player");
        bool isBullet = other.CompareTag("Bullet");

        if (isPlayer || isBullet)
        {
            if (gameManager == null)
            {
                gameManager = FindFirstObjectByType<GameManager>();
            }

            if (gameManager != null)
            {
                gameManager.ShowWinMessage();
            }

            Destroy(gameObject); // Optional: remove goal object after triggering
        }
    }
}