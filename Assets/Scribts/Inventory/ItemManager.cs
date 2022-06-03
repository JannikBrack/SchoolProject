using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private List<InventorySlot> slots;
    [SerializeField] private Item[] items;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject itemParent;
    [SerializeField] private WeaponManager weaponManager;
    private int itemID;
    private int weaponID;
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
            if (hitColider.gameObject.tag == "Item")
            {
                if (slots.Count == 0 && itemExist(hitColider.gameObject))
                {
                    
                    createSlot(hitColider.gameObject, false);
                    return;
                }
                else
                {
                    foreach (InventorySlot slot in slots)
                    {
                        if (itemExist(hitColider.gameObject) && sameItemInSlot(slot) && !itemStackIsFull(slot) && slot.StackedItem)
                        {
                            slot.AddItem(items[itemID]);
                            Destroy(hitColider.gameObject);
                            return;
                        }
                    }
                    if (itemExist(hitColider.gameObject))
                    {
                        createSlot(hitColider.gameObject, false);
                        return;
                    }
                }

            }
            else if (hitColider.gameObject.tag == "Weapons")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if(gunExist(hitColider.gameObject))
                    {
                        weaponManager.loadout[weapons[weaponID].weaponType] = weapons[weaponID];
                        weaponManager.SwitchWeaponIcon(weapons[weaponID], true);
                        Destroy(hitColider.gameObject);
                    }
                }
            }
        }
    }

    private void createSlot(GameObject hitColider, bool isWeapon)
    {
        if (isWeapon)
        {
            Instantiate(inventorySlotPrefab, itemParent.transform.position, Quaternion.identity).transform.SetParent(itemParent.transform);
            slots = itemParent.GetComponentsInChildren<InventorySlot>().ToList<InventorySlot>();
            slots[numbersOfSlots].AddNewWeapon(weapons[weaponID]);
            slots[numbersOfSlots].tag = "WeaponSlot";
            slots[numbersOfSlots].owningItem = true;
            numbersOfSlots++;
            Destroy(hitColider);
        }
        else
        {
            Instantiate(inventorySlotPrefab, itemParent.transform.position, Quaternion.identity).transform.SetParent(itemParent.transform);
            slots = itemParent.GetComponentsInChildren<InventorySlot>().ToList<InventorySlot>();
            slots[numbersOfSlots].AddNewItem(items[itemID]);
            slots[numbersOfSlots].owningItem = true;
            numbersOfSlots++;
            Destroy(hitColider);
        }

    }
    #region items
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

    #endregion

    public void RemoveSlot(InventorySlot removingSlot)
    {
            slots.Remove(removingSlot);
            Destroy(removingSlot.gameObject);
            numbersOfSlots--;
    }

    private bool gunExist(GameObject hitColider)
    {
        foreach (Weapon weapon in weapons)
        {
            if (hitColider.name.StartsWith(weapon.weaponID.ToString()))
            {
                weaponID = weapon.weaponID;
                return true;
            }
        }
        return false;
    }

    private void createNewWeaponSlot(Weapon weapon)
    {
        Instantiate(inventorySlotPrefab, itemParent.transform.position, Quaternion.identity).transform.SetParent(itemParent.transform);
        slots = itemParent.GetComponentsInChildren<InventorySlot>().ToList<InventorySlot>();
        slots[numbersOfSlots].AddNewWeapon(weapon);
        slots[numbersOfSlots].tag = "WeaponSlot";
        slots[numbersOfSlots].owningItem = true;
        numbersOfSlots++;
    }

    public void SwitchWeapons(int weaponType)
    {
        if (!weaponManager.invWeaponSlots[weaponType].activeInHierarchy) return;
        foreach(var weapon in weapons)
        {
            if (weapon.weaponIcon == null) continue;
            else if (weapon.weaponIcon.name.Equals(weaponManager.invWeaponSlots[weaponType].name))
            {
                createNewWeaponSlot(weapon);
                return;
            }
        }
    }
}
