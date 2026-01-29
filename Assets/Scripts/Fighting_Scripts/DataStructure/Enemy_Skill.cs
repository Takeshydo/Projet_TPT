using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_Skill", menuName = "Scriptable Objects/Enemy_Skill")]
public class Enemy_Skill : ScriptableObject
{
    public string AttackName;
    public float Damage;
    public ZoneArea.ZoneType[] validZones;
    public float Proba = 1f;
}
