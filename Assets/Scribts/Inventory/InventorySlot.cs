using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] GameObject InvSlot;

    public void AddItem(GameObject newItem)
    {
        newItem.transform.SetParent(InvSlot.transform, false);
        newItem.GetComponent<Rigidbody>().useGravity = false;
        newItem.transform.localScale = new Vector3(100f, 100f,100f);
        newItem.layer = 5;
    }

    public bool isFull()
    {
        if (InvSlot.transform.childCount > 2)
        {
            return true;
        }
        return false;
    }
}
