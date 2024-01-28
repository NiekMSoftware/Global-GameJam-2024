using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Weapon[] weapons;
    [SerializeField] private LaughValue laughManager;
    [SerializeField] int currentWeaponNum;
    [SerializeField] int unlockedWeapon;
    [SerializeField] Weapon currentWeapon;
    [SerializeField] private Camera cam;
    [SerializeField] float maxGlobalCd = 1;
    [SerializeField] int oldWeapon;

    [Header("Inventory")]
    public Image[] weaponImages; // Array to hold weapon UI images

    float globalCD;
    public bool stunned = false;

    void Start()
    {
        // Initialize UI colors at the start
        InitializeWeaponUI();
    }

    void InitializeWeaponUI()
    {
        for (int i = 0; i < weaponImages.Length; i++)
        {
            weaponImages[i].color = i == currentWeaponNum ? Color.red : Color.white;
        }
    }

    void Update()
    {
        globalCD -= Time.deltaTime;

        // Update UI colors based on selected weapon
        UpdateWeaponUI();

        // Handle weapon selection
        HandleWeaponSelection();

        // Rest of your existing code...

        oldWeapon = currentWeaponNum;
    }

    void UpdateWeaponUI()
    {
        for (int i = 0; i < weaponImages.Length; i++)
        {
            weaponImages[i].color = i == currentWeaponNum ? Color.red : Color.white;
        }
    }

    void HandleWeaponSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && unlockedWeapon >= 0)
        {
            currentWeaponNum = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && unlockedWeapon >= 1)
        {
            currentWeaponNum = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && unlockedWeapon >= 2)
        {
            currentWeaponNum = 2;
        }

        switch (Input.mouseScrollDelta.y)
        {
            case > 0 when currentWeaponNum < unlockedWeapon:
                currentWeaponNum++;
                break;
            case > 0:
                currentWeaponNum = 0;
                break;
            case < 0 when currentWeaponNum > 0:
                currentWeaponNum--;
                break;
            case < 0:
                currentWeaponNum = unlockedWeapon;
                break;
        }

        currentWeapon = weapons[currentWeaponNum];
    }
}
