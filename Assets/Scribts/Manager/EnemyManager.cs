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

    //General
    private int playerLevel;

    public void levelSetUp(int lvl)
    {
        playerLevel = lvl;
        if (playerLevel < 30)
        {
            enemy_Level = 1f;
            zombie_Health = 500f;
            zombie_Damage = 100f;
            zombie_Xp_Dropamount = 25f;
        }
        else if (playerLevel < 50 && playerLevel >= 30)
        {
            enemy_Level = 2f;
            zombie_Health = 1500f;
            zombie_Damage = 1250f;
            zombie_Xp_Dropamount = 30f;
        }
        else if (playerLevel < 70 && playerLevel >= 50)
        {
            enemy_Level = 3f;
            zombie_Health = 4500f;
            zombie_Damage = 3500f;
            zombie_Xp_Dropamount = 50f;
        }
        else if (playerLevel < 80 && playerLevel >= 70)
        {
            enemy_Level = 4f;
            zombie_Health = 25000f;
            zombie_Damage = 5000f;
            zombie_Xp_Dropamount = 75f;
        }
        else if (playerLevel >= 80)
        {
            enemy_Level = 5f;
            zombie_Health = 75000f;
            zombie_Damage = 15000f;
            zombie_Xp_Dropamount = 125f;
        }
    }
}
