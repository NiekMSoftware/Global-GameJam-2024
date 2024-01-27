using UnityEngine;

public class Hedge : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    public void Use()
    {
        if (inventory.HasItem(QuestItem.ItemType.HedgeTrimmer))
            Destroy(gameObject);
    }
}
