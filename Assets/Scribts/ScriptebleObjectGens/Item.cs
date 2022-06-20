using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public bool isStackable;
    public int maxStackSize;
    public int itemID;
    public bool isUsable;

    public bool isHealing;
    public double healPercent;

    public bool isAmmo;
    public int weaponID; 

    public GameObject itemPrefab;
    public Sprite itemIcon;
    public int dropChances;
    public int maxDrops;
    public int minDrops;
}
