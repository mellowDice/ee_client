using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
  public Camera cam;

  private Rigidbody body;

  public float speed = 5f;
  public float speedMultiplier = 1f;
  public bool computerControlled = false;
  Vector3 computerControlledDirection = new Vector3(1,0,0);
  Vector3 direction = new Vector3(1,0,0);

  PlayerAttributes playerAttributes;
  private static PlayerNetworkController netMove;


  ////////////////////////////
  // Unity Built-in Methods //
  ////////////////////////////

	// START: Initialization
	public virtual void Start () {
    body = GetComponent<Rigidbody>();
    playerAttributes = GetComponent<PlayerAttributes>();
    if(playerAttributes.mainPlayer) {
      netMove = GetComponent<PlayerNetworkController>();
    }
    if(computerControlled) {
      InvokeRepeating("UpdateComputerControlledDirection", 2.5f, 5f);
    }
    if(playerAttributes.mainPlayer) {
      InvokeRepeating("UpdateAndSendPlayerDirection", 0f, 0.1f);
      InvokeRepeating("SendMainPlayerState", 0f, 0.1f);
    }
	}

  // FIXED UPDATE: Before Every Physics Calculation, add force to player
  void FixedUpdate () {
    body.AddForce(direction * speed * speedMultiplier); 
  }


  /////////////////////////////////////
  // Methods Relating to Main Player //
  /////////////////////////////////////

  // UPDATE AND SEND PLAYER DIRECTION: Update player direction based on where camera is facing
  void UpdateAndSendPlayerDirection() {
    var camDirection = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).direction;
    direction = computerControlled ? computerControlledDirection : camDirection;
    netMove.Look(direction);
  }

  // SEND MAIN PLAYER STATE: Send server current position/velocity of player
  void SendMainPlayerState() {
    var position = GetComponent<Transform>().position;
    var velocity = body.velocity;
    var angularVelocity = body.angularVelocity;
    netMove.PlayerStateSend(position, velocity, angularVelocity);
  }

  // SET CAMERA POSITION: Keeps camera situated above ball
  public virtual void setCameraPosition() {
      var radius = GetComponent<SphereCollider>().radius;
      cam.GetComponent<Transform>().position = GetComponent<Transform>().position + new Vector3(0,radius,0);    
  }

  // UPDATE COMPUTER CONTROLLED DIRECTION: For debugging - has player move in circles instead following camera
	void UpdateComputerControlledDirection() {
    computerControlledDirection = Quaternion.AngleAxis(90.0f, Vector3.up) * computerControlledDirection;
  }


  ///////////////////////////////////////
  // Methods Relating to Other Players //
  ///////////////////////////////////////

  // RECEIVE PLAYER DIRECTION: Receive direction player is currently facing
  public void ReceivePlayerDirection(Vector3 networkDir) {
    direction = networkDir;
  }

  // RECEIVE PLAYER STATE: Receive updated position/velocity from server and compare
  public void ReceivePlayerState(Vector3 position, Vector3 velocity, Vector3 angularVelocity) {
    var resetState = false;
    var bodyPosition = GetComponent<Transform>().position;
    if(Vector3.Distance(body.velocity, velocity) > 0.1) {
      Debug.Log("VELOCITY MISMATCH:" + Vector3.Distance(body.velocity, velocity));
      resetState = true;
    }
    if(Vector3.Distance(body.angularVelocity, angularVelocity) > 0.1) {
      Debug.Log("ANGULAR VELOCITY MISMATCH:" + Vector3.Distance(body.angularVelocity, angularVelocity));
      resetState = true;
    }
    if(Vector3.Distance(bodyPosition, position) > 0.1) {
      Debug.Log("POSITION MISMATCH:" + Vector3.Distance(bodyPosition, position));
      resetState = true;
    } 
    if(resetState) {      
      GetComponent<Transform>().position = position;
      body.velocity = velocity;
      body.angularVelocity = angularVelocity;
    }
  }


}
