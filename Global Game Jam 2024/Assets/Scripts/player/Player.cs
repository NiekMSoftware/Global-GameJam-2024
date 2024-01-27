using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Monkey
{
    [Header("Dodging Properties")]
    [SerializeField] private float dodgeForce;
    [SerializeField] private float timeUntilNext;
    public AnimationClip Running;
    public AnimationClip Idle;
    [SerializeField] Animator animator;

    [SerializeField] private Transform cursor;
    public PlayerController playerController;
    private InputAction moveAction;
    private InputAction dodgeAction;
    
    private bool pressed;
    private bool isOnCoolDown = false;

    // move direction
    private Vector2 direction;
    
    private void Awake()
    {
        animator = GetComponent <Animator>();
        playerController = new PlayerController();
    }

    private void OnEnable()
    {
        playerController.gameplay.move.performed += OnMovePerformed;
        playerController.gameplay.move.canceled += OnMoveCanceled;
        playerController.gameplay.dodge.performed += OnDodgePerformed;
        
        playerController.Enable();
    }

    private void OnDisable()
    {
        playerController.Disable();
        
        playerController.gameplay.move.performed -= OnMovePerformed;
        playerController.gameplay.move.canceled -= OnMoveCanceled;
        playerController.gameplay.dodge.performed -= OnDodgePerformed;
    }

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
        pressed = playerController.gameplay.dodge.triggered;
    }

    private void FixedUpdate()
    {
        // Check if the key has been pressed, the player is not already in cooldown,
        // and the player has a movement direction (only in FixedUpdate)
        if (pressed && !isOnCoolDown && direction.magnitude > 0)
        {
            Dodge();
            StartCoroutine(DodgeCooldown());
        }

        float YRotation = (cursor.rotation.y < 0) ? 180f : 0f;
        transform.rotation = Quaternion.Euler(0f, YRotation, 0f);
        
        // Only apply the move force when not in cooldown.
        if (!isOnCoolDown)
        {
            // If there is input direction, apply force
            if (direction != Vector2.zero)
            {
                animator.Play("Running");
                
                // Apply force in the specified direction
                monkeyRb.AddForce(direction * speed);
            }
            else
            {
                animator.Play("Idle");
            }
        }
    }

    protected override void Dodge()
    {
        // thrust player to position
        monkeyRb.AddForce(direction * dodgeForce, ForceMode2D.Impulse);
        isOnCoolDown = true;
    }

    private IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(timeUntilNext);
        
        isOnCoolDown = false;
    }
    
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        direction = Vector2.zero;
    }

    private void OnDodgePerformed(InputAction.CallbackContext context)
    {
        if (!isOnCoolDown && direction.magnitude > 0)
        {
            Dodge();
            StartCoroutine(DodgeCooldown());
        }
    }
}
