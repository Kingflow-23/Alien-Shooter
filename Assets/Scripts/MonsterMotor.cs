using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MonsterMotor : MonoBehaviour
{
    [Header("Animation Clips (State Names in Base Layer)")]
    [Tooltip("Idle animations")]
    public string[] idleStates;
    [Tooltip("Walk animations")]
    public string[] walkStates;
    [Tooltip("Run animations")]
    public string[] runStates;
    [Tooltip("Rage animations")]
    public string[] rageStates;
    [Tooltip("Attack animations")]
    public string[] attackStates;
    [Tooltip("Hit reactions")]
    public string[] hitStates;
    [Tooltip("Death animations (played once)")]
    public string[] deathStates;

    private Animator animator;
    private bool isDead = false;
    public string CurrentState { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        // Ensure your Animator's default state is an idle (e.g. idleStates[0])
    }

    /// <summary>
    /// Plays one random Idle clip (looped).
    /// </summary>
    public void PlayIdle()
    {
        Debug.Log("PlayIdle called");

        if (idleStates.Length == 0) return;
        PlayRandomFromList(idleStates);
    }

    /// <summary>
    /// Plays one random Walk clip (looped).
    /// </summary>
    public void PlayWalk()
    {
        Debug.Log("PlayWalk called");

        if (walkStates.Length == 0 || isDead) return;
        PlayRandomFromList(walkStates);
    }

    public void PlayRage()
    {
        Debug.Log("PlayRage called");

        if (rageStates.Length == 0 || isDead) return;
        PlayRandomFromList(rageStates);
    }

    /// <summary>
    /// Plays one random Run clip (looped).
    /// </summary>
    public void PlayRun()
    {
        Debug.Log("PlayRun called");

        if (runStates.Length == 0 || isDead) return;
        PlayRandomFromList(runStates);
    }

    /// <summary>
    /// Plays one random Attack clip (non‑looped).
    /// </summary>
    public void PlayAttack()
    {
        Debug.Log("PlayAttack called");

        if (attackStates.Length == 0 || isDead) return;
        PlayRandomFromList(attackStates);
    }

    /// <summary>
    /// Plays one random Hit reaction (non‑looped).
    /// </summary>
    public void PlayHit()
    {
        Debug.Log("PlayHit called");

        if (hitStates.Length == 0 || isDead) return;
        PlayRandomFromList(hitStates);
    }

    /// <summary>
    /// Plays one random Death animation and marks monster as dead.
    /// </summary>
    public void PlayDeath()
    {
        Debug.Log("PlayDeath called");
        
        if (deathStates.Length == 0 || isDead) return;
        isDead = true;
        PlayRandomFromList(deathStates);
    }

    /// <summary>
    /// Internal helper to pick a random stateName from an array and cross‑fade into it.
    /// </summary>
    public void PlayRandomFromList(string[] states)
    {
        int idx = Random.Range(0, states.Length);
        string state = states[idx];

        if (CurrentState == state) return; // avoid repeating
        animator.Play(state, 0);
        CurrentState = state;
    }
}
