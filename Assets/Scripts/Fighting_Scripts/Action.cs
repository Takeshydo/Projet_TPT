using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEditor.PackageManager;

public class Action : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private string[] zoneOrder = { "Front", "Right", "Back", "Left" }; // ordre horaire
    [SerializeField] private string positionTag = "FighterPosition";

    private Enemy enemyScript;
    private Hero Herostat;
    public UI_Update_Info ui;

    private Dictionary<string, Transform> zonePositions = new Dictionary<string, Transform>();
    private int currentZoneIndex = 0;
    private ZoneArea.ZoneType currentZone;

    //Turn Base System
    public int PA = 1;
    public int actuelAction = 2;
    public int actionLeft;
    public bool cantMove = false;
    public bool cantAttack = false;
    public bool cantObject = false;
    public bool IsTurnFinished => actionLeft <= 0;
    private bool zoneReady = false;

    public List<Skills_Structure> equippedSkills = new List<Skills_Structure>();

    void Start()
    {
        // Récupère les positions des zones
        foreach (string zoneTag in zoneOrder)
        {
            GameObject zoneObj = GameObject.FindGameObjectWithTag(zoneTag);
            if (zoneObj != null)
            {
                Transform spawnPos = zoneObj.GetComponentsInChildren<Transform>()
                    .FirstOrDefault(t => t.CompareTag(positionTag));

                if (spawnPos != null)
                    zonePositions[zoneTag] = spawnPos;
            }
            else
            {
                Debug.LogWarning($"Zone introuvable : {zoneTag}");
            }
        }
        zoneReady = true;
    }

    void Update()
    {
        if (enemy != null && player != null)
        {
            Vector3 lookDir = enemy.transform.position - player.transform.position;
            lookDir.y = 0;
            player.transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    void MoveClockwise()
    {
        currentZoneIndex = (currentZoneIndex + 1) % zoneOrder.Length;
        MovePlayerTo(zoneOrder[currentZoneIndex]);
    }

    void MoveCounterClockwise()
    {
        currentZoneIndex = (currentZoneIndex - 1 + zoneOrder.Length) % zoneOrder.Length;
        MovePlayerTo(zoneOrder[currentZoneIndex]);
    }

    void MovePlayerTo(string zoneTag)
    {
        if (zonePositions.ContainsKey(zoneTag))
        {
            player.transform.position = zonePositions[zoneTag].position;
            Debug.Log($"Joueur déplacé vers {zoneTag}");
        }
        else
        {
            Debug.LogError($"Position introuvable pour la zone {zoneTag}");
        }
    }
    public void StartTurn()
    {
        actionLeft = PA * actuelAction;
        cantMove = false;
        cantAttack = false;
        cantObject = false;
    }

    public void EndTurn()
    {
        if (actionLeft == 0)
        {
            Debug.Log("Fin du tour");
        }

    }

    public DamageResult AttackAction(Skills_Structure skills)
    {
        int cost = 1;
        float finalDamage = skills.Damage;
        bool isCrit = false;

        if (enemyScript == null || Herostat == null) Debug.Log("Un des scripts est null");

        if (currentZone == skills.condition)
        {
            if (skills.Effets != "None")
            {
                enemyScript.status = skills.Effets;
            }
            if (skills.Critique == true)
            {
                finalDamage *= Herostat.Critique;
                isCrit = true;
            }
        }
        float DamageTaken = enemyScript.TakeDamage(finalDamage, currentZone);
        actionLeft -= cost;

        return new DamageResult { damage = DamageTaken, isCrit = isCrit };
    }
    public void ObjectAction(int cost = 1)
    {
        Debug.Log("Object utilisé");
        //Script inventaire pour object
        actionLeft -= cost;
    }

    public void MoveRight(int cost = 1)
    {
        if (!cantMove)
        {
            MoveClockwise();
            actionLeft -= cost;
        }
        return;
    }

    public void MoveLeft(int cost = 1)
    {
        if (!cantMove)
        {
            MoveCounterClockwise();
            actionLeft -= cost;
        }
        return;
    }

    //Detection de zone pour UI 

    public string GetCurrentZone() => zoneOrder[currentZoneIndex];

    public string GetRightZone()
    {
        int index = (currentZoneIndex + 1) % zoneOrder.Length;
        return zoneOrder[index];
    }
    public string GetLeftZone()
    {
        int index = (currentZoneIndex - 1 + zoneOrder.Length) % zoneOrder.Length;
        return zoneOrder[index];
    }

    private void OnTriggerEnter(Collider other)
    {
        // L'autre objet doit être IsTrigger = true
        if (other.CompareTag("Front") || other.CompareTag("Right") || other.CompareTag("Back") || other.CompareTag("Left"))
        {
            currentZone = ZoneArea.TagToZoneType(other.tag);
            Debug.Log($"Entré dans zone : {other.tag} => {currentZone}");
        }
    }

    public void SetNewEnemy(GameObject NewEnemyInstance)
    {
        enemy = NewEnemyInstance;
        if (enemy != null)
        {
            enemyScript = enemy.GetComponent<Enemy>();
        }
    }
    public void SetNewHero(GameObject NewHeroInstance)
    {
        player = NewHeroInstance;
        if (player != null)
        {
            Herostat = player.GetComponent<Hero>();
            TryToPlace();
        }
        else
        {
            Debug.Log("Player est null");
        }
    }

    public void TryToPlace()
    {
        if (!zoneReady || player == null) return;
        MovePlayerTo(zoneOrder[currentZoneIndex]);
    }

}
public struct DamageResult
{
    public float damage;
    public bool isCrit;
}


