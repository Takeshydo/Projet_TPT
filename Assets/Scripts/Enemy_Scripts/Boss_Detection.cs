using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_Detection : MonoBehaviour
{
  public GameObject Prefabs;
  public EnemyID enemyID;

  private void OnTriggerEnter(Collider other)
  {
    if (!other.CompareTag("Player")) return;

    if (Prefabs.scene.name != null)
    {
      Debug.LogError("enemyPrefab doit être un asset !");
      return;
    }
    GameManagement.Instance.CurrentenemyID = enemyID.enemyID;
    GameManagement.Instance.SelectedPrefabs = Prefabs;
    SceneManager.LoadScene("Arena");
  }


}
