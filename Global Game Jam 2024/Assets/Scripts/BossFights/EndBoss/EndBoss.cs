using System.Collections;
using UnityEngine;

public class EndBoss : Enemy
{
    [SerializeField] private GameObject fireRing;
    [SerializeField] private GameObject rock;
    [SerializeField] private float coolDownTime;
    [SerializeField] private float squareSize;
    [SerializeField] private Vector3 squareOffset;
    [SerializeField] private float stunTime;
    [SerializeField] private Color stunColor;
    [SerializeField] private int amountOfAttacksToStun;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private int amountOfAttacks = 0;

    private float coolDownTimer;

    private float stunTimer;

    private Color startingColor;

    [SerializeField] private AttackInfo[] attacks;

    [SerializeField] private States state;

    private enum States
    {
        Idle,
        Attacking,
        Stunned
    }

    [System.Serializable]
    private struct AttackInfo
    {
        public Vector2 randomAmountRange;
        public float attackCoolDownTime;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        coolDownTimer = coolDownTime;
        stunTimer = stunTime;

        startingColor = spriteRenderer.color;
    }

    private void Update()
    {
        spriteRenderer.color = startingColor;

        if (state == States.Stunned)
        {
            amountOfAttacks = 0;
            spriteRenderer.color = stunColor;
            stunTimer -= Time.deltaTime;

            if (stunTimer <= 0)
            {
                stunTimer = stunTime;
                state = States.Idle;
            }
        }

        if (state != States.Idle) return;

        coolDownTimer -= Time.deltaTime;

        if (coolDownTimer <= 0)
        {
            coolDownTimer = coolDownTime;
            Attack();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + squareOffset, new Vector2(-squareSize, squareSize));
    }

    protected override void Attack()
    {
        state = States.Attacking;
        amountOfAttacks++;

        if (amountOfAttacks >= amountOfAttacksToStun)
        {
            state = States.Stunned;
            return;
        }

        int randomAttackNum = Random.Range(0, attacks.Length);

        AttackInfo attackInfo = attacks[randomAttackNum];

        float randomAmount = Random.Range(attackInfo.randomAmountRange.x, attackInfo.randomAmountRange.y);

        CancelInvoke();

        switch (randomAttackNum)
        {
            case 0:
                StartCoroutine(Attack(ThumpAttack, (int)Mathf.Ceil(randomAmount), attackInfo));
                break;

            case 1:
                StartCoroutine(Attack(RockAttack, (int)Mathf.Ceil(randomAmount), attackInfo));
                break;
        }   
    }

    private IEnumerator Attack(System.Action action, int amount, AttackInfo attackInfo)
    {
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(attackInfo.attackCoolDownTime);

            action.Invoke();
            state = States.Idle;
        }
    }

    private void ThumpAttack()
    {
        Instantiate(fireRing, transform.position, Quaternion.identity);
    }

    private void RockAttack()
    {
        Vector3 randomPosition = transform.localPosition + squareOffset;
        randomPosition.x += Random.Range(-squareSize / 2, squareSize / 2);
        randomPosition.y += Random.Range(-squareSize / 2, squareSize / 2);
        randomPosition.z = 0;

        Instantiate(rock, randomPosition, Quaternion.identity);
    }

    protected override void Die()
    {
        Time.timeScale = 0f;
        print("You defeated the boss!!");
    }

    public override void TakeDamage(float damage)
    {
        if (state == States.Stunned)
            base.TakeDamage(damage);
    }
}
