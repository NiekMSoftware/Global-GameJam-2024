using System.Collections;
using UnityEditor;
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
    [SerializeField] AnimationClip dodge;
    [SerializeField] SpriteRenderer banana;

    [SerializeField] ParticleSystem dust;
    [SerializeField] AudioSource audio;
    float audioCd;

    float dodgeDuration;
    

    //private bool immune = false;
    public bool immune = false;
    public bool stunned = false;

    private Vector2 playerDirection;
    
    private bool pressed;
    private bool isOnCooldown = false;

    [SerializeField] private Transform cursor;
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
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

    private void Update()
    {
        if (!pressed)
        {
            playerDirection = Move();
            animator.ResetTrigger("Dodge");
            animator.SetTrigger("Run");
        }
        else
        {
            if (dodgeDuration > 0)
            {
                animator.ResetTrigger("Run");
                animator.SetTrigger("Dodge");
            }
            if (dodgeDuration <= 0.3)
            {
                playerDirection = Vector2.zero;
            }
        }

    }

    private void FixedUpdate()
    {
        GetComponent<SpriteRenderer>().flipX = (cursor.rotation.y < 0) ? true : false;
        banana.flipX = (cursor.rotation.y < 0) ? true : false;

        audioCd -= Time.deltaTime;
        dodgeDuration -= Time.deltaTime;
      //  if (playerDirection != Vector2.zero)


        if (playerDirection != Vector2.zero && !stunned)
        {
            CreateDust();

            if (dodgeDuration <= 0)
            {
                animator.ResetTrigger("Dodge");
                animator.SetTrigger("Run");
            }

            if (audioCd < 0)
            {
                audio.Play();
                audioCd = audio.clip.length;
            }
            // Apply force in the specified direction
            monkeyRb.AddForce(playerDirection * speed);
        }
        else if (dodgeDuration <= 0)
        {
            animator.Play("Idle");
                audio.Stop();
            
        }

        if (!stunned)
            monkeyRb.velocity = Vector2.ClampMagnitude(monkeyRb.velocity, speed);
        
        if (pressed && !isOnCooldown && monkeyRb.velocity.magnitude > 0 && !stunned)
        {
            Dodge();
        }
    }

    protected override Vector2 Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        return new Vector2(x, y);
    }

    protected override void OnTakeDamage()
    {
        healthSlider.value = Health;
    }
    protected override void Dodge()
    {
        if (pressed && !isOnCooldown)
        {
            dodgeDuration = dodge.length;
            print("YEEt");
            monkeyRb.AddForce(playerDirection * dodgeForce, ForceMode2D.Impulse);

            isOnCooldown = true;
            immune = true;
            StartCoroutine(DodgeCooldown());
        }
    }

    //public void OnMove(InputAction.CallbackContext ctx)
    //{
    //    if (!isDodging)
    //        playerDirection = ctx.ReadValue<Vector2>();
    //    else playerDirection = Vector2.zero;
    //}

    public void OnDodge(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() > 0 && playerDirection.magnitude > 0 && dodgeDuration < 0)
        {
            animator.StopPlayback();
            dodgeDuration = dodge.length;
            pressed = true;
        }
    }

    IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(timeUntilNext);
        isOnCooldown = false;
        pressed = false;
        immune = false;
    }

    public override void TakeDamage(float damage)
    {
        if (!immune)
        {
            print("take damage");
            base.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BossAttack"))
        {
            if (collision.TryGetComponent(out FireRing fireRing))
                TakeDamage(fireRing.GetDamage());
            else if (collision.transform.TryGetComponent(out Rock rock))
                TakeDamage(rock.GetDamage());
        }
    }
    public void CreateDust()
    {
        dust.Play();
    }
}