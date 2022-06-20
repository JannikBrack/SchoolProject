using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public int weaponID;
    public int weaponType;

    public float firerate;
    public float cooldownTime;
    public int magazineSize;
    public int currentMagAmmoAmount;
    public int ammoAmount;

    public float aimSpeed;
    public bool isAimable;

    public float damage;

    public double dropChances;

    public GameObject prefab;
    public Sprite weaponIcon;
}
