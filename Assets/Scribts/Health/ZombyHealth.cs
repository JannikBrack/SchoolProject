using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombyHealth : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject healthBarParent;
    [SerializeField] EnemyHealthManager enemyHealthManager;

    private float enemyHealth;
    private float xP_Dropamount;
    private float enemyHealthPercent;
    private float enemyMaxHealth;


    private void Awake()
    {
        healthBar.transform.localScale = new Vector3(3 * enemyHealthPercent, 0.25f, 0.01f);
        enemyMaxHealth = EnemyManager.instance.zombie_Health;
    }
    private void FixedUpdate()
    {
        enemyHealthPercent = enemyHealthManager.EnemyHealthPercent;

        enemyHealth = (EnemyManager.instance.zombie_Health * enemyHealthPercent);
        enemyMaxHealth = EnemyManager.instance.zombie_Health;
        xP_Dropamount = EnemyManager.instance.zombie_Xp_Dropamount;

        UpdateData();

        Collider[] hitColiders = Physics.OverlapSphere(transform.position, 20f);

        foreach (var hitcolider in hitColiders)
        {
            if (hitcolider.gameObject.CompareTag("Player"))
            {
                healthBarParent.SetActive(true);
                healthBarParent.transform.LookAt(hitcolider.gameObject.transform);
                return;
            }
            else
            {
                healthBarParent.SetActive(false);
            }
        }
    }

    private void UpdateData()
    {
        enemyHealthManager.EnemyHealth = enemyHealth;
        enemyHealthManager.EnemyMaxHealth = enemyMaxHealth;
        enemyHealthManager.XP_Dropamount = xP_Dropamount;
    }
}
