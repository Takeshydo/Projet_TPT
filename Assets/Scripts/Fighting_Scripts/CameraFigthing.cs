using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFigthing : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Player;
    public Transform PlayerT;
    public Transform EnemyT;
    public float height = 2f;
    public float distance = 5f;
    public float smoothSpeed = 5f;

    public enum CameraMode
    {
        OnHero,
        OnEnemy,
    }

    public CameraMode DefaultMode = CameraMode.OnHero;
    void LateUpdate()
    {
        switch (DefaultMode)
        {
            case CameraMode.OnHero:
                OnHero();
                break;
            case CameraMode.OnEnemy:
                OnEnemy();
                break;
        }
    }


    private void OnHero()
    {
        if (!PlayerT || !EnemyT) return;
        Vector3 FightDirection = (EnemyT.position - PlayerT.position).normalized;

        Vector3 desiredPosition = PlayerT.position - FightDirection * distance + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        Vector3 LookTarget = PlayerT.position + FightDirection * 2f;
        transform.LookAt(LookTarget);
    }

    private float orbitAngle = 0f;
    private float orbitspeed = 20f;
    private void OnEnemy()
    {
        var distance = 15f;
        var height = 5f;
        if (!EnemyT) return;
        orbitAngle += orbitspeed * Time.deltaTime;
        if (orbitAngle > 360f)
        {
            orbitAngle -= 360f;
        }
        float rad = orbitAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)) * distance;
        Vector3 desiredPosition = EnemyT.position + offset + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.LookAt(EnemyT.position + Vector3.up * 1f);
    }
    public void SetNewEnemy(GameObject NewEnemyInstance)
    {
        Enemy = NewEnemyInstance;
        EnemyT = NewEnemyInstance.transform;
    }
    public void SetNewHero(GameObject NewHeroInstance)
    {
        Player = NewHeroInstance;
        PlayerT = NewHeroInstance.transform;
    }

}

