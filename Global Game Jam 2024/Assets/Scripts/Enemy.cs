using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Monkey
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Weapon weapon;
    [SerializeField] private float stoppingDistance;
    private float startSpeed;
    [SerializeField] private float standstillCooldown = 1f;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject barBackground;
    [SerializeField] private Sprite deadSprite;
    private bool isDead = false;
    Vector3 healthBarFullSize;

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
    }

    private void Update()
    {
        if (!isDead)
        {
            agent.SetDestination(player.position);

            weapon.currentCD -= Time.deltaTime;
            healthBar.transform.position = new Vector3(transform.position.x - ((healthBarFullSize.x - healthBar.transform.localScale.x) / 2), transform.position.y + 0.5f, +1.472f);
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
        healthBar.transform.localScale = new Vector3(((healthBarFullSize.x/maxHealth) * health), healthBarFullSize.y, 1);
        barPosMover = (1 / maxHealth) * health;
        Debug.Log(barPosMover);
        
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
        Destroy(gameObject.transform.parent.GetChild(1).gameObject);
        Destroy(gameObject.transform.parent.GetChild(2).gameObject);
        isDead = true;
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = deadSprite;
        Debug.Log("Enemy Died");
    }
}
