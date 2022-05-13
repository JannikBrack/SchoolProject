using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] GameObject ItemImage;
    [SerializeField] TextMeshProUGUI stackSizeText;
    public bool owningItem;
    public bool StackedItem;
    public int stackSize = 0;

    public void AddItem(Item item)
    {
        StackedItem = item.isStackable;
        ItemImage.SetActive(true);
        ItemImage.GetComponent<Image>().sprite = item.ItemIcon;
        stackSize++;
        stackSizeText.text = stackSize.ToString();
        owningItem = true;
    }

    public void AddItemInStack(Item item)
    {
        stackSize++;
        stackSizeText.text = stackSize.ToString();
    }

    public bool CheckItemAmount(Item item)
    {
        if (stackSize < item.maxStackSize) return true;
        else return false;
    }

}
