using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject ItemParent;
    [SerializeField] GameObject Player;
    [SerializeField] InventorySlot[] slots;
    [SerializeField] Item[] items;
    

    void Start()
    {
        slots = ItemParent.GetComponentsInChildren<InventorySlot>();
    }

    private void FixedUpdate()
    {
        Collider[] hitColiders = Physics.OverlapSphere(Player.transform.position, 4f);
        foreach (var hitColider in hitColiders)
        {
            if(hitColider.gameObject.tag == "Item")
            {
                Debug.Log(hitColider.gameObject.name);
                for (int i = 0; i < slots.Length; i++)
                {
                    if (!slots[i].isFull)
                    {
                        for (int j = 0; j < items.Length; j++)
                        {
                            if (items[j].ItemPrefab.name + "(Clone)" == hitColider.gameObject.name) {
                                slots[i].AddItem(items[j]);
                                Destroy(hitColider.gameObject);
                                
                            }
                        }
                        return;
                    }
                }
                
                
            }
            
        }

    }
}
