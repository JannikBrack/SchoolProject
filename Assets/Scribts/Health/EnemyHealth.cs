using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject healthBarParent;
    [SerializeField] float enemyHealth;
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
            Destroy(gameObject);
        }
        healthBar.transform.localScale = new Vector3(enemyHealth, 0f, 0f);
    }
}
