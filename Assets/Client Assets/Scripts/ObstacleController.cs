using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObstacleController : MonoBehaviour {

  public GameObject obstaclePrefab;
  Dictionary<string, GameObject> obstacles;

  void CreateObstacle (Vector3 position, string id) {
    rb = GetComponent<Rigidbody();
    obstacles.Add(position, id)
    var obstacle = Instantiate(obstaclePrefab);
    var Quaternion rotation = Quaternion.Euler(0, 30, 0);
    rocketInstance = Instantiate(rocketPrefab, position, rotation) as rb;
  }

  void DestroyObject (Collider other) {
    // obstacle active state to false
    if (other.gameObject.CompareTag("Player")) {
      obstaclePrefab.SetActive(false);
    }
    // remove from obstacles object
    obstacles.Remove(id);
    var destroy = obstacles.GetComponent<NetworkDestroy>();
    destroy.OnDestroy()
  }

}