using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player_Mouvement : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    public float speed = 5f;

    [Header("Paramètres d'escalade")]
    public float climbSpeed = 20f; // 6f = vitesse
    private bool isOnLadder = false;

    [Header("Camera")]
    public Transform cameraTransform;

    private Rigidbody rb;

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

        if (isOnLadder)
        {
            HandleLadderMovement();
        }
        else
        {
            HandleNormalMovement();
        }
    }

    private void HandleNormalMovement()
    {
        Vector3 moveInput = Vector3.zero;

        if (Keyboard.current.wKey.isPressed) moveInput += Vector3.forward;
        if (Keyboard.current.sKey.isPressed) moveInput += Vector3.back;
        if (Keyboard.current.aKey.isPressed) moveInput += Vector3.left;
        if (Keyboard.current.dKey.isPressed) moveInput += Vector3.right;

        moveInput.Normalize();

        if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = cameraTransform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 moveDirection = camForward * moveInput.z + camRight * moveInput.x;

            Vector3 targetVelocity = moveDirection * speed;
            targetVelocity.y = rb.linearVelocity.y; // conserve la vélocité verticale
            rb.linearVelocity = targetVelocity;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        rb.useGravity = true;
    }

    private void HandleLadderMovement()
    {
        float verticalInput = 0f;

        if (Keyboard.current.wKey.isPressed)
            verticalInput = 1f;
        else if (Keyboard.current.sKey.isPressed)
            verticalInput = -1f;

        Vector3 climbVelocity = Vector3.up * verticalInput * climbSpeed;
        rb.linearVelocity = climbVelocity;

        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ladder"))
        {
            isOnLadder = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ladder"))
        {
            isOnLadder = false;

            if (!IsGrounded())
                rb.useGravity = true;
        }
    }

    private bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.down * 0.1f, 0.1f);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("ground"))
                return true;
        }
        return false;
    }
}
