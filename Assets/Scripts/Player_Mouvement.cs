using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Mouvement : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    public float speed = 5f; // Vitesse du joueur

    void Update()
    {
        // Vérifie que le clavier est disponible
        if (Keyboard.current == null) return;

        // Vecteur de déplacement
        Vector3 move = Vector3.zero;

        // Détection des touches ZQSD (AZERTY)
        if (Keyboard.current.wKey.isPressed) move += Vector3.forward;  // Z = avancer
        if (Keyboard.current.sKey.isPressed) move += Vector3.back;     // S = reculer
        if (Keyboard.current.aKey.isPressed) move += Vector3.left;     // Q = gauche
        if (Keyboard.current.dKey.isPressed) move += Vector3.right;    // D = droite

        // Normalise pour éviter la vitesse diagonale trop grande
        move.Normalize();

        // Applique le déplacement
        transform.position += move * speed * Time.deltaTime;
    }
}
