using System.Collections;
using UnityEngine;

public class EndBoss : Enemy
{
    [SerializeField] private GameObject fireRing;
    [SerializeField] private GameObject rock;
    [SerializeField] private float coolDownTime;
    [SerializeField] private float squareSize;
    [SerializeField] private Vector3 squareOffset;

    private float coolDownTimer;

    [SerializeField] private AttackInfo[] attacks;

    [System.Serializable]
    private struct AttackInfo
    {
        public Vector2 randomAmountRange;
        public float attackCoolDownTime;
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
}
