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

    private Dictionary<string, Transform> zonePositions = new Dictionary<string, Transform>();
    private int currentZoneIndex = 0;
    private ZoneArea.ZoneType currentZone;

    //Turn Base System
    public int PA = 1;
    public int actuelAction = 2;
    public int actionLeft;
    public bool cantMove = false;

    public bool isMyTurn = false;

    void Start()
    {
        if (enemy != null)
        {
            enemyScript = enemy.GetComponent<Enemy>();
        }
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

        //Set UP action
        StartTurn();

        // Place le joueur en "Front"
        MovePlayerTo(zoneOrder[currentZoneIndex]);
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
    void StartTurn()
    {
        actionLeft = PA * actuelAction;
        isMyTurn = true;
        cantMove = false;
    }

    public void AttackAction(Skills_Structure skills)
    {
        int cost = 1;
        if (!isMyTurn) return;

        if (skills.Damage > 0)
        {
            enemyScript.TakeDamage(skills.Damage);
        }

        if (currentZoneIndex == 1 || currentZoneIndex == 3)
        {
            enemyScript.status = skills.Effets;
        }


        actionLeft -= cost;
    }
    public void ObjectAction(int cost = 1)
    {
        //Script inventaire pour object
        actionLeft -= cost;
    }

    public void MoveRight(int cost = 1)
    {
        if (!cantMove)
        {
            MoveClockwise();
            cantMove = true;
            actionLeft -= cost;
        }
        return;
    }

    public void MoveLeft(int cost = 1)
    {
        if (!cantMove)
        {
            MoveCounterClockwise();
            cantMove = true;
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

}

