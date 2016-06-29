using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
  private Rigidbody body;
  public Camera cam;
  public float speed = 5f;
  public float speedMultiplier = 1f;
  public bool computerControlled = false;
  Vector3 computerControlledDirection = new Vector3(1,0,0);
  Vector3 direction = new Vector3(1,0,0);

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
    if(playerAttributes.mainPlayer) {
      InvokeRepeating("mainPlayerUpdateDirection", 0f, 0.1f);
      InvokeRepeating("mainPlayerSendState", 0f, 0.1f);
    }
	}
	void UpdateComputerControlledDirection() {
    computerControlledDirection = Quaternion.AngleAxis(90.0f, Vector3.up) * computerControlledDirection;
  }
	// Update is called once per frame
	void FixedUpdate () {
    MoveInDirection(direction);  
	}
  void mainPlayerSendState() {
    var position = GetComponent<Transform>().position;
    var velocity = body.velocity;
    var angularVelocity = body.angularVelocity;
    netMove.PlayerStateReconcileSend(position, velocity, angularVelocity);
  }
  void mainPlayerUpdateDirection() {
    if(computerControlled) {
      direction = computerControlledDirection;
    }
    else {
      // setCameraPosition();
      direction = getDirection();     
    }
    netMove.Look(direction);
  }
  public void PlayerStateReconcileReceive(Vector3 position, Vector3 velocity, Vector3 angularVelocity) {
    var resetState = false;
    if(Vector3.Distance(body.velocity, velocity) > 1) {
      Debug.Log("VELOCITY MISMATCH:" + Vector3.Distance(body.velocity, velocity));
      resetState = true;
    }
    if(Vector3.Distance(body.angularVelocity, angularVelocity) > 1) {
      Debug.Log("ANGULAR VELOCITY MISMATCH:" + Vector3.Distance(body.angularVelocity, angularVelocity));
      resetState = true;
    }
    var bodyPosition = GetComponent<Transform>().position;
    if(Vector3.Distance(bodyPosition, position) > 1) {
      Debug.Log("POSITION MISMATCH:" + Vector3.Distance(bodyPosition, position));
      resetState = true;
    } 
    if(resetState) {      
      GetComponent<Transform>().position = position;
      body.velocity = velocity;
      body.angularVelocity = angularVelocity;
    }
  }
  public void UpdateDirectionFromNetwork(Vector3 networkDir) {
    direction = networkDir;
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
