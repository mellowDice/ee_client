using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
  private Rigidbody body;
  public Camera camera;
  public float speed = 5f;
	// Use this for initialization
	void Start () {
    body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
    var radius = GetComponent<SphereCollider>().radius;
    camera.GetComponent<Transform>().position = GetComponent<Transform>().position + new Vector3(0,radius,0);
	  Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    Debug.Log(radius);
    body.AddForce(ray.direction * speed);
	}
}
