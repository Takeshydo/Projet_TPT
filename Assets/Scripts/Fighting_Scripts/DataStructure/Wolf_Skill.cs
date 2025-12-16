using UnityEngine;

[CreateAssetMenu(fileName = "Wolf_Skill", menuName = "Scriptable Objects/Wolf_Skill")]
public class Wolf_Skill : ScriptableObject
{
    public string AttackName;
    public float Damage;
    public ZoneArea.ZoneType[] validZones;
    public float Proba = 1f;
}
