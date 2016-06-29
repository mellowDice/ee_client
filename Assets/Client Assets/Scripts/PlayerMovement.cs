using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
  private Rigidbody body;
  public Camera cam;
  public float speed = 5f;
  public float speedMultiplier = 1f;
  public bool computerControlled = false;
  Vector3 computerControlledDirection = new Vector3(1,0,0);

  PlayerAttributes playerAttributes;
  private static NetworkMove netMove;

	// Use this for initialization
	public virtual void Start () {
    body = GetComponent<Rigidbody>();
    playerAttributes = GetComponent<PlayerAttributes>();
    if(playerAttributes.mainPlayer) {
      netMove = GetComponent<NetworkMove>();
    }
    if(computerControlled) {
      InvokeRepeating("UpdateComputerControlledDirection", 2.5f, 5f);
    }
	}
	void UpdateComputerControlledDirection() {
    computerControlledDirection = Quaternion.AngleAxis(90.0f, Vector3.up) * computerControlledDirection;
  }
	// Update is called once per frame
	void FixedUpdate () {
    if(computerControlled) {
      netMove.Look(computerControlledDirection);
      MoveInDirection(computerControlledDirection);

    } else if(playerAttributes.mainPlayer) {
      // setCameraPosition();
  	  var direction = getDirection();
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
