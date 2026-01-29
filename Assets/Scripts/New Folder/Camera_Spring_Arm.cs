using UnityEngine;

public class Camera_Spring_Arm : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public bool rotatePlayerWithCamera = true;

    [Header("Spring Arm Settings")]
    public Vector3 cameraLocalPosition = new Vector3(0, 5, -20);
    public Vector3 offset = new Vector3(0, 2f, 0);
    public float rotationSpeed = 5f;
    public float minYAngle = -20f;
    public float maxYAngle = 60f;

    [Header("Collision Settings")]
    public LayerMask collisionLayers;
    public float collisionRadius = 0.5f;
    public float smoothSpeed = 10f;

    private float currentX = 0f;
    private float currentY = 20f;
    private Transform springArm;
    private Vector3 defaultLocalPos;

    void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
                target = playerObj.transform;
            else
                Debug.LogWarning("Player not found!");
        }

        springArm = new GameObject("SpringArm").transform;

        transform.SetParent(springArm);
        transform.localPosition = cameraLocalPosition;
        transform.localRotation = Quaternion.identity;

        defaultLocalPos = cameraLocalPosition;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Rotation caméra
        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        // Position du bras
        springArm.position = target.position + offset;
        springArm.rotation = Quaternion.Euler(currentY, currentX, 0);

        // Calcul de la position désirée
        Vector3 desiredWorldPos = springArm.TransformPoint(defaultLocalPos);
        Vector3 dir = desiredWorldPos - springArm.position;
        float distance = dir.magnitude;

        // Collision caméra
        if (Physics.SphereCast(springArm.position, collisionRadius, dir.normalized, out RaycastHit hit, distance, collisionLayers))
        {
            Vector3 targetPos = springArm.position + dir.normalized * Mathf.Max(hit.distance - collisionRadius, 0.1f);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, desiredWorldPos, smoothSpeed * Time.deltaTime);
        }

        // Tourner le joueur seulement si il n'est pas sur l'échelle
        if (target.TryGetComponent<Player_Mouvement>(out var player))
        {
            if (!player.isOnLadder && rotatePlayerWithCamera)
            {
                Vector3 forward = springArm.forward;
                forward.y = 0;
                target.rotation = Quaternion.Slerp(
                    target.rotation,
                    Quaternion.LookRotation(forward),
                    10f * Time.deltaTime
                );
            }
        }

        transform.LookAt(target.position + offset);
    }
}
