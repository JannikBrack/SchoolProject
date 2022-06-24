using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject itemSpawner;

    private float enemyHealth;
    private float xP_Dropamount;
    private float enemyHealthPercent;
    private float enemyMaxHealth;

    [Header("ItemSpawning")]
    [SerializeField] Item[] lootItems;
    public float EnemyHealth { get => enemyHealth; set => enemyHealth = value; }
    public float XP_Dropamount { get => xP_Dropamount; set => xP_Dropamount = value; }
    public float EnemyHealthPercent { get => enemyHealthPercent; set => enemyHealthPercent = value; }
    public float EnemyMaxHealth { get => enemyMaxHealth; set => enemyMaxHealth = value; }

    private void Awake()
    {
        EnemyHealthPercent = 1;
    }

    public void GetDamage(float amountOfDamage)
    {
        EnemyHealth -= amountOfDamage;

        EnemyHealthPercent = EnemyHealth / EnemyMaxHealth;
        Debug.Log(EnemyHealth);
        if (enemyHealth <= 0)
        {
            float randomNumber = Random.Range(0, 100);


            //deathAnimations

            PlayerManager.instance.xP_Meeter += XP_Dropamount;


            if (!(lootItems.Length <= 0))
            {
                foreach (var item in lootItems)
                {
                    if (randomNumber <= item.dropChances)
                    {
                        for (int i = 0; i < Random.Range(item.minDrops, item.maxDrops); i++)
                        {
                            Instantiate(item.itemPrefab, itemSpawner.transform.position, Quaternion.identity);
                        }

                    }
                }
            }
            

            Destroy(gameObject);
        }
        healthBar.transform.localScale = new Vector3(3 * enemyHealthPercent, 0.25f, 0.01f);
    }

    
}
