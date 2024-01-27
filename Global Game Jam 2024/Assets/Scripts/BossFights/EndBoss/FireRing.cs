using UnityEngine;

public class FireRing : MonoBehaviour
{
    [SerializeField] private float maxSize;
    [SerializeField] private float startSize;
    [SerializeField] private float increaseSpeed;

    private float currentSizeValue;

    private void Start()
    {
        Vector3 startScale;
        startScale.x = startSize;
        startScale.y = startSize;
        startScale.z = startSize;
        transform.localScale = startScale;
    }

    private void Update()
    {
        currentSizeValue += increaseSpeed * Time.deltaTime;

        if (currentSizeValue >= maxSize)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 size = transform.localScale;
        size.x += currentSizeValue;
        size.y += currentSizeValue;
        size.z += currentSizeValue;
        transform.localScale = size;
    }
}
