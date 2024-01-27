using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float maxDistance;
    public Transform parent;
    public float dmg;

    Vector2 starPos;

    void Start()
    {
        starPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(starPos, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != parent)
        {
            if (collision.gameObject.TryGetComponent(out Monkey monkey))
            {
                monkey.TakeDamage(dmg);
            }
            else if (collision.transform.parent.TryGetComponent(out Monkey monkeyParent))
            {
                monkeyParent.TakeDamage(dmg);
            }
            print("hit: " + collision);
            Destroy(gameObject);
        }
    }
}
