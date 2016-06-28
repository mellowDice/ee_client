using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
	void Update() {
    if (Input.GetButton("Fire1") && GetComponent<Rigidbody>().mass > 1) {
      GetComponent<PlayerMovement>().speed = 50f;
      GetComponent<Rigidbody>().mass -= 0.1f * Time.deltaTime;
      GetComponent<Transform>().localScale -= new Vector3(0.02f * Time.deltaTime,0.02f * Time.deltaTime,0.02f * Time.deltaTime);
    } else {
      GetComponent<PlayerMovement>().speed = 5f;
    }
  }
	void OnTriggerEnter(Collider other) {
    Destroy(other.transform.parent.gameObject);
    GetComponent<Rigidbody>().mass += 0.5f;
    GetComponent<Transform>().localScale += new Vector3(0.1f,0.1f,0.1f);
    //Send emit for death to network
  }
}
