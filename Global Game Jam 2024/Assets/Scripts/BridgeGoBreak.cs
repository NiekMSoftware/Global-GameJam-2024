using UnityEngine;

public class BridgeGoBreak : MonoBehaviour
{
    [SerializeField] private GameObject noBrokenTilemap;
    [SerializeField] private GameObject brokenTilemap;

    private void Start()
    {
        noBrokenTilemap.SetActive(true);
        brokenTilemap.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        noBrokenTilemap.SetActive(false);
        brokenTilemap.SetActive(true);
    }
}
