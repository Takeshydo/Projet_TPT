using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Var Donnée du Monstre
    public string EnemyName = "Enemy1";
    public int EnemyLvl = 5;
    public float EnemyHP = 10.0f;
    public float EnemyXP = 20f;
    [SerializeField] private float CEnemyHP;
    public float Damage = 5.0f;
    public float Defense = 5.0f;
    public string status = "None";
    public float speed = 5f;

    public event System.Action<Enemy> OnDeath;

    private bool EnemyIsDead => CEnemyHP <= 0;
    void Start()
    {
        CEnemyHP = EnemyHP;
        status = "None";
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TakeDamage(float amount)
    {
        float reductionratio = Defense / 100; // Passage defense en % 
        reductionratio = Mathf.Min(reductionratio, 1.0f); //Verif du ration < 100%
        float DamageTaken = amount * (1f - reductionratio); //Dégat avec ratio def
        DamageTaken = Mathf.Max(DamageTaken, 0f); //Verif du ration > 0
        CEnemyHP -= DamageTaken;
        Debug.Log("Enemy HP" + CEnemyHP + "");
        if (CEnemyHP <= 0)
        {
            Die();
        }
    }

    public void StartTurn()
    {
        Debug.Log("Début du tour de l'enemie");
    }

    void Die()
    {
        Debug.Log("Enemie est mort");
        OnDeath?.Invoke(this);
    }
}
