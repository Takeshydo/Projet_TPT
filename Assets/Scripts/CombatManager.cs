using UnityEngine;

public class CombatManager : MonoBehaviour
{
    //private bool fighter = true;

    [SerializeField] private string placement = "FighterPosition";
    [SerializeField] private string Zone = "Front";
    [SerializeField] private string ZoneB = "Back";

    public CameraFigthing cam;
    public UI_Update_Info ui;
    public GameObject EnemyPrefab;
    public GameObject PlayerPrefab;
    public Transform spawnPoint;
    private GameObject CEnemyInstance;
    private GameObject CPlayerInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemy();

        if (GameManagement.Instance != null)
        {
            
            if (GameManagement.Instance.enteredFromBack)
            {
                SpawnHeroB();
            }
            else
            {
                SpawnHeroF();

            }
        }
        else
        {
            Debug.Log("Frr ta pas creer ton GameManagement clown");
        }


    }

    public void SpawnEnemy()
    {

        //Choper le spawning spécifique du monstre
        if (CEnemyInstance != null)
        {
            Destroy(CEnemyInstance);
        }
        if (EnemyPrefab != null)
        {
            CEnemyInstance = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
            ui.SetNewEnemy(CEnemyInstance);
            cam.SetNewEnemy(CEnemyInstance);
        }
    }
    public void SpawnHeroF()
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

                CPlayerInstance = Instantiate(PlayerPrefab, spawnPos, Quaternion.identity);
                var playerAction = CPlayerInstance.GetComponent<Action>();
                cam.SetNewHero(CPlayerInstance);
                playerAction.SetNewHero(CPlayerInstance);
                if (CEnemyInstance != null)
                {
                    playerAction.SetNewEnemy(CEnemyInstance);
                }

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

    public void SpawnHeroB()
    {
        GameObject Position = GameObject.FindGameObjectWithTag(placement);
        GameObject FrontZone = GameObject.FindGameObjectWithTag(ZoneB);

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

                CPlayerInstance = Instantiate(PlayerPrefab, spawnPos, Quaternion.identity);
                var playerAction = CPlayerInstance.GetComponent<Action>();
                cam.SetNewHero(CPlayerInstance);
                playerAction.SetNewHero(CPlayerInstance);
                if (CEnemyInstance != null)
                {
                    playerAction.SetNewEnemy(CEnemyInstance);
                }

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
