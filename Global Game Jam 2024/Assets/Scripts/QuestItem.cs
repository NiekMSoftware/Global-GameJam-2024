using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public ItemType itemType;

    public enum ItemType
    {
        None,
        Wrench,
        HedgeTrimmer
    }
}
