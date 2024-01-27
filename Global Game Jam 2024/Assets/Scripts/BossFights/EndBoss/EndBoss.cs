using UnityEngine;

public class EndBoss : Enemy
{
    [SerializeField] private float coolDownTime;

    private float coolDownTimer;

    private enum Attacks
    {
        Thump
    }

    private void Start()
    {
        coolDownTimer = coolDownTime;
    }

    private void Update()
    {
        coolDownTimer -= Time.deltaTime;

        if (coolDownTimer <= 0)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        System.Array attackValues = System.Enum.GetValues(typeof(Attacks));

        Attacks randomAttack = (Attacks)attackValues.GetValue(Random.Range(0, attackValues.Length));

        switch (randomAttack)
        {
            case Attacks.Thump:

                break;
        }
        
    }
}
