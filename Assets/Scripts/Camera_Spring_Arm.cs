using UnityEngine;

public class Camera_Spring_Arm : MonoBehaviour
{
    [Header("Target")]
    public Transform target;           // Player

    [Header("Spring Arm Settings")]
    public Vector3 cameraLocalPosition = new Vector3(0, 5, -20); // Position caméra au bout du bras
    public Vector3 offset = new Vector3(0, 2f, 0);              // Hauteur du bras
    public float rotationSpeed = 5f;
    public float minYAngle = -20f;
    public float maxYAngle = 60f;

    [Header("Collision Settings")]
    public LayerMask collisionLayers;  // Layers pour détecter obstacles
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

        // Création Spring Arm
        springArm = new GameObject("SpringArm").transform;

        // Caméra attachée au Spring Arm
        transform.SetParent(springArm);
        transform.localPosition = cameraLocalPosition;
        transform.localRotation = Quaternion.identity;

        defaultLocalPos = cameraLocalPosition;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Rotation orbitale Spring Arm
        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        springArm.position = target.position + offset;
        springArm.rotation = Quaternion.Euler(currentY, currentX, 0);

        // Position désirée caméra
        Vector3 desiredWorldPos = springArm.TransformPoint(defaultLocalPos);
        Vector3 dir = desiredWorldPos - springArm.position;
        float distance = dir.magnitude;

        RaycastHit hit;
        // SphereCast corrigé pour détecter collisions
        if (Physics.SphereCast(springArm.position, collisionRadius, dir.normalized, out hit, distance, collisionLayers, QueryTriggerInteraction.Ignore))
        {
            // Glisser la caméra vers le Player
            Vector3 targetPos = springArm.position + dir.normalized * Mathf.Max(hit.distance - collisionRadius, 0.1f);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }
        else
        {
            // Position normale
            transform.position = Vector3.Lerp(transform.position, desiredWorldPos, smoothSpeed * Time.deltaTime);
        }

        // Toujours regarder le Player
        transform.LookAt(target.position + offset);
    }
}
