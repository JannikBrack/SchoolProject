using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class InventorySlot : MonoBehaviour
{
    #region Variables
    [SerializeField] Button itemImage;
    [SerializeField] TextMeshProUGUI stackSizeText;
    [SerializeField] TextMeshProUGUI itemName;

    private ItemManager itemManager;
    private PlayerHealth playerHealth;

    public bool owningItem;
    public bool owningAmmo;
    #endregion

    #region itemVariables
    public bool StackedItem;
    public int stackSize = 0;
    public int maxStackSize;
    public int itemSlotID;
    public bool isUseble;
    #endregion

    #region weaponVariables
    public int weaponID;
    #endregion

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        itemManager = player.GetComponent<ItemManager>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    #region ItemCode

    //adding new item and setting important variables to prevent different items on one slot
    public void AddNewItem(Item item)
    {
        itemSlotID = item.itemID;
        maxStackSize = item.maxStackSize;
        StackedItem = item.isStackable;
        isUseble = item.isUsable;
        itemImage.GetComponent<Image>().sprite = item.itemIcon;
        stackSize++;
        stackSizeText.text = stackSize.ToString();
        itemName.text = item.name;
        owningItem = true;
    }

    public void AddItem(Item item)
    {
        stackSize++;
        stackSizeText.text = stackSize.ToString();
    }

    //removes the items if it is healing or useble
    public void RemoveItem()
    {
        Item item = itemManager.GetItem(itemSlotID);
        if (isUseble)
        {
            if (item.isHealing)
                playerHealth.RefillPlayerHealth(item.healPercent);
            stackSize--;
            if (stackSize <= 0) itemManager.RemoveSlot(this);
            else stackSizeText.text = stackSize.ToString();
        }
    }
    #endregion

    #region WeaponCode
    //adds a weapon to a slot if the weapon in the hotbar is full
    public void AddNewWeapon(Weapon weapon)
    {
        itemImage.GetComponent<Image>().sprite = weapon.weaponIcon;
        itemName.text = weapon.weaponName;
        weaponID = weapon.weaponID;
    }

    public void RemoveStackSize(int removeAmount)
    {
        stackSize -= removeAmount;
        if (stackSize == 0) itemManager.RemoveSlot(this);
        stackSizeText.text = stackSize.ToString();
    }

    public int GetStackSize()
    {
        return stackSize;
    }
    #endregion
}
