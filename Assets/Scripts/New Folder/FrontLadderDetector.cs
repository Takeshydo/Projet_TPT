using UnityEngine;

public class FrontLadderDetector : MonoBehaviour
{
    public Player_Mouvement player; // Assigner ton Player ici

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ladder"))
            player.SetFrontTouchingLadder(true); // Active l’escalade
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ladder"))
            player.SetFrontTouchingLadder(false); // Désactive l’escalade
    }
}
