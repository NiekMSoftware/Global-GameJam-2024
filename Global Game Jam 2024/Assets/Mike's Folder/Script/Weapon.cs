using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float dmg;
    public float maxCD;
    public float currentCD;
    [SerializeField] public float happieness;
    public float attackDuration;

    public virtual void Attack() { }
}
