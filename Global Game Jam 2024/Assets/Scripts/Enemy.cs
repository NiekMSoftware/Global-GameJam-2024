using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Monkey
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] protected Transform player;
    [SerializeField] private Weapon weapon;
    [SerializeField] private float stoppingDistance;
    private float startSpeed;
    [SerializeField] private float standstillCooldown = 1f;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject barBackground;
    [SerializeField] private Sprite deadSprite;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] protected GameObject laughParticle;

    public Room room;

    private MissionManager missionManager;

    public bool isDead = false;
    Vector3 healthBarFullSize;

    [SerializeField] private Animator animator;


    float barPosMover;

    private States state;

    private enum States
    {
        Idle,
        Attacking
    }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>().transform;
        startSpeed = agent.speed;

        if (healthBar == null)
        {
            healthBar = transform.parent.GetChild(1).gameObject;
            barBackground = transform.parent.GetChild(2).gameObject;
        }
        healthBarFullSize = healthBar.transform.localScale;
        barBackground.transform.localScale = healthBarFullSize;

        missionManager = FindObjectOfType<MissionManager>();

        laughParticle = transform.GetChild(0).gameObject;
        laughParticle.SetActive(false);
    }

    private void Update()
    {
        if (!isDead)
        {
            agent.SetDestination(player.position);

            weapon.currentCD -= Time.deltaTime;
            healthBar.transform.position = new Vector3(transform.position.x - ((healthBarFullSize.x - healthBar.transform.localScale.x) / 2), transform.position.y + 0.5f, +1.472f);
            barBackground.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, +1.472f);


            animator.gameObject.transform.position = transform.position;

        if (agent.hasPath)
        {
            animator.Play("Walking Animation");
        }
        else
        {
            animator.Play("Idle Animation");
        }

        weapon.currentCD -= Time.deltaTime;
        healthBar.transform.position = new Vector3(transform.position.x - ((1 - healthBar.transform.localScale.x)/2), transform.position.y+0.5f, +1.472f);
        barBackground.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, +1.472f);

            CalculateDestination();

            HandleStates();

            HandleAttacking();
        }
        
    }

    private void HandleAttacking()
    {
        if (state != States.Attacking) return;

        Attack();
    }

    private void CalculateDestination()
    {
        if (state == States.Attacking)
            agent.SetDestination(transform.position);
        else if (state == States.Idle)
            agent.SetDestination(player.position);
    }

    protected override void OnTakeDamage()
    {
        if (!isDead)
        {
            healthBar.transform.localScale = new Vector3(((healthBarFullSize.x / maxHealth) * health), healthBarFullSize.y, 1);
            barPosMover = (1 / maxHealth) * health;
        }
        
    }

    private void HandleStates()
    {
        if (Vector3.Distance(player.position, transform.position) < stoppingDistance)
        {
             state = States.Attacking;
        }
        else
        {
            state = States.Idle;
            //Makes the enemy actually move if the player is far away and isnt attacking
            if (weapon.currentCD < weapon.maxCD - weapon.attackDuration)
            {
                agent.speed = startSpeed;
            }
            //Makes the enemy not instantly attack you when it stands still
            if (weapon.currentCD < standstillCooldown)
            {
                weapon.currentCD = standstillCooldown;
            }
        }
    }

    protected override void Attack()
    {
        if(weapon.currentCD < weapon.maxCD - weapon.attackDuration)
        {
            //Rotates the enemy towards the player if it isnt attacking
            Vector3 dir = player.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 5);
            agent.speed = startSpeed;
        }
        else
        {
            //Sets the enemys speed to zero if it is attacking
            agent.speed = 0;
        }

        if (weapon.currentCD < 0)
        {
            weapon.Attack();
        }
    }

    protected override void Die()
    {
        if (!isDead)
        {
            Destroy(gameObject.transform.parent.GetChild(1).gameObject);
            Destroy(gameObject.transform.parent.GetChild(2).gameObject);
            gameObject.GetComponent<Collider2D>().enabled = false;
            isDead = true;
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            renderer.sprite = deadSprite;
            missionManager.CuredEnemy(this);
            room?.RemoveEnemy(this);
            animator.SetBool("isDead", true);
            laughParticle.SetActive(true);
            Debug.Log("Enemy Died");
        }
    }
}
