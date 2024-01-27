using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float dmg;
    public float maxCD;
    public float currentCD;
    [SerializeField] protected float happieness;
    public float attackDuration;
    [SerializeField] protected AudioSource audio;
    [SerializeField] protected AudioClip[] audioclips;

    public virtual void Attack() { }
}
