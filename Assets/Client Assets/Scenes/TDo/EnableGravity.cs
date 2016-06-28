using UnityEngine;
using System.Collections;

public class EnableGravity : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  if(transform.position.y < -10) {
	  	Destroy(transform.gameObject);
	  }
	}
}
