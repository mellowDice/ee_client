using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider other) {
    Destroy(other.transform.parent.gameObject);
    //Send emit for death to network
  }
}
