using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
  private Rigidbody body;
  public Camera camera;
  public bool mainPlayer = false;
  public float speed = 5f;
  private static NetworkMove netMove;

	// Use this for initialization
	void Start () {
    body = GetComponent<Rigidbody>();
    if(mainPlayer) {
      netMove = GetComponent<NetworkMove>();
    }
	}
	
	// Update is called once per frame
	void Update () {
    if(mainPlayer) {
      var radius = GetComponent<SphereCollider>().radius;
      camera.GetComponent<Transform>().position = GetComponent<Transform>().position + new Vector3(0,radius,0);
  	  Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
      // Debug.Log(radius);
      netMove.Look(ray.direction);
      MoveInDirection(ray.direction);
    }
	}
  public void MoveInDirection(Vector3 direction) {
    body.AddForce(direction * speed);
  }
}
