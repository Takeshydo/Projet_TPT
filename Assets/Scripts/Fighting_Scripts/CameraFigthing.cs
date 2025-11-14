using TreeEditor;
using UnityEngine;

public class CameraFigthing : MonoBehaviour
{
    public Transform Player;
    public Transform Enemy;
    public Vector3 offset = new Vector3(0f, 2f, -4f);


    void LateUpdate()
    {
        if (Player == null && Enemy == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) { Player = p.transform; }
            GameObject e = GameObject.FindGameObjectWithTag("Enemy");
            if (e != null) { Enemy = e.transform; }
        }

        transform.position = Player.position + Player.rotation * offset;
        transform.LookAt(Enemy.position);
    }
}
