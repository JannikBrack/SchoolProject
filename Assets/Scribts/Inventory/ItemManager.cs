using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private List<InventorySlot> slots;
    [SerializeField] private Item[] items;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject itemParent;
    private int itemID;
    private int numbersOfSlots;

    void Start()
    {
        numbersOfSlots = 0;
    }

    private void FixedUpdate()
    {
        Collider[] hitColiders = Physics.OverlapSphere(player.transform.position, 4f);
        foreach (var hitColider in hitColiders)
        {
            if(hitColider.gameObject.tag == "Item")
            {
                if(slots.Count == 0 && itemExist(hitColider.gameObject))
                {
                    createSlot(hitColider.gameObject);
                    return;
                }
                else
                {
                    foreach (InventorySlot slot in slots)
                    {
                        if (itemExist(hitColider.gameObject) && sameItemInSlot(slot) && !itemStackIsFull(slot) &&  slot.StackedItem)
                        {
                            slot.AddItem(items[itemID]);
                            Destroy(hitColider.gameObject);
                            return;
                        }
                    }
                    if (itemExist(hitColider.gameObject))
                    {
                        createSlot(hitColider.gameObject);
                        return;
                    }
                }

            }
        }
    }

    private void createSlot(GameObject hitColider)
    {
        Instantiate(inventorySlotPrefab, itemParent.transform.position, Quaternion.identity).transform.SetParent(itemParent.transform);
        slots = itemParent.GetComponentsInChildren<InventorySlot>().ToList<InventorySlot>();
        slots[numbersOfSlots].AddNewItem(items[itemID]);
        slots[numbersOfSlots].owningItem = true;
        numbersOfSlots++;
        Destroy(hitColider);
    }

    private bool itemExist(GameObject hitColider)
    {
        foreach (Item item in items)
        {
            if (hitColider.name.StartsWith(item.itemID.ToString()))
            {
                itemID = item.itemID;
                return true;
            }
        }
        return false;
    }
    private bool sameItemInSlot(InventorySlot slot)
    {
        if (itemID == slot.itemSlotID) return true;
        else return false;
    }
    private bool itemStackIsFull(InventorySlot slot)
    {
        if (slot.stackSize == slot.maxStackSize) return true;
        else return false;
    }
    public void RemoveItem(InventorySlot removingSlot)
    {
        slots.Remove(removingSlot);
        Destroy(removingSlot.gameObject);
        numbersOfSlots--;
    }
}
