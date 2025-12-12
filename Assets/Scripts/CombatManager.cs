using UnityEngine;

public class CombatManager : MonoBehaviour
{
    //private bool fighter = true;

    [SerializeField] private string placement = "FighterPosition";
    [SerializeField] private string Zone = "Front";
    [SerializeField] private GameObject player;

    public Action heroAction;
    public UI_Update_Info ui;
    public GameObject EnemyPrefab;
    public Transform spawnPoint;
    private GameObject CEnemyInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnHero();        
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {

        //Choper le spawning spécifique du monstre
        if(CEnemyInstance != null)
        {
            Destroy(CEnemyInstance);
        }
        if(EnemyPrefab != null)
        {
            CEnemyInstance = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
            heroAction.SetNewEnemy(CEnemyInstance);
            ui.SetNewEnemy(CEnemyInstance);
        }
    }
    public void SpawnHero()
    {
        GameObject Position = GameObject.FindGameObjectWithTag(placement);
        GameObject FrontZone = GameObject.FindGameObjectWithTag(Zone);

        if (FrontZone != null)
        {
            Transform spawnPosition = null;

            foreach (Transform child in FrontZone.transform)
            {
                if (child.CompareTag(placement))
                {
                    spawnPosition = child;
                    break;
                }
            }
            if (spawnPosition != null)
            {
                BoxCollider box = spawnPosition.GetComponent<BoxCollider>();
                Vector3 spawnPos = box.bounds.center;

                Instantiate(player, spawnPos, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Clown i a pas de ref");
            }
        }
        else
        {
            Debug.LogError("Rien connard");
        }
    }
}
