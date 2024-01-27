using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAttack : Weapon
{
    [SerializeField] Transform body;
    [SerializeField] GameObject attack;
    [SerializeField] float bulletSpeed;
    [SerializeField] bool isEnemy;

    public override void Attack()
    {
        currentCD = maxCD;

        GameObject currentAttack = Instantiate(attack, transform.position + transform.forward, Quaternion.identity);

        if (currentAttack.TryGetComponent(out Rigidbody2D rb))
        {
            currentAttack.GetComponent<Projectile>().isEnemy = isEnemy;
            currentAttack.GetComponent<Projectile>().parent = (body == null) ? transform.parent : body;
            currentAttack.GetComponent<Projectile>().dmg = dmg;
            rb.AddForce(transform.forward * bulletSpeed, ForceMode2D.Impulse);
        }
    }
}
