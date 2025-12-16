using System;
using UnityEngine;

public class Hero : MonoBehaviour
{

    public string HeroName = "Stark";
    public int HeroLevel = 2;
    public float MaxHP = 250f;
    public float currentHP = 250f;
    public int MaxXP = 100;
    public float currentXP = 0f;
    public float Force = 15f;
    public float Defense = 25f;
    public float Critique = 1.05f;
    public float speed = 10f;
    /*public float Precision = 100f;*/


    void Start()
    {

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
        currentHP -= DamageTaken;
    }

    public void LevelUp()
    {
        if(currentXP >= MaxXP)
        {
            HeroLevel += 1;
            MaxHP += 20f;
        }
    }
}
