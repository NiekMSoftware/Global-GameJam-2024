using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Monkey
{
    [Header("Dodging Properties")]
    [SerializeField] private float dodgeForce;
    [SerializeField] private float timeUntilNext;
    
    private bool pressed;
    private bool isOnCoolDown = false;
    
    private void Start()
    {
        InitPlayer();
    }

    private void InitPlayer()
    {
        health = maxHealth;
        
        monkeyRb = GetComponent<Rigidbody2D>();
        monkeyRb.drag = 5f;
    }

    private void Update()
    {
        pressed = Input.GetKeyDown(KeyCode.Space);
        
        // check if the key has been pressed
        if (pressed && !isOnCoolDown)
        {
            Dodge();

            isOnCoolDown = true;
            StartCoroutine(DodgeCooldown());
        }
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
        // Add force to dodge the player
        Vector2 direction = Move();
        
        // thrust player to position
        monkeyRb.AddForce(direction * dodgeForce);
    }

    protected override Vector2 Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(x, y);

        if (direction != Vector2.zero)
        {
            float angle = GetRotationAngle(direction);
            monkeyRb.rotation = angle;
        }

        return direction;
    }

    private float GetRotationAngle(Vector2 direction)
    {
        float angle = 0;

        if (Math.Abs(direction.x) > Math.Abs(direction.y))
        {
            // if we primarily move horizontally
            angle = (direction.x > 0) ? -90 : 90;
        }
        else
        {
            // if we primarily move vertically
            angle = (direction.y > 0) ? 0 : 180;
        }
        
        return angle;
    }

    private IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(timeUntilNext);
        
        isOnCoolDown = false;
    }
}
