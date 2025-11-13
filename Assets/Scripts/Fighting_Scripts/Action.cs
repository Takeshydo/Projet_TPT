using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class Action : MonoBehaviour
{
    [SerializeField] private GameObject player; // Ton joueur déjà dans la scène
    [SerializeField] private string[] zoneTags = { "Right", "Left", "Front", "Back" };

    private Dictionary<string, Transform[]> zonePositions = new Dictionary<string, Transform[]>();

    void Start()
    {
        // Récupère toutes les positions enfants pour chaque zone
        foreach (var zoneTag in zoneTags)
        {
            GameObject zoneObj = GameObject.FindGameObjectWithTag(zoneTag);
            if (zoneObj != null)
            {
                Transform[] positions = zoneObj.GetComponentsInChildren<Transform>()
                    .Where(t => t.CompareTag("FighterPosition"))
                    .ToArray();

                zonePositions[zoneTag] = positions;
            }
            else
            {
                zonePositions[zoneTag] = new Transform[0];
                Debug.LogWarning("Aucune zone trouvée avec le tag : " + zoneTag);
            }
        }
    }

    void Update()
    {
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            TeleportToZone("Right");
        }
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            TeleportToZone("Left");
        }
    }

    private void TeleportToZone(string zoneTag)
    {
        if (player == null)
        {
            Debug.LogError("Pas de joueur assigné !");
            return;
        }

        if (zonePositions.ContainsKey(zoneTag) && zonePositions[zoneTag].Length > 0)
        {
            Transform spawnPoint = zonePositions[zoneTag][0]; // tu peux randomiser plus tard
            player.transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogError("Aucune position trouvée pour la zone : " + zoneTag);
        }
    }
}
