using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] GameObject ItemImage;
    public bool isFull;
    public bool owningItem;
    public bool StackedItem;

    public void AddItem(Item Item)
    {
        ItemImage.SetActive(true);
        ItemImage.GetComponent<Image>().sprite = Item.ItemIcon;
        StackedItem = Item.isStackable;
        if (StackedItem)
        {
            if (Item.stackSize == Item.maxStackSize) isFull = true;
            else isFull = false;
        }
        else isFull = true;
    }


}
