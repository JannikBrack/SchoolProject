using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject healthBarParent;
    [SerializeField] GameObject itemSpawner;

    [SerializeField] float enemyHealth;
    [SerializeField] float xP_Dropamount;
    private float enemyHealthPercent;
    private float enemyMaxHealth;

    [SerializeField] LayerMask Player;

    [Header("ItemSpawning")]
    [SerializeField] Item[] lootItems;
    [SerializeField] Weapon[] lootWeapons;

    private void Awake()
    {
        enemyHealthPercent = 1f;
        healthBar.transform.localScale = new Vector3(3 * enemyHealthPercent, 0.25f, 0.01f);
        enemyMaxHealth = EnemyManager.instance.zombie_Health;
    }
    private void FixedUpdate()
    {
        enemyHealth = EnemyManager.instance.zombie_Health * enemyHealthPercent;
        enemyMaxHealth = EnemyManager.instance.zombie_Health;
        xP_Dropamount = EnemyManager.instance.zombie_Xp_Dropamount;
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

    public void GetDamage(float amountOfDamage)
    {
        enemyHealth -= amountOfDamage;
        Debug.Log("Enemyhealth: " + enemyHealth);
        Debug.Log("EnemyMaxhealth: " + enemyMaxHealth);
        enemyHealthPercent = enemyHealth / enemyMaxHealth;
        if (enemyHealth <= 0)
        {
            float randomNumber = Random.Range(0, 100);
            //deathAnimations

            PlayerManager.instance.xP_Meeter += xP_Dropamount;


            if (!(lootItems.Length <= 0))
            {
                foreach (var item in lootItems)
                {
                    if (randomNumber <= item.dropChances)
                    {
                        for (int i = 0; i < Random.Range(1, item.maxDrops); i++)
                        {
                            Instantiate(item.itemPrefab, itemSpawner.transform.position, Quaternion.identity);
                        }

                    }
                }
            }
            else if (!(lootWeapons.Length <= 0))
            {
                foreach (var weapon in lootWeapons)
                {
                    if (randomNumber <= weapon.dropChances) Instantiate(weapon.prefab, transform.position, Quaternion.identity);
                }
            }

            Destroy(gameObject);
        }
        healthBar.transform.localScale = new Vector3(3 * enemyHealthPercent, 0.25f, 0.01f);
    }
}
