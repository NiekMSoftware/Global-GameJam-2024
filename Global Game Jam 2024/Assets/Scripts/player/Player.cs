using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Monkey
{
    private void Update()
    {
        Vector2 movement = Move();
        transform.Translate(movement * (Speed * Time.deltaTime));
    }
    
    protected override void Attack()
    {
        base.Attack();
    }

    protected override void Dodge()
    {
        base.Dodge();
    }

    protected override Vector2 Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        return new Vector2(x, y);
    }
}
