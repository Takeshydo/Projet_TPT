using UnityEngine;

public class Spawning : MonoBehaviour
{
    //private bool fighter = true;

    [SerializeField] private string placement = "FighterPosition";
    [SerializeField] private string Zone = "Front";
    [SerializeField] private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject Position = GameObject.FindGameObjectWithTag(placement);
        GameObject FrontZone = GameObject.FindGameObjectWithTag(Zone);

        if (Position != null && FrontZone != null)
        {
            BoxCollider box = Position.GetComponent<BoxCollider>();
            Vector3 spawnPos = box.bounds.center;

            Instantiate(player, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Rien connard");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
