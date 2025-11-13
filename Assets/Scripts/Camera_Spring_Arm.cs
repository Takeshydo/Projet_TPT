using UnityEngine;

public class Camera_Spring_Arm : MonoBehaviour
{
    [Header("Target")]
    public Transform target;           // Player

    [Header("Spring Arm Settings")]
    public float armLength = 7f;       // Distance caméra / bras
    public Vector3 offset = new Vector3(0, 2f, 0); // Hauteur du bras
    public float rotationSpeed = 5f;   // Sensibilité souris
    public float minYAngle = -20f;
    public float maxYAngle = 60f;

    private float currentX = 0f;
    private float currentY = 20f;
    private Transform springArm;

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

        // Crée le Spring Arm (bras invisible)
        springArm = new GameObject("SpringArm").transform;

        // Attache la caméra au Spring Arm
        transform.SetParent(springArm);
        transform.localPosition = new Vector3(0, 0, -armLength); // caméra au bout du bras
        transform.localRotation = Quaternion.identity;           // ne pivote pas
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Rotation orbitale du Spring Arm autour du Player via la souris
        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        // Position du Spring Arm = Player + offset
        springArm.position = target.position + offset;

        // Rotation du Spring Arm
        springArm.rotation = Quaternion.Euler(currentY, currentX, 0);

        // La caméra reste fixée au bout du bras
    }
}
