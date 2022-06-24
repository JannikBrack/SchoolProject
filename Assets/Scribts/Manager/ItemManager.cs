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

    void Start()
    {
        numbersOfSlots = 0;
    }

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
                        currentItem = items[itemID];
                        if (currentItem.isAmmo)
                        {
                            weapons[currentItem.weaponID].ammoAmount++;
                        }
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

    public void RemoveSlot(InventorySlot removingSlot)
    {
        slots.Remove(removingSlot);
        Destroy(removingSlot.gameObject);
        numbersOfSlots--;
    }

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
                
            }
            if (weapon.weaponType == 1)
            {
                weapon.damage = 200f;
            }
            if (weapon.weaponType == 2)
            {
                
            }
        }
    }

    private void UpdateAmmoAmount()
    {
        foreach (Weapon weapon in weapons)
        {
            foreach (Item item in items)
            {
                if (item.weaponID == weapon.weaponID)
                {
                    weapon.ammoAmount = GetAmmoAmount(item.itemID);
                }
            }
        }
    }

    public void WeaponLevelUp(int lvl)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.weaponType == 0)
            {
                //weapon scaling
            }
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
                        Debug.Log(i);
                        if (i <= 25f) weapon.damage = weapon.damage + (weapon.damage * 0.2f);
                        else
                            return;
                    }

            }
            if (weapon.weaponType == 2)
            {
                //weapon scaling
            }
        }
    }

    public void SetUpStarterEquip()
    {
        for(int i = 0; i < 30; i++)
        Instantiate(items[4].itemPrefab, new Vector3(itemOrientation.transform.position.x, itemOrientation.transform.position.y * Random.Range(0.1f,0.5f), itemOrientation.transform.position.z), Quaternion.identity);
    }
}
