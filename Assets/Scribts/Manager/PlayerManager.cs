using UnityEngine.UI;
using TMPro;
using UnityEngine;


//This is a class to store variables
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    
    void Awake()
    {
        //Allowing to make instances of this script
        instance = this;
        Lvl1SetUp();
    }

    public GameObject player;

    //Images to change if Skill is baught
    public Image cc_Background;
    public Image dp_Background;
    public Image fh_Background;
    public Image ss_Background;
    public Image fc_Background;
    
    [SerializeField] TextMeshProUGUI numberofSP;
    [SerializeField] TextMeshProUGUI playerLevelUI;

    [SerializeField] EnemyManager enemyManager;
    [SerializeField] ItemManager itemManager;
    public PlayerHealth health;

    public int playerLevel;

    
    public float xP_Amount;
    public float xP_Meeter;
    public float skillpointAmount;

    public bool deadPlayer;
    public bool gamePaused;

    private float lastHealth;
    private float nextHealth;
    private float nextSkillPoint;
    private float nextLvl_Up;

    [Header("Skills")]
    public bool closeCall;
    public bool deadlyPrecision;
    public bool fastHands;
    public bool sideStep;
    public bool fastChicken;

    /*
    public int lvl_Ups = 0;
    */

    private void FixedUpdate()
    {
        #region forPresentation
        /*
        if (Input.GetKey(KeyCode.P))
        {
            if (lvl_Ups > 0)
            {
                for (int i = 0; i < lvl_Ups; i++)
                {
                    lvl_Ups--;
                    float lastNeededXP = nextLvl_Up;
                    nextLvl_Up = nextLvl_Up + (nextLvl_Up * 0.05f);
                    xP_Amount += nextLvl_Up;
                    nextLvl_Up = lastNeededXP;
                    LevelUp(lvl_Ups);
                }
            }
        }
        */
        #endregion
    }

    
    private void UpdateUI()
    {
        numberofSP.text = skillpointAmount.ToString() + " Skillpoints";
        playerLevelUI.text = "Player Level: " + playerLevel.ToString();
    }

    //Method is used to give the player the xp amount from a meeter after he died
    public void MeeterToAmount()
    {
        xP_Amount =  xP_Amount + xP_Meeter;
        xP_Meeter = 0;
    }

    //Loads the settings for Level 1
    private void Lvl1SetUp()
    {
        playerLevel = 1;
        xP_Meeter = 0;
        xP_Amount = 0;
        skillpointAmount = 0;

        lastHealth = 2000f;
        health.SetHealth(lastHealth);

        nextLvl_Up = 10f;

        nextSkillPoint = 10f;

        enemyManager.levelSetUp(1);
        itemManager.WeaponLevelReset();
    }

    //Is leveling the stats (like health and damage or enemy health / damage) depending on the Xp amount
    public void LevelUp()
    {
            if (xP_Amount < nextLvl_Up)
            {
                return;
            }
            while (xP_Amount >= nextLvl_Up)
            {
                if (xP_Amount >= nextLvl_Up)
                {
                    playerLevel++;

                    if (playerLevel == nextSkillPoint && playerLevel <= 50)
                    {
                        skillpointAmount++;
                        nextSkillPoint += 10;
                        UpdateUI();
                    }

                    nextHealth = lastHealth + (lastHealth * 0.065f);
                    nextHealth = Mathf.Round(nextHealth);
                    lastHealth = nextHealth;
                    health.SetHealth(nextHealth);

                    xP_Amount -= nextLvl_Up;
                    nextLvl_Up = nextLvl_Up + (nextLvl_Up * 0.025f);

                    if (xP_Amount < 0) xP_Amount = 0f;
                }
            }
            itemManager.WeaponLevelUp(playerLevel);
            UpdateUI();
    }

    //Two alternative methods to load a statslvl
    public void LoadPlayLevel(int lvl, float lastXp_Amount, float last_Health)
    {
        for (int i = 1; i < lvl; i++)
        {
            if (playerLevel == nextSkillPoint && playerLevel <= 50)
            {
                skillpointAmount++;
                nextSkillPoint += 10;
            }
            playerLevel++;

            nextLvl_Up = nextLvl_Up + (nextLvl_Up * 0.05f);
        }
        xP_Amount = lastXp_Amount;

        lastHealth = last_Health;

        nextHealth = lastHealth + (lastHealth * 0.065f);
        nextHealth = Mathf.Round(nextHealth);
        lastHealth = nextHealth;
        health.SetHealth(nextHealth);
        itemManager.WeaponLevelUp(lvl);
        UpdateUI();
    }
    public void LoadPlayLevel(int lvl)
    {
        for (int i = 1; i < lvl; i++)
        {
            if (playerLevel == nextSkillPoint && playerLevel <= 50)
            {
                skillpointAmount++;
                nextSkillPoint += 10;
            }

            playerLevel++;

            nextHealth = lastHealth + (lastHealth * 0.065f);
            nextHealth = Mathf.Round(nextHealth);
            lastHealth = nextHealth;
            health.SetHealth(nextHealth);

            nextLvl_Up = nextLvl_Up + (nextLvl_Up * 0.05f);
        }
        if (playerLevel == nextSkillPoint && playerLevel <= 50)
        {
            skillpointAmount++;
            nextSkillPoint += 10;
        }

        enemyManager.levelSetUp(playerLevel);
        itemManager.WeaponLevelUp(lvl);
        UpdateUI();
    }

    //Methods to buy the skills
    public void buyCloseCall()
    {
        if (skillpointAmount > 0 && !closeCall)
        {
            skillpointAmount--;
            closeCall = true;
            cc_Background.color = Color.green;
            UpdateUI();
        }
    }
    public void buydeadlyPrecision()
    {
        if (skillpointAmount > 0 && ! deadlyPrecision)
        {
            skillpointAmount--;
            deadlyPrecision = true;
            dp_Background.color = Color.green;
            UpdateUI();
        }
    }
    public void buyFastHands()
    {
        if (skillpointAmount > 0 && !fastHands)
        {
            skillpointAmount--;
            fastHands = true;
            fh_Background.color = Color.green;
            UpdateUI();
        }
    }
    public void buySideStep()
    {
        if (skillpointAmount > 0 && !sideStep)
        {
            skillpointAmount--;
            sideStep = true;
            ss_Background.color = Color.green;
            UpdateUI();
        }
    }
    public void buyFastChicken()
    {
        if (skillpointAmount > 0 && !fastChicken)
        {
            skillpointAmount--;
            fastChicken = true;
            fc_Background.color = Color.green;
            UpdateUI();
        }
    }
}

