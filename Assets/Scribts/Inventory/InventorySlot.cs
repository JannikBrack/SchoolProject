using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class InventorySlot : MonoBehaviour
{
    [SerializeField] Button itemImage;
    [SerializeField] TextMeshProUGUI stackSizeText;
    [SerializeField] TextMeshProUGUI itemName;
    public ItemManager itemManager;
    public bool owningItem;
    public bool StackedItem;
    public int stackSize = 0;
    public int maxStackSize;
    public int itemSlotID;
    public bool isUseble;

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        itemManager = player.GetComponent<ItemManager>();
    }
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

    public void RemoveItem()
    {
        if (isUseble)
        {
            stackSize--;
            if (stackSize == 0) itemManager.RemoveItem(this);
            else stackSizeText.text = stackSize.ToString();
        }
    }
}
