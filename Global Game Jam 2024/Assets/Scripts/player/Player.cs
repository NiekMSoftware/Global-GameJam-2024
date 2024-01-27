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
        monkeyRb.AddForce(playerDirection * speed);
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