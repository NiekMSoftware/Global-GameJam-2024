using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Monkey
{
    [Header("Dodge Properties")]
    [SerializeField] private float dodgeForce;
    [SerializeField] private float timeUntilNext;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip Idle;
    [SerializeField] AnimationClip Walking;

    [SerializeField] Transform cursor;
    private Vector2 playerDirection;
    
    private bool pressed;
    private bool isOnCooldown = false;
    
    private void Awake()
    {
        monkeyRb = GetComponent<Rigidbody2D>();

        monkeyRb.drag = 5f;
    }

    private void FixedUpdate()
    {
        GetComponent<SpriteRenderer>().flipX = (cursor.rotation.y < 0) ? true : false;

        if (playerDirection != Vector2.zero)
        {
            animator.Play("Running");

            // Apply force in the specified direction
            monkeyRb.AddForce(playerDirection * speed);
        }
        else
        {
            animator.Play("Idle");
        }

        monkeyRb.velocity = Vector2.ClampMagnitude(monkeyRb.velocity, speed);
        
        if (pressed && !isOnCooldown && transform.position.magnitude > 0)
        {
            Dodge();
            pressed = false;
        }
    }

    protected override void Dodge()
    {
        monkeyRb.AddForce(playerDirection * dodgeForce, ForceMode2D.Impulse);
        
        isOnCooldown = true;
        StartCoroutine(DodgeCooldown());
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        playerDirection = ctx.ReadValue<Vector2>();
    }

    public void OnDodge(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() > 0 && !isOnCooldown && transform.position.magnitude > 0)
        {
            pressed = true;
        }
    }

    IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(timeUntilNext);
        isOnCooldown = false;
    }
}