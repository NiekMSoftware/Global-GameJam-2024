using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Monkey
{
    [Header("Dodging Properties")]
    [SerializeField] private float dodgeForce;
    [SerializeField] private float timeUntilNext;
    
    [Header("Animator")]
    public AnimationClip Running;
    public AnimationClip Idle;
    [SerializeField] Animator animator;
    
    [Space]
    [SerializeField] private Transform cursor;
    private PlayerController playerController;
    private InputAction moveAction;
    private InputAction dodgeAction;
    private InputAction cursorAction;
    
    private bool pressed;
    private bool isOnCoolDown = false;

    // move direction
    private Vector2 direction;
    private Vector2 cursorDirection;
    
    private void Awake()
    {
        animator = GetComponent <Animator>();
        playerController = new PlayerController();
        
        cursorAction = playerController.gameplay.cursor;
    }

    private void OnEnable()
    {
        playerController.gameplay.move.performed += OnMovePerformed;
        playerController.gameplay.move.canceled += OnMoveCanceled;
        playerController.gameplay.dodge.performed += OnDodgePerformed;

        cursorAction.performed += OnCursorMovedPerformed;
        cursorAction.canceled += OnCursorMovedCanceled;
        
        playerController.Enable();
    }

    private void OnDisable()
    {
        playerController.Disable();
        
        playerController.gameplay.move.performed -= OnMovePerformed;
        playerController.gameplay.move.canceled -= OnMoveCanceled;
        playerController.gameplay.dodge.performed -= OnDodgePerformed;
        
        cursorAction.performed -= OnCursorMovedPerformed;
        cursorAction.canceled -= OnCursorMovedCanceled;
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

    public void OnCursorMovedPerformed(InputAction.CallbackContext context)
    {
        cursorDirection = context.ReadValue<Vector2>();

        Vector2 lookPos =
        Camera.main.ScreenToWorldPoint(
        new Vector3(cursorDirection.x, cursorDirection.y, Camera.main.transform.position.z));
        
        lookPos.x -= transform.position.x;
        lookPos.y -= transform.position.y;
        
         cursor.rotation = Quaternion.LookRotation(new Vector3(lookPos.x, lookPos.y, 0));
    }

    private void OnCursorMovedCanceled(InputAction.CallbackContext context)
    {
        cursorDirection = Vector3.zero;
    }
}