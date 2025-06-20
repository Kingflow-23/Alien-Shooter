using UnityEngine;

public class WeaponAiming : MonoBehaviour
{
    public float aimSpeed = 10f;
    private WeaponManager wm;
    private WeaponComponent wc;

    void Start()
    {
        wm = GetComponent<WeaponManager>();
    }

    void Update()
    {
        var weapon = wm.CurrentWeapon;
        if (weapon == null) return;

        var wc = weapon.GetComponent<WeaponComponent>();
        if (wc == null || wc.adsPosition == null || wc.hipPosition == null) return;

        wc.isAiming = Input.GetMouseButton(1);
        Transform target = wc.isAiming ? wc.adsPosition : wc.hipPosition;

        // Move and rotate weapon to match the target position relative to the weaponSocket
        weapon.transform.localPosition = Vector3.Lerp(
            weapon.transform.localPosition,
            target.localPosition,
            Time.deltaTime * aimSpeed
        );

        weapon.transform.localRotation = Quaternion.Lerp(
            weapon.transform.localRotation,
            target.localRotation,
            Time.deltaTime * aimSpeed
        );
    }
}