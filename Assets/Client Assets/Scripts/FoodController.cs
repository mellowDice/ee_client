using UnityEngine;
using System.Collections;

public class FoodController : MonoBehaviour {
  public string _id;
  public string id
    { get { return _id; } set { _id = value; } }

  void Start () {
    NewPos();
  }

  public void NewPos () {
    Ray ray = new Ray(transform.position, Vector3.down);
    RaycastHit hit;
      if(Physics.Raycast(ray, out hit)) {
        Debug.DrawLine(transform.position, hit.point, Color.green);
      }
    transform.position = new Vector3 (transform.position.x, hit.point.y + 1f, transform.position.z);
  }

  // void OnTriggerEnter (Collider other) {
  //   if (other.gameObject.CompareTag("Player")) {
  //     Debug.Log("Eating all day ery'day");
  //     transform.parent.GetComponent<KnickKnackNetworkController>().FoodEaten(_id);
  //   }
  // }
}