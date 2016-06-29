using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
  private Rigidbody body;
  public Camera cam;
  public float speed = 5f;
  public float speedMultiplier = 1f;
  PlayerAttributes playerAttributes;
  private static NetworkMove netMove;

	// Use this for initialization
	public virtual void Start () {
    body = GetComponent<Rigidbody>();
    playerAttributes = GetComponent<PlayerAttributes>();
    if(playerAttributes.mainPlayer) {
      netMove = GetComponent<NetworkMove>();
    }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
    if(playerAttributes.mainPlayer) {
      setCameraPosition();
  	  var direction = getDirection();
      netMove.Look(direction);
      MoveInDirection(direction);
    }
	}
  public virtual Vector3 getDirection() {
    var ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    return ray.direction;
  }
  public virtual void setCameraPosition() {
      var radius = GetComponent<SphereCollider>().radius;
      cam.GetComponent<Transform>().position = GetComponent<Transform>().position + new Vector3(0,radius + 0.5f,0);    
  }
  public void MoveInDirection(Vector3 direction) {
    body.AddForce(direction * speed * speedMultiplier);
  }
}
