using UnityEngine;
using UnityEngine.InputSystem;

public class Monted : MonoBehaviour
{
    [Header("Paramètres de la montée")]
    public float climbSpeed = 3f;   // vitesse de montée

    private bool isOnLadder = false;  // le joueur est en contact
    private Transform player;

    void Update()
    {
        if (player == null || Keyboard.current == null)
            return;

        // Le joueur veut monter : maintenir Z
        if (isOnLadder && Keyboard.current.wKey.isPressed)
        {
            // monte verticalement
            player.position += Vector3.up * climbSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOnLadder = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOnLadder = false;
            player = null;
        }
    }
}
