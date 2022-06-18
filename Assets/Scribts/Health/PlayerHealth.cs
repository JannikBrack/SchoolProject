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
                    if (ranNum <= 15)
                    {
                        playerHealth = 0.01f;
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
            healthbar.fillAmount = playerHealth / lvlPlayerhealth;
            LP_Amount.text = playerHealth.ToString() + " LP";
        }
    }
    public void Respawn()
    {
        //Animation
        deadScreeAnimator.SetTrigger("Respawn");

        PlayerManager.instance.deadPlayer = false;

        //LevelManagement
        PlayerManager.instance.MeeterToAmount();
        PlayerManager.instance.LevelUp(1);

        //Respawn
        transform.position = spawnpoint.position;
        Physics.SyncTransforms();

        //Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Refill Health
        RefillPlayerHealth(lvlPlayerhealth);
    }
    public void RefillPlayerHealth(float refillAmount)
    {
        playerHealth += refillAmount;
        LP_Amount.text = playerHealth.ToString() + " LP";
        healthbar.fillAmount = playerHealth / lvlPlayerhealth;
    }
    public void SetHealth(float newHealth)
    {
        playerHealth = newHealth;
        lvlPlayerhealth = newHealth;
        healthbar.fillAmount = 1;
        LP_Amount.text = playerHealth.ToString() + " LP";
    }
    public float GetHealth()
    {
        return playerHealth;
    }
    public float GetMaxHealth()
    {
        return lvlPlayerhealth;
    }
}
