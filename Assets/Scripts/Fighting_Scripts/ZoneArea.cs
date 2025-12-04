using UnityEngine;
using System.Collections.Generic;

public class ZoneArea : MonoBehaviour
{
    public enum ZoneType { Front, Flank, Back };
    private static readonly Dictionary<string, ZoneType> tagToZone = new Dictionary<string, ZoneType>()
    {
        {"Front", ZoneType.Front},
        {"Right", ZoneType.Flank},
        {"Back", ZoneType.Back},
        {"Left", ZoneType.Flank}
    };

    public static ZoneType TagToZoneType(string tag)
    {
        if (tagToZone.TryGetValue(tag, out ZoneType zone))
            return zone;

        Debug.LogWarning($"Tag inconnu pour une zone : {tag}");
        return ZoneType.Front; // fallback
    }

}
