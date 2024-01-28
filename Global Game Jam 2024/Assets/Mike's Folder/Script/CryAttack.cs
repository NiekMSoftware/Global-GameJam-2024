using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryAttack : Weapon
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] LayerMask layer;
    RaycastHit2D hit;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip crying;

    public override void Attack()
    {
        currentCD = maxCD;

        audio.clip = audioclips[0];

        audio.Play();

        Invoke("UseAttack", audioclips[0].length);

    }

    void UseAttack()
    {
        audio.clip = audioclips[1];

        audio.Play();

        hit = Physics2D.Raycast(transform.position + transform.forward, transform.forward, Mathf.Infinity, layer);
        animator.Play("Crying");
        Debug.Log(animator);
        if (hit.collider != null)
        {
            print(hit.collider.name);
            if (hit.transform.TryGetComponent(out Monkey monkey))
            {
                monkey.TakeDamage(dmg);
            }
            else if (hit.transform.parent != null)
            {
                if (hit.transform.TryGetComponent(out Monkey monkeyParent))
                {
                    monkeyParent.TakeDamage(dmg);
                }
            }
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);

            lineRenderer.enabled = true;
        }

        Invoke("TurnOffLineRenderer", 1f);
    }

    void TurnOffLineRenderer()
    {
        lineRenderer.enabled = false;
    }
}
