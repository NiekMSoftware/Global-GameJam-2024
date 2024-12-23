using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float maxDistance;
    public Transform parent;
    public float dmg;
    public bool isEnemy;

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
            if (isEnemy)
            {
                if (collision.gameObject.TryGetComponent(out Player player))
                {
                    player.TakeDamage(dmg);
                }
                else if (collision.transform.parent != null)
                {
                    if (collision.transform.parent.TryGetComponent(out Player playerParent))
                    {
                        playerParent.TakeDamage(dmg);
                    }
                }
            }
            else
            {
                if (collision.gameObject.TryGetComponent(out Monkey monkey))
                {
                    Debug.Log("Monkey take damage");
                    monkey.TakeDamage(dmg);
                }
                else if (collision.transform.parent != null)
                {
                    if (collision.transform.parent.TryGetComponent(out Monkey monkeyParent))
                    {
                        monkeyParent.TakeDamage(dmg);
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
