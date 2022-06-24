using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Image healthbar;
    [SerializeField] float playerHealth;
    private float lvlPlayerhealth;
    [SerializeField] TextMeshProUGUI LP_Amount;
    [SerializeField] GameObject deadScreen;
    [SerializeField] Transform spawnpoint;
    [SerializeField] Animator deadScreeAnimator;
    void Awake()
    {
        deadScreen.SetActive(false);
    }

    private void Update()
    {
        //for presetation instandkill
        if (Input.GetKeyUp(KeyCode.K)) GetDamage(playerHealth);
    }

    //methode to subrtact healt depending on the enemy stats and controlls death
    public void GetDamage(float amountOfDamage)
    {
        if (!PlayerManager.instance.deadPlayer)
        {
            playerHealth -= amountOfDamage;
            if (playerHealth <= 0)
            {
                if (PlayerManager.instance.closeCall && amountOfDamage > playerHealth)
                {
                    int ranNum = Random.Range(0, 100);
                    if (ranNum <= 20)
                    {
                        playerHealth = lvlPlayerhealth * 0.1f;
                    }
                    else
                    {
                        //Dead
                        deadScreeAnimator.Play("PlayerDied");
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.Confined;
                        PlayerManager.instance.deadPlayer = true;
                    }
                }
                else
                {
                    //Dead
                    deadScreeAnimator.Play("PlayerDied");
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    PlayerManager.instance.deadPlayer = true;
                }
            }
            playerHealth = Mathf.Round(playerHealth);
            healthbar.fillAmount = playerHealth / lvlPlayerhealth;
            LP_Amount.text = playerHealth.ToString() + " HP";
        }
    }
    
    //allows the player to respawn
    public void Respawn()
    {
        //Refill Health
        RefillPlayerHealth(lvlPlayerhealth);

        //Animation
        deadScreeAnimator.SetTrigger("Respawn");

        PlayerManager.instance.deadPlayer = false;

        //LevelManagement
        PlayerManager.instance.MeeterToAmount();
        PlayerManager.instance.LevelUp();

        //Respawn
        transform.position = spawnpoint.position;
        Physics.SyncTransforms();

        //Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        
    }

    //refills player health up to an amount (for respawning)
    public void RefillPlayerHealth(float refillAmount)
    {
        playerHealth += refillAmount;
        LP_Amount.text = playerHealth.ToString() + " HP";
        healthbar.fillAmount = playerHealth / lvlPlayerhealth;
    }

    //refills player health up to given percent (for healing items
    public void RefillPlayerHealth(double refillPercent)
    {
        float refillingHealth = (float)(lvlPlayerhealth * refillPercent);
        playerHealth += refillingHealth;
        if (playerHealth > lvlPlayerhealth) playerHealth = lvlPlayerhealth;

        LP_Amount.text = playerHealth.ToString() + " HP";
        healthbar.fillAmount = playerHealth / lvlPlayerhealth;
    }

    //sets the health (for scaling stats)
    public void SetHealth(float newHealth)
    {
        playerHealth = newHealth;
        lvlPlayerhealth = newHealth;
        healthbar.fillAmount = 1;
        LP_Amount.text = playerHealth.ToString() + " HP";
    }

    //getting methods
    public float GetHealth()
    {
        return playerHealth;
    }
    public float GetMaxHealth()
    {
        return lvlPlayerhealth;
    }
}
