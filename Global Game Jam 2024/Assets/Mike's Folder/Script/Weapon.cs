using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float dmg;
    [SerializeField] protected float maxCD;
    public float currentCD;
    [SerializeField] protected float happieness;

    public virtual void Attack() { }
}
