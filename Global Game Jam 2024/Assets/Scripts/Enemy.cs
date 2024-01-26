using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Monkey
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Attack()
    {

    }

    protected override Vector2 Move()
    {
        return Vector2.zero;
    }
    protected override void Die()
    {
        Destroy(gameObject);
        Debug.Log("Enemy Died");
    }
}
