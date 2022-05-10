using UnityEngine;

public class TestItemSpawner : MonoBehaviour
{
    [SerializeField] private Item toSpawn;
    private void Awake()
    {
        Instantiate(toSpawn.ItemPrefab,transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
