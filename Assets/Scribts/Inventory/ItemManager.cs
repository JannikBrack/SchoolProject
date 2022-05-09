using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject ItemParent;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject[] slots;
    

    void Start()
    {
        slots = ItemParent.GetComponentsInChildren<GameObject>();
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
                    if (slots[i].gameObject.GetComponentInChildren<GameObject>().tag == "Items")
                    {
                        Debug.Log("ist voll");
                    }
                    else
                    {
                        hitColider.gameObject.transform.SetParent(slots[i].transform);
                    }
                }
            }
            
        }

    }
}
