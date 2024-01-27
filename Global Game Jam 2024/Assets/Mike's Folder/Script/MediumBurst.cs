using UnityEngine;

public class MediumBurst : Weapon
{
    [SerializeField] GameObject prefab;
    ParticleSystem em;

    public override void Attack()
    {
        currentCD = maxCD;

        GameObject instance = Instantiate(prefab, transform.position, transform.rotation);

        em = instance.GetComponent<ParticleSystem>();
        em.Play();

        foreach (GameObject obj in FindObjectsOfType<GameObject>()) 
        {
            if (obj.TryGetComponent(out Monkey monkey))
            {
                if (monkey.transform != transform.parent && monkey.transform != transform)
                {
                    if (Vector2.Distance(monkey.transform.position, transform.position) < em.shape.length)
                    {
                        float value = Vector3.Angle(transform.forward, (monkey.transform.position - transform.position.normalized));

                        if (Mathf.Abs(value) < em.shape.angle * 1.1f)
                        {
                            monkey.TakeDamage(dmg);
                        }
                    }
                }
            }
            else if (obj.transform.parent.TryGetComponent(out Monkey monkeyParent))
            {
                if (monkeyParent.transform != transform.parent && monkeyParent.transform != transform)
                {
                    if (Vector2.Distance(monkeyParent.transform.position, transform.position) < em.shape.length)
                    {
                        float value = Vector3.Angle(transform.forward, (monkeyParent.transform.position - transform.position.normalized));

                        if (Mathf.Abs(value) < em.shape.angle * 1.1f)
                        {
                            monkeyParent.TakeDamage(dmg);
                        }
                    }
                }
            }
        }   
    }
}
