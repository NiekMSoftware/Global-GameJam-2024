using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LongRangeAttack : Weapon
{
    [SerializeField] GameObject attack;
    [SerializeField] float bulletSpeed;

    public override void Attack()
    {
        currentCD = maxCD;

        GameObject currentAttack = Instantiate(attack, transform.position + transform.forward, Quaternion.identity);
        if (currentAttack.TryGetComponent(out Rigidbody2D rb))
        {
            currentAttack.GetComponent<Projectile>().dmg = dmg;
            rb.AddForce(transform.forward * bulletSpeed, ForceMode2D.Impulse);
        }
    }
}
