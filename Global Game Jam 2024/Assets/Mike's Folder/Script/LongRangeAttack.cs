using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LongRangeAttack : Weapon
{
    [SerializeField] GameObject attack;
    [SerializeField] float bulletSpeed;
    [SerializeField] Animator anim;
    [SerializeField] AnimationClip haha;

    public override void Attack()
    {
        currentCD = maxCD;
        anim.Play("haah");
        GameObject currentAttack = Instantiate(attack, transform.position + transform.forward, Quaternion.identity);
        if (currentAttack.TryGetComponent(out Rigidbody2D rb))
        {
            currentAttack.GetComponent<Projectile>().parent = transform.parent;
            currentAttack.GetComponent<Projectile>().dmg = dmg;
            rb.AddForce(transform.forward * bulletSpeed, ForceMode2D.Impulse);
        }
    }
}
