using UnityEngine;
using UnityEngine.InputSystem;

public class Monted : MonoBehaviour
{
    [Header("Paramètres de la montée")]
    public float climbSpeed = 3f;       // Vitesse de montée
    public float mountHeight = 1.5f;    // Hauteur finale du joueur une fois monté

    private bool canMount = false;      // Le joueur peut activer la montée
    private bool isMounting = false;    // La montée est en cours
    private bool isMounted = false;     // Le joueur est monté
    private Transform player;           // Référence du joueur

    private void Update()
    {
        if (Keyboard.current == null || player == null)
            return;

        // Activation de la montée avec E
        if (canMount && !isMounted && Keyboard.current.eKey.wasPressedThisFrame)
        {
            isMounting = true;
            Debug.Log("Montée activée. Appuie sur Z pour monter.");
        }

        // Montée progressive avec Z
        if (isMounting && Keyboard.current.zKey.isPressed)
        {
            Vector3 targetPos = new Vector3(player.position.x, transform.position.y + mountHeight, player.position.z);
            player.position = Vector3.MoveTowards(player.position, targetPos, climbSpeed * Time.deltaTime);

            // Vérifie si le joueur a atteint la hauteur cible
            if (Vector3.Distance(player.position, targetPos) < 0.05f)
            {
                FinishMount();
            }
        }

        // Descente avec E
        if (isMounted && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Dismount();
        }
    }

    private void FinishMount()
    {
        isMounting = false;
        isMounted = true;

        // Fixe le joueur sur la monture
        player.SetParent(transform);
        player.localPosition = new Vector3(0, mountHeight, 0);
        player.localRotation = Quaternion.identity;

        Debug.Log("Le joueur est monté.");
    }

    private void Dismount()
    {
        isMounted = false;
        isMounting = false;

        player.SetParent(null);
        Debug.Log("Le joueur est descendu.");
    }

    private void OnTriggerStay(Collider other)
    {
        // Tant que le joueur reste dans le trigger, il peut monter
        if (other.CompareTag("Player"))
        {
            canMount = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canMount = false;

            // Si le joueur n’est pas encore monté, on réinitialise la référence
            if (!isMounted)
                player = null;

            // Arrête la montée si elle était en cours
            isMounting = false;

            Debug.Log("Le joueur s'est éloigné, montée annulée.");
        }
    }
}
