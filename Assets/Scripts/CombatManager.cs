using UnityEngine;
using System.Collections;
using NUnit.Framework;

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
    public CombatState currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemy();
        /*if (GameManagement.Instance != null)
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
        }*/
        SpawnHeroF();
        WhoStart();
    }

    void Update()
    {
        if (currentState == CombatState.PlayerTurn)
        {
            var action = CPlayerInstance.GetComponent<Action>();
            if (action.IsTurnFinished)
            {
                EnemyTurn();
            }
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
            Enemy enemy = CEnemyInstance.GetComponent<Enemy>();
            enemy.OnDeath += HandleEnemyDeath;


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
                var enemy = CEnemyInstance.GetComponent<Enemy>();
                cam.SetNewHero(CPlayerInstance);
                playerAction.SetNewHero(CPlayerInstance);
                if (CEnemyInstance != null)
                {
                    playerAction.SetNewEnemy(CEnemyInstance);
                    enemy.SetNewHero(playerAction);
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
                var enemy = CEnemyInstance.GetComponent<Enemy>();
                cam.SetNewHero(CPlayerInstance);
                playerAction.SetNewHero(CPlayerInstance);
                if (CEnemyInstance != null)
                {
                    playerAction.SetNewEnemy(CEnemyInstance);
                    enemy.SetNewHero(playerAction);

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


    //System TPT c'est parti !!!

    public enum CombatState
    {
        PlayerTurn,
        EnemyTurn,
        Busy,
    }

    public void WhoStart()
    {
        var SpeedPlayer = CPlayerInstance.GetComponent<Hero>();
        var SpeedEnemy = CEnemyInstance.GetComponent<Enemy>();

        if (SpeedEnemy.speed > SpeedPlayer.speed)
        {
            EnemyTurn();
        }
        if (SpeedEnemy.speed < SpeedPlayer.speed)
        {
            PlayerTurn();
        }
    }

    public void PlayerTurn()
    {
        currentState = CombatState.PlayerTurn;
        var action = CPlayerInstance.GetComponent<Action>();
        action.StartTurn();

        ui.ActionMenu();
    }

    public void EnemyTurn()
    {
        currentState = CombatState.EnemyTurn;
        Enemy enemy = CEnemyInstance.GetComponent<Enemy>();
        StartCoroutine(EnemyRoutine(enemy));

    }

    public void IsDead()
    {

    }

    void HandleEnemyDeath(Enemy enemy)
    {
        Debug.Log("CombatManager : Enemi Vaincu");
        currentState = CombatState.Busy;

        ui.gameObject.SetActive(false);
        StartCoroutine(EndCombatRoutine(enemy));
    }

    IEnumerator EndCombatRoutine(Enemy enemy)
    {
        yield return new WaitForSeconds(1f);

        Destroy(enemy.gameObject);

        // XP / Loot / Animation ici

        Debug.Log("Combat terminé");

        // Retour monde / prochain combat
    }

    IEnumerator EnemyRoutine(Enemy enemy)
    {
        enemy.StartTurn();
        while (!enemy.hasFinished)
            yield return null;

        PlayerTurn();
    }
}
