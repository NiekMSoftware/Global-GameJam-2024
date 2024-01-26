using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Monkey
{
    private void Start()
    {
        monkeyRb = GetComponent<Rigidbody2D>();
        monkeyRb.drag = 5f;
    }

    private void FixedUpdate()
    {
        // Get direction from player input
        Vector2 direction = Move();
        
        // If direction is not zero (player is pressing keys), add force to move
        monkeyRb.AddForce(direction * speed);
        
        // Clamp the velocity to make sure the player doesn't go too fast
        monkeyRb.velocity = Vector2.ClampMagnitude(monkeyRb.velocity, speed);
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
