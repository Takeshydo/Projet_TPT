using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_Detection : MonoBehaviour
{  
 public GameObject Prefabs;

  private void OnTriggerEnter(Collider other)
  {
    if (!other.CompareTag("Player")) return;

    if (Prefabs.scene.name != null)
    {
      Debug.LogError("enemyPrefab doit être un asset !");
      return;
    }

    GameManagement.Instance.SelectedPrefabs = Prefabs;
    SceneManager.LoadScene("Arena");
  }


}
