using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<QuestItem> items = new();

    public void AddItem(QuestItem item)
    {
        items.Add(item);
    }

    public void RemoveItem(QuestItem item)
    {
        items.Remove(item);
    }

    public bool HasItem(QuestItem.ItemType itemTypeToCheck)
    {
        foreach (QuestItem item in items)
        {
            if (item.itemType == itemTypeToCheck)
            {
                return true;
            }
        }

        return false;
    }
}
