using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject player;
    [SerializeField] TextMeshProUGUI numberOfXP;
    public float xP_Amount = 0;
    public float xP_Meeter = 0;
    public bool deadPlayer;

    [Header("Skills")]
    public bool doubleJump;
    public bool DeadlyEscape;
    public bool SpyEye;
    public bool DeagleDemon;
    public bool ShadowStep;
    public bool Laplas;
    public bool FastChicken;
    public bool FlashDash;


    private void FixedUpdate()
    {
        UpdateXpAmount();
    }

    private void UpdateXpAmount()
    {
            numberOfXP.text = xP_Amount.ToString() + " Xp";
    }

    public void MeeterToAmount()
    {
        if (deadPlayer)
        {
            xP_Amount = xP_Meeter;
            xP_Meeter = 0;
        }
    }
}
