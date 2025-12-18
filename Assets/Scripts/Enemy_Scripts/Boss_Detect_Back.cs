using UnityEngine;

public class Boss_Detect_Back : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jte prend par deriere");

            if (GameManagement.Instance != null)
            {
                GameManagement.Instance.enteredFromBack = true;
            }
        }
    }
}
