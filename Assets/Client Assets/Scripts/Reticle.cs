using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {
  public Camera mainCam;
  private Vector3 scale;

  void Start () {
  	scale = transform.localScale;	
  }

  void Update () {
	float distance;
	RaycastHit hit = new RaycastHit ();
	if (Physics.Raycast (new Ray (mainCam.transform.position, mainCam.transform.rotation * Vector3.forward), out hit) && (hit.collider.name != "Player")) {
		distance = hit.distance;
	} else { 
	  distance = mainCam.farClipPlane * .95f;
	}
	transform.position = mainCam.transform.position + mainCam.transform.rotation * Vector3.forward * distance;
	transform.LookAt (mainCam.transform.position);
	transform.Rotate (0f, 180f, 0f);
	transform.localScale = scale * distance;

  }
}
