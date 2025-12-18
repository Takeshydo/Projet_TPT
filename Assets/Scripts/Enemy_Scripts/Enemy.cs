using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Var Donnée du Monstre
    public string EnemyName = "Enemy1";
    public int EnemyLvl = 5;
    private float EnemyHP = 500.0f;
    public float CEnemyHP;
    public float Damage = 15.0f;
    public float Defense = 5.0f;
    public string status = "None";
    public bool IsDead = false;
    void Start()
    {
        CEnemyHP = EnemyHP;
        status = "None";
        Debug.Log($"Ennemi instancié. HP réinitialisés à {CEnemyHP}");
        
    }

    public void TakeDamage(float amount)
    {
        float reductionratio = Defense /100; // Passage defense en % 
        reductionratio = Mathf.Min(reductionratio, 1.0f); //Verif du ration < 100%
        float DamageTaken = amount * (1f - reductionratio); //Dégat avec ratio def
        DamageTaken = Mathf.Max(DamageTaken, 0f); //Verif du ration > 0
        CEnemyHP -= DamageTaken;
        Debug.Log("Enemy HP"+ CEnemyHP+"");
    }

    public void Death()
    {
        if(CEnemyHP == 0)
        {
            IsDead = true; 
            Debug.Log ("L'enemy est mort");
        }
    }
}
