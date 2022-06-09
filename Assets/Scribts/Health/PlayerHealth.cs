using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Image healthbar;
    [SerializeField] float playerHealth;
    [SerializeField] GameObject deadScreen;
    [SerializeField] Transform spawnpoint;
    [SerializeField] Animator deadScreeAnimator;
    public bool dead;
    void Awake()
    {
        playerHealth = 1;
        deadScreen.SetActive(false);
        dead = false;
    }


    public void GetDamage(float amountOfDamage)
    {
        if (!dead)
        {
            playerHealth -= amountOfDamage;
            if (playerHealth <= 0)
            {
                //Dead
                deadScreeAnimator.Play("PlayerDied");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                dead = true;
            }
            healthbar.fillAmount = playerHealth;
        }
    }
    public void Respawn()
    {
        deadScreeAnimator.SetTrigger("Respawn");
        dead = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerHealth = 1;
        healthbar.fillAmount = playerHealth;
        Vector3.Lerp(transform.position, spawnpoint.position,Vector3.Distance(spawnpoint.position,transform.position));
    }
}
