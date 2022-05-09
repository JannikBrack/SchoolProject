using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject ItemParent;
    [SerializeField] GameObject Player;
    [SerializeField] int ItemSlotCount;
    [SerializeField] int ItemCount;
    GameObject[] InventoryItems;
    void Start()
    {
        foreach(Transform child in ItemParent.transform)ItemSlotCount++;
        InventoryItems = new GameObject[ItemSlotCount];
        ItemSlotCount = InventoryItems.Length;
    }

    private void Update()
    {
        Collider[] hitColiders = Physics.OverlapSphere(Player.transform.position, 0.1f);
        foreach (var hitColider in hitColiders)
        {
            Debug.Log(hitColider.gameObject.name);
        }
    }
}
