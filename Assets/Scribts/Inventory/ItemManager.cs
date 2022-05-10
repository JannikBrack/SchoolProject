using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject ItemParent;
    [SerializeField] GameObject Player;
    [SerializeField] InventorySlot[] slots;
    

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
                for (int i = 0; i < slots.Length; i++)
                {
                    if (!slots[i].isFull())
                    {
                        slots[i].AddItem(hitColider.gameObject);
                    }
                }
            }
            
        }

    }
}
