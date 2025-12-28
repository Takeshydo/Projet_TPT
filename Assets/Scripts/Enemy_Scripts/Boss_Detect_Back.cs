using UnityEngine;

public class Boss_Detect_Back : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManagement.Instance.enteredFromBack = true;
        }
    }
}
