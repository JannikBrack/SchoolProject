using UnityEngine;

public class TestItemSpawner : MonoBehaviour
{
    [SerializeField] private Item toSpawn;
    private void Awake()
    {
        Instantiate(toSpawn.itemPrefab,transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
