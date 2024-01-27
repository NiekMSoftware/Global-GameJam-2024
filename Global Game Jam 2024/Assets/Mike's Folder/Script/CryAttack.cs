using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryAttack : Weapon
{
    [SerializeField] LineRenderer lineRenderer;
    RaycastHit2D hit;

    public override void Attack()
    {
        currentCD = maxCD;

        hit = Physics2D.Raycast(transform.position, transform.forward);

        if (hit.collider != null)
        {
            if (hit.transform.TryGetComponent(out Monkey monkey)) 
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);

                lineRenderer.enabled = true;
                monkey.TakeDamage(dmg);
            }
        }

        Invoke("TurnOffLineRenderer", 1f);
    }

    void TurnOffLineRenderer()
    {
        lineRenderer.enabled = false;
    }
}
