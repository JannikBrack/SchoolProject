using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string typeName;
    public float firerate;
    public float aimSpeed;
    public bool isAimeble;
    public GameObject prefab;
}
