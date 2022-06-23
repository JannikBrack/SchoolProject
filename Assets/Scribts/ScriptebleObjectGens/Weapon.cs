using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    //Weapon Identification
    public string weaponName;
    public int weaponID;
    public int weaponType;

    //ShootingSettings
    public float firerate;
    public float cooldownTime;
    public int magazineSize;
    public int currentMagAmmoAmount;
    public int ammoAmount;
    public int ammoID;
    public float damage;

    //Objects and Scribts
    public GameObject prefab;
    public Sprite weaponIcon;
    public ShootAnimation ShootAnimation;
}
