using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(MonsterMotor))]

public class MonsterMovement : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRadius = 10f;
    public float minPatrolTime = 3f;
    public float maxPatrolTime = 7f;

    [Header("Agro Settings")]
    public float agroRadius = 8f;
    public float attackRange = 1.5f;
    private float lastAttackTime;
    public float attackCooldown = 1.5f;
    public float attackDamage;

    [Header("Speeds")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float rotationSpeed = 5f;

    private NavMeshAgent agent;
    private Transform player;
    private MonsterMotor motor;
    private MonsterHealth monsterHealth;
    

    private Coroutine patrolCoroutine;
    private bool isChasing;
    private bool inAttackRange;
    public int scoreValue;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        motor = GetComponent<MonsterMotor>();
        monsterHealth = GetComponent<MonsterHealth>();

        player = GameObject.FindWithTag("Player")?.transform;

        StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        if (monsterHealth != null && monsterHealth.IsDead)
        {
            return; // Exit Update if dead
        }
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 1) Out of agro → patrol
        if (distance > agroRadius)
        {
            if (isChasing)
            {
                StopChasing();
                StartPatrol();
            }   
            return;
        }

        // 2) Within agro → stop patrolling, start chasing if not already
        if (!isChasing)
        {
            StopPatrol();
            StartChasing();
        }

        // 3) If close enough to attack
        if (distance <= attackRange)
        {
            if (!inAttackRange)
            {
                // just entered attack range
                inAttackRange = true;
                agent.ResetPath();              // stop the agent
                motor.PlayIdle();               // or a 'wound' idle stance
            }

            RotateTowards(player.position);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                motor.PlayAttack();
                AudioManager.instance.PlaySFX("Monster");
                lastAttackTime = Time.time;

                DealDamageToPlayer(); 
            }

            return;
        }

        // 4) Outside attack range → resume run once, then keep chasing
        if (inAttackRange)
        {
            inAttackRange = false;
            motor.PlayRun();
        }

        // 5) Continue chasing every frame
        agent.SetDestination(player.position);
    }

    private void StartPatrol()
    {
        StopChasing();
        if (patrolCoroutine == null)
            patrolCoroutine = StartCoroutine(PatrolRoutine());
    }

    private void StopPatrol()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }
    }

    private void StartChasing()
    {
        isChasing = true;
        inAttackRange = false;
        motor.PlayRun();
        AudioManager.instance.PlaySFX("Monster");
        agent.speed = chaseSpeed;
    }

    private void StopChasing()
    {
        isChasing = false;
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // 1) Move to the next patrol point
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * patrolRadius;
            randomPoint.y = transform.position.y;

            agent.speed = patrolSpeed;
            agent.SetDestination(randomPoint);

            motor.PlayWalk();

            // 2) Wait until we arrive
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance + 0.1f)
                yield return null;

            // 3) We’re stopped—idle for a bit
            motor.PlayIdle();
            float waitTime = Random.Range(minPatrolTime, maxPatrolTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    void DealDamageToPlayer()
    {
        // Ensure we only apply damage if the player is still alive
        if (player != null)
        {
            var playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);  // Apply the attack damage
                Debug.Log("Monster dealt " + attackDamage + " damage to player.");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, agroRadius);
    }
}
