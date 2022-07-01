using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> slots;

    [SerializeField] private Item[] items;
    [SerializeField] private Weapon[] weapons;

    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject itemParent;
    [SerializeField] private GameObject itemOrientation;

    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private LayerMask liftable;
    private int itemID;
    private int weaponID;
    private int numbersOfSlots;
    private Item currentItem;

    //Resets the number of Inventory slots
    void Start()
    {
        numbersOfSlots = 0;
    }

    //Searching for items or weapos in the liftable layer
    private void FixedUpdate()
    {
        Collider[] hitColiders = Physics.OverlapSphere(transform.position, 5f,liftable);
        foreach (var hitColider in hitColiders)
        {
            if (hitColider.gameObject.tag == "Item")
            {
                if (slots.Count == 0 && itemExist(hitColider.gameObject))
                {
                    createSlot(hitColider.gameObject, false);
                    currentItem = items[itemID];
                    if (currentItem.isAmmo)
                    {
                        weapons[currentItem.weaponID].ammoAmount++;
                    }
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
                            currentItem = items[itemID];
                            if (currentItem.isAmmo)
                            {
                                weapons[currentItem.weaponID].ammoAmount++;
                            }
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

        UpdateAmmoAmount();
    }

    //Creates a inventory slot when a new item is picked up depending on if it is an weapon or not
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

    //searching the item in the item array retuns true if the item exists and saves the item id
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

    //searches the item in the specific solt
    private bool sameItemInSlot(InventorySlot slot)
    {
        if (itemID == slot.itemSlotID) return true;
        else return false;
    }

    //looking if the inventory slot is full
    private bool itemStackIsFull(InventorySlot slot)
    {
        if (slot.stackSize == slot.maxStackSize) return true;
        else return false;
    }

    //Revoves items from the slot and emptyslots
    public void RemoveSlot(InventorySlot removingSlot)
    {
        slots.Remove(removingSlot);
        Destroy(removingSlot.gameObject);
        numbersOfSlots--;
    }

    //removes the ammo form the inventory which is loaded in the magazine
    public void RemoveUsedAmmo(int itemID, int removeAmount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemSlotID == itemID)
            {
                slot.RemoveStackSize(removeAmount);
                return;
            }
        }
    }

    //gets the current ammo amount of each ammunition that the ui could be updatet correctly
    public int GetAmmoAmount(int itemID)
    {
        int ammoAmount = 0;

        foreach (InventorySlot slot in slots)
        {
            if (slot.itemSlotID == itemID)
            {
                ammoAmount += slot.GetStackSize();
            }
            else return ammoAmount;
        }
        return ammoAmount;
    }

    //searching the weapon in the weapon array retuns true if the weapon exists and safes the weapon id
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

    public Item GetItem(int itemID)
    {
        return items[itemID];
    }

    public void WeaponLevelReset()
    {
        foreach(var weapon in weapons)
        {
            if (weapon.weaponType == 0)
            {
                weapon.damage = 100f;
            }
            if (weapon.weaponType == 1)
            {
                weapon.damage = 200f;
            }
            if (weapon.weaponType == 2)
            {
                weapon.damage = 800f;
            }
        }
    }

    //updates the current ammo amount
    private void UpdateAmmoAmount()
    {
        
        foreach (Weapon weapon in weapons)
        {
            foreach (Item item in items)
            {
                if (item.isAmmo && item.weaponID == weapon.weaponID)
                {
                    weapon.ammoAmount = GetAmmoAmount(item.itemID);
                }
            }
        }
    }
    
    //meathod to scale the weapons
    public void WeaponLevelUp(int lvl)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.weaponType == 1)
            {
                if (lvl == 1)
                    weapon.damage = 200;
                else if (lvl <= 25f)
                    for (int i = 2; i < lvl + 1; i++)
                    {
                        if (i <= 25f) weapon.damage = weapon.damage + (weapon.damage * 0.2f);
                        else
                            return;
                    }
                else
                    for (int i = 2; i < 26; i++)
                    {
                        if (i <= 25f) weapon.damage = weapon.damage + (weapon.damage * 0.2f);
                        else
                            return;
                    }

            }
        }
    }

    //spawns a fix amount of ammo near the player as staterequip
    public void SetUpStarterEquip()
    {
        for(int i = 0; i < 30; i++)
        {
            Instantiate(items[4].itemPrefab, new Vector3(itemOrientation.transform.position.x, itemOrientation.transform.position.y * Random.Range(0.1f, 0.5f), itemOrientation.transform.position.z), Quaternion.identity);

            Instantiate(items[5].itemPrefab, new Vector3(itemOrientation.transform.position.x, itemOrientation.transform.position.y * Random.Range(0.1f, 0.5f), itemOrientation.transform.position.z), Quaternion.identity);
        }
            
    }
}
