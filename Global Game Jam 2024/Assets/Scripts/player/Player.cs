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

    public PlayerController playerController;
    private InputAction moveAction;
    private InputAction dodgeAction;
    
    private bool pressed;
    private bool isOnCoolDown = false;

    // move direction
    private Vector2 direction;
    
    private void Awake()
    {
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

        // Only apply the move force when not in cooldown.
        if (!isOnCoolDown)
        {
            // Calculate the rotation angle based on the current move direction
            float rotationAngle = GetRotationAngle(direction);

            // Apply the rotation to your player character
            monkeyRb.rotation = rotationAngle;
        
            // If there is input direction, apply force
            if (direction != Vector2.zero)
            {
                monkeyRb.AddForce(direction * speed);
            }
        }
    }

    protected override void Dodge()
    {
        // thrust player to position
        monkeyRb.AddForce(direction * dodgeForce, ForceMode2D.Impulse);
        isOnCoolDown = true;
    }

    private float GetRotationAngle(Vector2 dir)
    {
        float angle = 0;

        if (Math.Abs(dir.x) > Math.Abs(dir.y))
        {
            // if we primarily move horizontally
            angle = (dir.x > 0) ? -90 : 90;
        }
        else
        {
            // if we primarily move vertically
            angle = (dir.y > 0) ? 0 : 180;
        }
        
        return angle;
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
