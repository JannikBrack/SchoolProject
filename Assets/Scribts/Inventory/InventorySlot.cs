using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Button itemImage;
    [SerializeField] TextMeshProUGUI stackSizeText;
    [SerializeField] TextMeshProUGUI itemName;
    public bool owningItem;
    public bool StackedItem;
    public int stackSize = 0;
    public int maxStackSize;
    public int itemSlotID;

    public void AddNewItem(Item item)
    {
        itemSlotID = item.itemID;
        maxStackSize = item.maxStackSize;
        StackedItem = item.isStackable;
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


}
