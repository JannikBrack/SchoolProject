using UnityEngine;

public class TestItemSpawner : MonoBehaviour
{
    [SerializeField] private Item toSpawn;
    [SerializeField] private GameObject toSpawnGun;
    private void Awake()
    {
        if (toSpawnGun != null) Instantiate(toSpawnGun, transform.position, Quaternion.identity);
        else Instantiate(toSpawn.itemPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
