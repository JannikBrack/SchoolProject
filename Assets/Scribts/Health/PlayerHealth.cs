using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Image healthbar;
    [SerializeField] float playerHealth;
    [SerializeField] GameObject deadScreen;
    [SerializeField] Transform spawnpoint;
    [SerializeField] Animator deadScreeAnimator;
    void Awake()
    {
        playerHealth = 1;
        deadScreen.SetActive(false);
    }


    public void GetDamage(float amountOfDamage)
    {
        if (!PlayerManager.instance.deadPlayer)
        {
            playerHealth -= amountOfDamage;
            if (playerHealth <= 0)
            {
                if (PlayerManager.instance.DeadlyEscape && amountOfDamage > playerHealth)
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
                        Debug.Log(PlayerManager.instance.deadPlayer);
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
            healthbar.fillAmount = playerHealth;
        }
    }
    public void Respawn()
    {
        deadScreeAnimator.SetTrigger("Respawn");
        PlayerManager.instance.deadPlayer = false;
        PlayerManager.instance.MeeterToAmount();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerHealth = 1;
        healthbar.fillAmount = playerHealth;
        transform.position = spawnpoint.position;
        Physics.SyncTransforms();
    }
}
