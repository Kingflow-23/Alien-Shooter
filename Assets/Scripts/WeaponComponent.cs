using UnityEngine;

// Attach this to each weapon prefab
public class WeaponComponent : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform shootPoint;

    public Transform hipPosition;
    public Transform adsPosition;

    [HideInInspector] public bool isAiming = false;
}