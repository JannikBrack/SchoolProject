using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject healthBarParent;
    [SerializeField] float enemyHealth;
    [SerializeField] GameObject itemSpawner;
    [SerializeField] LayerMask Player;

    [Header("ItemSpawning")]
    [SerializeField] Item[] lootItems;
    [SerializeField] Weapon[] lootWeapons;

    private void Awake()
    {
        healthBar.transform.localScale = new Vector3(enemyHealth, 0.25f, 0.01f);
    }
    private void FixedUpdate()
    {

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
        if (enemyHealth <= 0)
        {
            float randomNumber = Random.Range(0, 100);
            //deathAnimations
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
                foreach(var weapon in lootWeapons)
                {
                    if (randomNumber <= weapon.dropChances) Instantiate(weapon.prefab, transform.position, Quaternion.identity);
                }
            }
            
            Destroy(gameObject);
        }
        healthBar.transform.localScale = new Vector3(enemyHealth, 0.25f, 0.01f);
    }
}
