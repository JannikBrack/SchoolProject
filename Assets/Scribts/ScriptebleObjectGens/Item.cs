using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public bool isStackable;
    public int maxStackSize;
    public int stackSize;

    public GameObject ItemPrefab;
    public Sprite ItemIcon;
}
