using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player_Mouvement : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    public float speed = 5f;

    [Header("Paramètres d'escalade")]
    public float climbSpeed = 5f;
    public bool isOnLadder = false;
    public bool atBottomOfLadder = false;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Détection")]
    public Transform front;

    private Rigidbody rb;
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
        }
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        // Déplacement sol
        moveInput = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) moveInput += Vector3.forward;
        if (Keyboard.current.sKey.isPressed) moveInput += Vector3.back;
        if (Keyboard.current.aKey.isPressed) moveInput += Vector3.left;
        if (Keyboard.current.dKey.isPressed) moveInput += Vector3.right;
        moveInput.Normalize();

        // Input vertical sur échelle
        verticalInput = 0f;
        if (isOnLadder)
        {
            if (Keyboard.current.wKey.isPressed) verticalInput = 1f;
            else if (Keyboard.current.sKey.isPressed) verticalInput = -1f;
        }
    }

    private void FixedUpdate()
    {
        // ESCALADE
        if (isOnLadder)
        {
            rb.useGravity = false;

            Vector3 newPos = rb.position;
            newPos.y += verticalInput * climbSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);

            // Bloquer la rotation du joueur sur l'échelle
            rb.rotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
            return;
        }

        // SOL
        rb.useGravity = true;

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDirection = camForward * moveInput.z + camRight * moveInput.x;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);

            // Tourner le joueur seulement si il n'est pas sur l'échelle
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }
        else
        {
            // 🔥 ANTI-GLISSE : rester sur place sans utiliser rb.velocity
            rb.MovePosition(rb.position);
        }
    }

    // ------------------------------------------
    // FRONT (détection échelle)
    // ------------------------------------------
    public void SetFrontTouchingLadder(bool state)
    {
        isOnLadder = state;

        if (state)
            Debug.Log("FRONT touche l’échelle → escalade activée");
        else
            Debug.Log("FRONT quitte l’échelle → escalade désactivée");
    }

    // ------------------------------------------
    // COLLISIONS
    // ------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ground") && isOnLadder)
        {
            atBottomOfLadder = true;
            isOnLadder = false;
            rb.useGravity = true;
            Debug.Log("Bas de l'échelle détecté");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
            atBottomOfLadder = false;
    }
}
