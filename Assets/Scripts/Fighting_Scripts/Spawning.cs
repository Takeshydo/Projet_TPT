using UnityEngine;

public class Hero1 : MonoBehaviour
{
    //private bool fighter = true;

    [SerializeField] private string placement = "FighterPosition";
    [SerializeField] private string Zonef = "Front";
    [SerializeField] private string ZoneB = "Back";
    [SerializeField] private GameObject player;
    public GameObject Boss_Detect;
    public GameObject EnemyPrefab;
    public Transform spawnPoint;
    private GameObject CEnemyInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManagement.Instance != null)
        {
            if (GameManagement.Instance.enteredFromBack)
            {
                SpawnBack();
            }
            else
            {
                SpawnDefault();
            }
        }
        else {
            Debug.Log("Frr ta pas creer ton GameManagement clown");
        }

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
            CEnemyInstance.name = EnemyPrefab.name;
        }
    }

    public void SpawnDefault()
    {
        GameObject Position = GameObject.FindGameObjectWithTag(placement);
        GameObject FrontZone = GameObject.FindGameObjectWithTag(Zonef);

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

    public void SpawnBack()
    {
        GameObject Position = GameObject.FindGameObjectWithTag(placement);
        GameObject BackZone = GameObject.FindGameObjectWithTag(ZoneB);

        if (BackZone != null)
        {
            Transform spawnPosition = null;

            foreach (Transform child in BackZone.transform)
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
                Debug.LogError("Clown i a pas de ref 2");
            }
        }
        else
        {
            Debug.LogError("Rien connard");
        }

    }
}
