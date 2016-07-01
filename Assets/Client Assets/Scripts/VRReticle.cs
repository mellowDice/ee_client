using UnityEngine;
using System.Collections;

public class VRReticle : MonoBehaviour {
  Camera camera;
  private Vector3 scale;

  void Awake () {
    camera = transform.parent.GetComponent<Camera>();
  }

  void Start () {
  	scale = transform.localScale;	
  }

  void Update () {
	float distance;
	RaycastHit hit = new RaycastHit ();
	if (Physics.Raycast (new Ray (camera.transform.position, camera.transform.rotation * Vector3.forward), out hit) && (hit.collider.name != "Player")) {
		distance = hit.distance;
	} else { 
	  distance = camera.farClipPlane * .95f;
	}
	transform.position = camera.transform.position + camera.transform.rotation * Vector3.forward * distance;
	transform.LookAt (camera.transform.position);
	transform.Rotate (0f, 180f, 0f);
	transform.localScale = scale * distance;

  }
}
