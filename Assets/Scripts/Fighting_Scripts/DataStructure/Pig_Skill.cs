using UnityEngine;

[CreateAssetMenu(fileName = "Pig_Skill", menuName = "Scriptable Objects/Pig_Skill")]
public class Pig_Skill : ScriptableObject
{
    public string AttackName;
    public float Damage;
    public ZoneArea.ZoneType[] validZones;
    public float Proba = 1f;
}
