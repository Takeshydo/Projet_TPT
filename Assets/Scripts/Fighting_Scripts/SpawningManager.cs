using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Transform spawnPoint;

    private GameObject CEnemyInstance;

    void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if(CEnemyInstance != null)
        {
            Destroy(CEnemyInstance);
        }
        if(EnemyPrefab != null)
        {
            CEnemyInstance = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
            CEnemyInstance.name = EnemyPrefab.name;
        }
    }
}
