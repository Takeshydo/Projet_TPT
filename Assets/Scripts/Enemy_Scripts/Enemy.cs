using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //Var Donnée du Monstre
    public string EnemyName = "Enemy1";
    public int EnemyLvl = 5;
    public float EnemyHP = 500.0f;
    public float Damage = 15.0f;
    public float Defense = 5.0f;
    public string status = "None";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float amount)
    {

    }
}
