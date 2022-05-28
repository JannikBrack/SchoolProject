using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGun", menuName = "Gun")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public int weaponID;
    public float firerate;
    public float aimSpeed;
    public bool isAimable;

    public GameObject prefab;
    public Sprite weaponIcon;
}
