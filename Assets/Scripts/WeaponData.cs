using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float fireRate;
    public float damage;
    public float range;
    public GameObject projectilePrefab;
    public float projectileForce;

    public GameObject impactEffectPrefab;

    // Shotgun-specific settings
    public bool isShotgun = false;
    public int pelletCount = 8;
    public float spreadAngle = 5f;

    [Header("Sound Settings")]
    public string fireSoundName;
}