using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] GameObject ItemImage;
    public bool owningItem;
    public bool isFull;
    public bool StackedItem;
    public int stackSize = 0;

    public void AddItem(Item item)
    {
        StackedItem = item.isStackable;
        ItemImage.SetActive(true);
        ItemImage.GetComponent<Image>().sprite = item.ItemIcon;
        stackSize++;
        owningItem = true;
    }

    public bool CheckItemAmount(Item item)
    {
        if (stackSize < item.maxStackSize) return true;
        else return false;
    }

}
