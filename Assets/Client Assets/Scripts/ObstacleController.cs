using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {

  string _id;
  public string id
    { get { return _id; } set { _id = value; } }

  void Start() {
    Ray ray = new Ray(transform.position, Vector3.down);
      RaycastHit hit;
      if(Physics.Raycast(ray, out hit)) {
        Debug.DrawLine(transform.position, hit.point, Color.green);
      }
    transform.position = new Vector3 (transform.position.x, hit.point.y + 1f, transform.position.z);
  }

  // void OnTriggerEnter (Collider other) {
  //   if (other.gameObject.CompareTag("Player")) {
  //     Debug.Log("blown to bits by " + _id);
  //     transform.parent.GetComponent<KnickKnackNetworkController>().ObstacleCollision(_id);
  //   }
  // }

}
