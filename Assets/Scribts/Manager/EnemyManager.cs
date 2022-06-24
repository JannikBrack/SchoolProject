using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Singleton
    public static EnemyManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    //Enemy
    public float enemy_Level;

    //Zombie
    public float zombie_Health;
    public float zombie_Damage;
    public float zombie_Xp_Dropamount;

    public void levelSetUp(int lvl)
    {
        Debug.Log(lvl);
        if (lvl == 1)
        {
            enemy_Level = 1f;
            zombie_Health = 500f;
            zombie_Damage = 100f;
            zombie_Xp_Dropamount = 25f;
        }
        else if (lvl == 2)
        {
            enemy_Level = 2f;
            zombie_Health = 1500f;
            zombie_Damage = 1250f;
            zombie_Xp_Dropamount = 30f;
        }
        else if (lvl == 3)
        {
            enemy_Level = 3f;
            zombie_Health = 4500f;
            zombie_Damage = 3500f;
            zombie_Xp_Dropamount = 50f;
        }
        else if (lvl == 4)
        {
            enemy_Level = 4f;
            zombie_Health = 25000f;
            zombie_Damage = 5000f;
            zombie_Xp_Dropamount = 75f;
        }
        else if (lvl == 5)
        {
            enemy_Level = 5f;
            zombie_Health = 75000f;
            zombie_Damage = 15000f;
            zombie_Xp_Dropamount = 125f;
        }
    }
}
