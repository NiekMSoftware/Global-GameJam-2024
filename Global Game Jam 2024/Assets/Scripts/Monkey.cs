using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    [Header("Base Monkey Properties")]
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float speed;

    [Space]
    [SerializeField] protected Rigidbody2D monkeyRb;

    public float Health {
        get {
            return health;
        }
        set {
            health = value;
        }
    }
    public float MaxHealth {
        get {
            return maxHealth;
        }
        set {
            maxHealth = value;
        }
    }
    public float Speed {
        get {
            return speed;
        }
        set {
            speed = value;
        }
    }

    protected virtual void Attack()
    {
        
    }

    protected virtual void Dodge()
    {
        
    }

    protected virtual Vector2 Move()
    {
        return Vector2.zero;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if(Health < 0)
        {
            Die();
        }
        if(Health > maxHealth)
        {
            Health = maxHealth;
        }
    }

    protected virtual void Die()
    {

    }
}
