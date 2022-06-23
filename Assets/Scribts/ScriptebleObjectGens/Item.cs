using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    

    //Item Identification
    public int itemID;
    public bool isUsable;

    //StackSettings
    public bool isStackable;
    public int maxStackSize;

    //If item can heal
    public bool isHealing;
    public double healPercent;

    //If Item is Ammo
    public bool isAmmo;
    public int weaponID;

    //DropSettings
    public int dropChances;
    public int maxDrops;
    public int minDrops;

    //Objects
    public GameObject itemPrefab;
    public Sprite itemIcon;
}
