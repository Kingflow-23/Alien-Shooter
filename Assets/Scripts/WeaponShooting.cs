// WeaponShooting.cs
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    private float nextFireTime;
    private Camera playerCamera;

    public GameObject smokeEffectPrefab;

    private WeaponManager wm;
    private WeaponComponent wc;
    private WeaponData wd;

    public float spawnDistance = 1f; 

    void Start()
    {
        wm = GetComponentInChildren<WeaponManager>();
        playerCamera = Camera.main;
        UpdateWeaponRef();
    }

    void Update()
    {
        // if weapon was switched, refresh its data
        if (wc == null || wc.gameObject != GetEqWeapon())
            UpdateWeaponRef();
    }

    public void TryFire()
    {
        if (wd == null || Time.time < nextFireTime)
        {
            Debug.Log("Cannot fire: Cooldown or missing weapon data.");
            return;
        }

        nextFireTime = Time.time + wd.fireRate;

        Debug.Log("Firing weapon: " + wd.name);

        if (!string.IsNullOrEmpty(wd.fireSoundName))
        {
            AudioManager.instance.PlaySFX(wd.fireSoundName);
        }

        if (wd.isShotgun)
        {
            ShootShotgun();
        }
        else if (wd.projectilePrefab != null)
        {
            ShootProjectile();
        }
        else
        {
            ShootHitscan();
        }
    }

    void UpdateWeaponRef()
    {
        var eq = wm?.CurrentWeapon;
        wc = eq ? eq.GetComponent<WeaponComponent>() : null;
        wd = wc ? wc.weaponData : null;
    }

    GameObject GetEqWeapon() => GetComponent<WeaponManager>()?.CurrentWeapon;

    void ShootProjectile()
    {
        if (wd.projectilePrefab == null || playerCamera == null)
            return;

        // 1) Calculate spawn position & rotation based on camera
        Vector3 dir = playerCamera.transform.forward;
        Vector3 spawnPos = playerCamera.transform.position + dir * spawnDistance;
        Quaternion spawnRot = Quaternion.LookRotation(dir);

        // 2) Instantiate the bullet there
        GameObject bullet = Instantiate(wd.projectilePrefab, spawnPos, spawnRot);

        // 3) Give it straight-line velocity
        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.useGravity = false;
            rb.linearVelocity = dir * wd.projectileForce;
        }
        else
        {
            Debug.LogWarning("Bullet has no Rigidbody: " + bullet.name);
        }

        // 4) Clean up
        Destroy(bullet, 5f);
    }

    void ShootHitscan()
    {
        if (wd.isShotgun)
        {
            Debug.Log("Shotgun mode active.");
            ShootShotgun();
            return;
        }

        // Regular hitscan shot
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, wd.range))
        {
            HandleHit(hit);
        }
    }

    void ShootShotgun()
    {
        if (wd.projectilePrefab == null || playerCamera == null) return;

        for (int i = 0; i < wd.pelletCount; i++)
        {
            Vector3 baseDirection = playerCamera.transform.forward;
            Vector3 spreadDirection = baseDirection + Random.insideUnitSphere * Mathf.Tan(wd.spreadAngle * Mathf.Deg2Rad);
            spreadDirection.Normalize();

            Vector3 spawnPos = playerCamera.transform.position + baseDirection * spawnDistance;
            Quaternion spawnRot = Quaternion.LookRotation(spreadDirection);

            GameObject pellet = Instantiate(wd.projectilePrefab, spawnPos, spawnRot);

            if (pellet.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.useGravity = false;
                rb.linearVelocity = spreadDirection * wd.projectileForce;
                Debug.Log("Pellet velocity: " + rb.linearVelocity);
            }
            else
            {
                Debug.LogWarning("Pellet has no Rigidbody!");
            }

            Destroy(pellet, 5f);
        }
    }

    void HandleHit(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Monster"))
        {
            hit.transform.GetComponent<MonsterHealth>()?.TakeDamage(wd.damage);
        }
        else
        {
            SpawnImpactEffect(hit.point, hit.normal); // Optional
        }
    }

    void SpawnImpactEffect(Vector3 position, Vector3 normal)
    {
        if (wd.impactEffectPrefab == null) return;

        GameObject impact = Instantiate(wd.impactEffectPrefab, position, Quaternion.LookRotation(normal));
        Destroy(impact, 1f); // auto destroy after 2 seconds
    }
}
