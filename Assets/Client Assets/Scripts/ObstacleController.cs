using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {

  public string id;

  void Start() {
    Ray ray = new Ray(transform.position, Vector3.down);
      RaycastHit hit;
      if(Physics.Raycast(ray, out hit, 1.5f)) {
        Debug.DrawLine(transform.position, hit.point, Color.green);
      }
    transform.position = new Vector3 (transform.position.x, hit.point.y + 1f, transform.position.z);
  }

  void OnTriggerEnter (Collider other) {
    if (other.gameObject.CompareTag("Player")) {
      Debug.Log("blown to bits by " + id);
      transform.parent.GetComponent<KnickKnackNetworkController>().ObstacleCollision(id);
    }
  }

}
