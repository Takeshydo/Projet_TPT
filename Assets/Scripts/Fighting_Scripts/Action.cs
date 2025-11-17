using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class Action : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private string[] zoneOrder = { "Front", "Right", "Back", "Left" }; // ordre horaire
    [SerializeField] private string positionTag = "FighterPosition";

    private Dictionary<string, Transform> zonePositions = new Dictionary<string, Transform>();
    private int currentZoneIndex = 0;

    //Turn Base System
    public int PA = 1;
    public int actuelAction = 2;
    public int actionLeft;
    public bool cantMove = false;

    public bool isMyTurn = false;

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

        //Set UP action
        StartTurn();

        // Place le joueur en "Front"
        MovePlayerTo(zoneOrder[currentZoneIndex]);
    }

    void Update()
    {

        MoveAction();


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

    public void MoveAction(int cost = 1)
    {
        if (!cantMove)
        {
            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                MoveClockwise();
                cantMove = true;
            }
            else if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                MoveCounterClockwise();
                cantMove = true;
            }
        }

        actionLeft -= cost;
        //Debug.Log(actionLeft);
    }
    public void AttackAction(int cost = 1)
    {
        //Script compétence avec base de donné
        actionLeft -= cost;
    }
    public void ObjectAction(int cost = 1)
    {
        //Script inventaire pour object
        actionLeft -= cost;
    }
}

