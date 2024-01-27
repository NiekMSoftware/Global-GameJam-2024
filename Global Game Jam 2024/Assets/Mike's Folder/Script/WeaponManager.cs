using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Weapon[] weapons;
    [SerializeField] int currentWeaponNum;
    [SerializeField] int unlockedWeapon;
    [SerializeField] Weapon currentWeapon;
    [SerializeField] private Camera cam;

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
        
        Vector3 mousePosition = cam.ScreenToWorldPoint(
        new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.z));

        mousePosition.x -= transform.position.x;
        mousePosition.y -= transform.position.y;

        print(mousePosition);

        currentWeapon.currentCD -= Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(new Vector3(mousePosition.x, mousePosition.y, 0).normalized);

        if (Input.GetMouseButton(0) && currentWeapon.currentCD < 0)
        {

            currentWeapon.Attack();
        }
    }
}
