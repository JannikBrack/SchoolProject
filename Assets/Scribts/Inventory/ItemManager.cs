using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject ItemParent;
    [SerializeField] GameObject Player;
    [SerializeField] InventorySlot[] slots;
    [SerializeField] Item[] items;
    [SerializeField] GameObject inventorySlotPrefab;
    [SerializeField] GameObject itemParent;
    int j;

    void Start()
    {

        
        for (int i = 0; i < 32; i++)
        {
            Instantiate(inventorySlotPrefab, itemParent.transform.position, Quaternion.identity).transform.SetParent(itemParent.transform);
        }
        
        slots = ItemParent.GetComponentsInChildren<InventorySlot>();
    }

    private void FixedUpdate()
    {
        Collider[] hitColiders = Physics.OverlapSphere(Player.transform.position, 4f);
        foreach (var hitColider in hitColiders)
        {
            if(hitColider.gameObject.tag == "Item")
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if(slots[i].owningItem && slots[i].CheckItemAmount(items[j]) && seachItem(hitColider.gameObject))
                    {
                        slots[i].AddItemInStack(items[j]);
                        Destroy(hitColider.gameObject);
                        return;
                    }
                    else if (!slots[i].owningItem && seachItem(hitColider.gameObject))
                    {
                        slots[i].AddItem(items[j]);
                        Destroy(hitColider.gameObject);
                        return;
                    }
                    
                }
            }
        }
    }
    private bool seachItem(GameObject hitColider)
    {
        for (j = 0; j < items.Length; j++)
        {
            if (items[j].ItemPrefab.name + "(Clone)" == hitColider.name) return true;
            else return false;
        }
        return false;
    }
}
