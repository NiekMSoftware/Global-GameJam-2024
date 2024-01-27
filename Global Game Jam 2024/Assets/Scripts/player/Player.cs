using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] ParticleSystem dust;


    private Vector2 playerDirection;
    
    private bool pressed;
    private bool isOnCooldown = false;

    [SerializeField] private Transform cursor;
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        monkeyRb = GetComponent<Rigidbody2D>();

        if (healthSlider == null)
        {
            healthSlider = GameObject.Find("PlayerHealth").GetComponent<Slider>();
        }
        Health = MaxHealth;
        healthSlider.maxValue = MaxHealth;
        healthSlider.value = Health;
        monkeyRb.drag = 5f;
        cursor = transform.GetChild(0);
    }

    private void FixedUpdate()
    {
        GetComponent<SpriteRenderer>().flipX = (cursor.rotation.y < 0) ? true : false;

        if (playerDirection != Vector2.zero)
        {
            CreateDust();
            animator.Play("Running");

            // Apply force in the specified direction
            monkeyRb.AddForce(playerDirection * speed);
        }
        else
        {
            animator.Play("Idle");
        }

        monkeyRb.velocity = Vector2.ClampMagnitude(monkeyRb.velocity, speed);
        
        if (pressed && !isOnCooldown && monkeyRb.velocity.magnitude > 0)
        {
            Dodge();
        }
    }
    protected override void OnTakeDamage()
    {
        healthSlider.value = Health;
    }
    protected override void Dodge()
    {
        if (pressed && !isOnCooldown)
        {
            print("YEEt");
            monkeyRb.AddForce(playerDirection * dodgeForce, ForceMode2D.Impulse);

            isOnCooldown = true;
            StartCoroutine(DodgeCooldown());
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        playerDirection = ctx.ReadValue<Vector2>();
    }

    public void OnDodge(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() > 0 && playerDirection.magnitude > 0)
        {
            pressed = true;
        }
    }

    IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(timeUntilNext);
        isOnCooldown = false;
        pressed = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BossAttack"))
        {
            if (collision.TryGetComponent(out Rock rock))
                TakeDamage(rock.GetDamage());
            else if (collision.TryGetComponent(out FireRing fireRing))
                TakeDamage(fireRing.GetDamage());
        }
    }
    public void CreateDust()
    {
        dust.Play();
    }
}