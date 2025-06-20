using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public GameObject blackHoleEffectPrefab;
    public float blackHoleDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        // Check for contact with Goal, Monster, or Ground
        if (other.CompareTag("Monster") || other.CompareTag("Goal") || other.CompareTag("Ground"))
        {
            if (blackHoleEffectPrefab != null)
            {
                InstantiateBlackHole(transform.position);
            }   
        }

        // Deal damage if it's a Monster
        if (other.CompareTag("Monster"))
        {
            var health = other.GetComponent<MonsterHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        // Destroy bullet in all cases
        Destroy(gameObject, 5f);
    }

    void InstantiateBlackHole(Vector3 position)
    {
        GameObject blackHole = Instantiate(blackHoleEffectPrefab, position, Quaternion.identity);
        Destroy(blackHole, blackHoleDuration);
    }
}