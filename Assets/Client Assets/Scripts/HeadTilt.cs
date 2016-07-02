using UnityEngine;
using System.Collections;

public class HeadTilt : MonoBehaviour {

	// Use this for initialization
	void Start () {
	  
	}
	
	// Update is called once per frame
	void Update () {
    var tilt = Mathf.Clamp(transform.rotation.z*180,-45,45);
    transform.parent.transform.rotation = Quaternion.Euler(0,-tilt,0);
	}
}
