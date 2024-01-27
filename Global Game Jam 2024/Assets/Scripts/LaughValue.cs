using UnityEngine;

public class LaughValue : MonoBehaviour
{
    [SerializeField] private float laughValue;
    [SerializeField] private float maxLaughValue;
    [SerializeField] private float laughDecreaseAmountPerSecond;
    [SerializeField] private float stunTime;

    private float stunTimer;

    private States state;

    public enum States
    {
        Stunned,
        Idle
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            laughValue += 3;
        }

        switch (state)
        {
            case States.Idle:
                laughValue -= Time.deltaTime * laughDecreaseAmountPerSecond;
                laughValue = Mathf.Clamp(laughValue, 0, maxLaughValue);

                if (laughValue >= maxLaughValue)
                {
                    state = States.Stunned;
                    laughValue = maxLaughValue;
                    print("stun enemy");
                }
                break;

            case States.Stunned:
                stunTimer -= Time.deltaTime;

                if (stunTimer <= 0)
                {
                    stunTimer = stunTime;
                    state = States.Idle;
                }
                break;
        }

    }

    public States GetCurrentState() => state;
}
