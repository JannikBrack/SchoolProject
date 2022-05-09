using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] GameObject ItemSlot;

    public void AddItem(GameObject newItem)
    {
        ItemSlot = newItem;
    }
}
