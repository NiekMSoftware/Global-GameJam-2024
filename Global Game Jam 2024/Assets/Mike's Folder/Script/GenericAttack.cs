using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAttack : Weapon
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
            rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
        }
    }
}
