using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player_Mouvement : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    public float speed = 5f; // Vitesse au sol

    [Header("Paramètres d'escalade")]
    public float climbSpeed = 5f; // Vitesse d'escalade
    private bool isOnLadder = false; // Le joueur touche une échelle
    private bool atBottomOfLadder = false; // Arrivé en bas de l'échelle

    [Header("Camera")]
    public Transform cameraTransform; // Référence à la caméra

    private Rigidbody rb;

    // Inputs stockés pour FixedUpdate
    private Vector3 moveInput;
    private float verticalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
                cameraTransform = cam.transform;
            else
                Debug.LogError("Aucune caméra avec le tag MainCamera trouvée.");
        }
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        // Récupération des inputs horizontaux
        moveInput = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) moveInput += Vector3.forward;
        if (Keyboard.current.sKey.isPressed) moveInput += Vector3.back;
        if (Keyboard.current.aKey.isPressed) moveInput += Vector3.left;
        if (Keyboard.current.dKey.isPressed) moveInput += Vector3.right;
        moveInput.Normalize();

        // Récupération de l'input vertical si sur échelle
        verticalInput = 0f;
        if (isOnLadder)
        {
            if (Keyboard.current.wKey.isPressed) verticalInput = 1f;
            else if (Keyboard.current.sKey.isPressed) verticalInput = -1f;
        }
    }

    private void FixedUpdate()
    {
        if (isOnLadder)
        {
            // Escalade : X/Z bloqués, Y contrôlé
            Vector3 vel = rb.linearVelocity;
            vel.x = 0f;
            vel.z = 0f;
            vel.y = verticalInput * climbSpeed;
            rb.linearVelocity = vel;

            Debug.Log("Escalade, verticalInput=" + verticalInput + ", linearVelocity=" + rb.linearVelocity);
        }
        else
        {
            // Déplacement horizontal
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();
            Vector3 camRight = cameraTransform.right;
            camRight.y = 0f;
            camRight.Normalize();

            Vector3 moveDirection = camForward * moveInput.z + camRight * moveInput.x;

            // Bloquer horizontal si arrivé en bas de l'échelle et S n'est pas appuyé
            if (atBottomOfLadder && !Keyboard.current.sKey.isPressed)
            {
                moveDirection = Vector3.zero;
            }

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Vector3 targetPos = rb.position + moveDirection * speed * Time.fixedDeltaTime;
                rb.MovePosition(targetPos);
            }

            rb.useGravity = true;

            // Débloquer horizontal si S est appuyé pour descendre
            if (atBottomOfLadder && Keyboard.current.sKey.isPressed)
            {
                atBottomOfLadder = false;
                Debug.Log("S pressé, mouvement horizontal débloqué");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Début de l'escalade
        if (other.CompareTag("ladder"))
        {
            isOnLadder = true;
            atBottomOfLadder = false;
            Debug.Log("Entré dans l'échelle, mouvement horizontal bloqué");
        }

        // Détection haut de l'échelle
        if (other.CompareTag("floor") && isOnLadder)
        {
            isOnLadder = false;
            atBottomOfLadder = false;
            rb.useGravity = true;

            // Réinitialiser la vélocité verticale pour ne pas s'envoler
            Vector3 vel = rb.linearVelocity;
            vel.y = 0f;
            rb.linearVelocity = vel;

            Debug.Log("Haut de l'échelle détecté, mouvement horizontal réactivé");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ladder"))
        {
            isOnLadder = false;
            Debug.Log("Sorti de l'échelle");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Arrivé en bas de l'échelle
        if (collision.collider.CompareTag("ground") && isOnLadder)
        {
            atBottomOfLadder = true;
            isOnLadder = false; // Sort de l'échelle
            Debug.Log("Arrivé en bas de l'échelle, mouvement horizontal bloqué");
        }
    }
}
