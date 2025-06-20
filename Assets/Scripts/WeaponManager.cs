// WeaponManager.cs
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weaponPrefabs;
    public Transform weaponSocket;

    private GameObject currentWeapon;
    private int currentIndex = 0;

    public GameObject CurrentWeapon => currentWeapon;

    void Start()
    {
        EquipWeapon(0);
    }

    void Update()
    {
        // Don't proceed if there are no weapons
        if (weaponPrefabs == null || weaponPrefabs.Length == 0)
            return;

        // number keys
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipWeapon(i);
            }
        }

        // scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            EquipWeapon((currentIndex + 1) % weaponPrefabs.Length);
        }
        else if (scroll < 0f)
        {
            EquipWeapon((currentIndex - 1 + weaponPrefabs.Length) % weaponPrefabs.Length);
        }
    }

    void EquipWeapon(int index)
    {
        if (index < 0 || index >= weaponPrefabs.Length || weaponSocket == null) return;

        // Disable all weapons
        foreach (var weapon in weaponPrefabs)
        {
            if (weapon != null)
                weapon.SetActive(false);
        }

        currentIndex = index;
        currentWeapon = weaponPrefabs[index];
        currentWeapon.SetActive(true);

        // Re-parent to weapon socket
        currentWeapon.transform.SetParent(weaponSocket, false);

        Debug.Log("Equipped: " + currentWeapon.name + " | Pos: " + currentWeapon.transform.position);
    }
}
