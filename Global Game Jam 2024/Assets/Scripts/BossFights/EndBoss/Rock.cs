using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Rock : MonoBehaviour
{
    [SerializeField] private GameObject circle;
    [SerializeField] private float teleGraphTime;
    [SerializeField] private float destroyTime;

    private SpriteRenderer spriteRenderer;

    private float teleGraphTimer;
    private float destroyTimer;

    private States state;

    private enum States
    {
        TeleGraph,
        Grounded
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        state = States.TeleGraph;

        teleGraphTimer = teleGraphTime;

        destroyTimer = destroyTime;
    }

    private void Update()
    {
        switch (state)
        {
            case States.TeleGraph:
                spriteRenderer.enabled = false;
                circle.SetActive(true);

                teleGraphTimer -= Time.deltaTime;

                if (teleGraphTimer <= 0)
                {
                    state = States.Grounded;
                }
                break;

            case States.Grounded:
                circle.SetActive(false);
                spriteRenderer.enabled = true;

                destroyTimer -= Time.deltaTime;

                if (destroyTimer <= 0)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }
}
