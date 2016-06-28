using UnityEngine;
using System.Collections;

public class EnableGravity : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 if (Input.anyKeyDown) {
    GetComponent<Rigidbody>().useGravity = true;
   }
	}
}
