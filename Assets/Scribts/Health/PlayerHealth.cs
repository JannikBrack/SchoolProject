using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Image Healthbar;
    [SerializeField] float playerHealth;
    [SerializeField] GameObject DeadScreen;
    public bool dead;
    void Awake()
    {
        playerHealth = 1;
        DeadScreen.SetActive(false);
        dead = false;
    }


    public void GetDamage(float amountOfDamage)
    {
        playerHealth -= amountOfDamage;
        if (playerHealth <= 0)
        {
            //Dead
            DeadScreen.SetActive(true);
            dead = true;
        }
        Healthbar.fillAmount = playerHealth;
    }
}
