using UnityEngine;

public class BridgeGoBreak : MonoBehaviour
{
    [SerializeField] private GameObject noBrokenTilemap;
    [SerializeField] private GameObject noBrokenCollisionMap;
    [SerializeField] private GameObject brokenTilemap;
    [SerializeField] private GameObject brokenCollisionMap;

    private void Start()
    {
        noBrokenTilemap.SetActive(true);
        noBrokenCollisionMap.SetActive(true);
        brokenTilemap.SetActive(false);
        brokenCollisionMap.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Player _)) return;

        noBrokenCollisionMap.SetActive(false);
        noBrokenTilemap.SetActive(false);
        brokenCollisionMap.SetActive(true);
        brokenTilemap.SetActive(true);
    }
}
