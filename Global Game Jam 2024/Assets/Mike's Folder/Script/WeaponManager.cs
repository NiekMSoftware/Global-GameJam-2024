using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Weapon[] weapons;
    [SerializeField] int currentWeaponNum;
    [SerializeField] int unlockedWeapon;
    [SerializeField] Weapon currentWeapon;

    void Update()
    {
         if(Input.GetKeyDown(KeyCode.Alpha1) && unlockedWeapon >= 0)
        {
            currentWeaponNum = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && unlockedWeapon  >= 1)
        {
            currentWeaponNum = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && unlockedWeapon >= 2)
        {
            currentWeaponNum = 2;
        }

        if (Input.mouseScrollDelta.y > 0 && currentWeaponNum < unlockedWeapon) 
        {
            currentWeaponNum++;
        }
        else if (Input.mouseScrollDelta.y > 0) currentWeaponNum = 0;

        if (Input.mouseScrollDelta.y < 0 && currentWeaponNum > 0)
        {
            currentWeaponNum--;
        }
        else if (Input.mouseScrollDelta.y < 0) currentWeaponNum = unlockedWeapon;

        currentWeapon = weapons[currentWeaponNum];

        currentWeapon.currentCD -= Time.deltaTime;

        if (Input.GetMouseButton(0) && currentWeapon.currentCD < 0)
        {
            currentWeapon.Attack();
        }
    }
}
