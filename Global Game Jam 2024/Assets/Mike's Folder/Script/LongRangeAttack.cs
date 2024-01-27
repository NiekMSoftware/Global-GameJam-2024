using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LongRangeAttack : Weapon
{
    [SerializeField] GameObject attack;
    [SerializeField] float bulletSpeed;
    [SerializeField] bool isEnemy;

    public override void Attack()
    {
        currentCD = maxCD;

        audio.clip = audioclips[0];

        audio.Play();

        GameObject currentAttack = Instantiate(attack, transform.position + transform.forward, Quaternion.identity);
        if (currentAttack.TryGetComponent(out Rigidbody2D rb))
        {
            currentAttack.GetComponent<Projectile>().isEnemy = isEnemy;
            currentAttack.GetComponent<Projectile>().parent = transform.parent;
            currentAttack.GetComponent<Projectile>().dmg = dmg;
            rb.AddForce(transform.forward * bulletSpeed, ForceMode2D.Impulse);
        }
    }
}
