using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public int weaponID;
    public float firerate;
    public float aimSpeed;
    public bool isAimable;
    public int weaponType;
    public float damage;

    public GameObject prefab;
    public Sprite weaponIcon;
}
