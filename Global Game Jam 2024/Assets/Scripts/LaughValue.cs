using UnityEngine;

public class LaughValue : MonoBehaviour
{
    [SerializeField] private float laughValue;
    [SerializeField] private float maxLaughValue;
    [SerializeField] private float laughDecreaseAmountPerSecond;
    [SerializeField] private float stunTime;
    [SerializeField] private Player player;
    [SerializeField] private WeaponManager weaponManager;

    private float stunTimer;

    private States state;

    public enum States
    {
        Stunned,
        Idle
    }

    private void Update()
    {
        switch (state)
        {
            case States.Idle:
                laughValue -= Time.deltaTime * laughDecreaseAmountPerSecond;
                laughValue = Mathf.Clamp(laughValue, 0, maxLaughValue);

                if (laughValue >= maxLaughValue)
                {
                    state = States.Stunned;
                    laughValue = 0;
                    player.stunned = true;
                    weaponManager.stunned = true;
                    print("stun player");
                }
                break;

            case States.Stunned:
                stunTimer -= Time.deltaTime;

                if (stunTimer <= 0)
                {
                    player.stunned = false;
                    weaponManager.stunned = false;
                    stunTimer = stunTime;
                    state = States.Idle;
                }
                break;
        }
    }

    public void AddAmount(float amount)
    {
        laughValue += amount;
    }

    public States GetCurrentState() => state;
}
