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

    private States state;

    private enum States
    {
        Idle,
        Attacking
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        agent.SetDestination(player.position);

        CalculateDestination();

        HandleStates();

        HandleAttacking();
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

    private void HandleStates()
    {
        if (Vector3.Distance(player.position, transform.position) < stoppingDistance)
        {
             state = States.Attacking;
        }
        else
        {
            state = States.Idle;
        }
    }

    protected override void Attack()
    {
        weapon.currentCD -= Time.deltaTime;
        
        if (weapon.currentCD < 0)
            weapon.Attack();
    }

    protected override Vector2 Move()
    {
        return Vector2.zero;
    }
    protected override void Die()
    {
        Destroy(gameObject);
        Debug.Log("Enemy Died");
    }
}
