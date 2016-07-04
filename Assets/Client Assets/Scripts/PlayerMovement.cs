using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

  // Player Rigidbody
  Rigidbody body;

  // Movement variables
  public float speed = 5f;
  float speedMultiplier = 0f;
  Vector3 computerControlledDirection = new Vector3(1,0,0);
  Vector3 direction = new Vector3(1,0,0);

  // Components
  PlayerAttributes playerAttributes;
  PlayerNetworkController playerNetworkController;
  ParticleSystem particles;

  // Boost
  public Image retFillNonVR;
  public Image retFillVR;
  private float charge = 0;
  private float maxCharge = 150;
  private bool boost = false;


  ////////////////////////////
  // Unity Built-in Methods //
  ////////////////////////////

	// Initialization
	public virtual void Start () {
    particles = GameAttributes.camera.GetComponentInChildren<ParticleSystem>();
    particles.loop = true;
    particles.Play();
    body = GetComponent<Rigidbody>();
    playerAttributes = GetComponent<PlayerAttributes>();

    // Additional setup only for main player
    if(playerAttributes.mainPlayer) {
      playerNetworkController = GameObject.Find("Network").GetComponent<PlayerNetworkController>();
      
      // Set up direction and state monitoring
      InvokeRepeating("UpdateAndSendPlayerDirection", 0f, 0.1f);
      InvokeRepeating("SendMainPlayerState", 0f, 0.1f);

      // Set up for computer controlled main player
      if(GameAttributes.computerControlledMainPlayer) {
        InvokeRepeating("UpdateComputerControlledDirection", 2.5f, 5f);
      }
    }
    NetworkController.OnReady(delegate() {
      speedMultiplier = 1f;
      RaycastHit hit = new RaycastHit();
      if(Physics.Raycast(transform.position, Vector3.down, out hit)) {
        transform.position -= new Vector3(0 , hit.distance - 10 , 0);
      }
      particles.loop = false;
      particles.Stop();
    });

    NetworkController.OnReady(delegate() {
      GetComponent<Rigidbody>().useGravity = true;
    });

    if(gameObject == GameAttributes.mainPlayer)
      SetupBoost();
	}

  // Before Every Physics Calculation, add force to player
  void FixedUpdate () {
    body.AddForce(direction * speed * speedMultiplier); 
  }

  void Update () {
    if(gameObject == GameAttributes.mainPlayer)
      CheckBoost();
  }
  /////////////////////////////////////
  // Methods Relating to All Players //
  /////////////////////////////////////

  public void Boost() {
    speedMultiplier = 3;
    playerNetworkController.Boost();
    boost = true;
  }
  public void EndBoost() {
    speedMultiplier = 1;
    boost = false;
  }


  /////////////////////////////////////
  // Methods Relating to Main Player //
  /////////////////////////////////////

  // Update player direction based on where camera is facing
  void UpdateAndSendPlayerDirection() {
    var camDirection = GameAttributes.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).direction;
    direction = GameAttributes.computerControlledMainPlayer ? computerControlledDirection : camDirection;
    playerNetworkController.Look(direction);
  }

  // Send server current position/velocity of player
  void SendMainPlayerState() {
    Debug.Log("send player state");
    var position = transform.position;
    var velocity = body.velocity;
    var angularVelocity = body.angularVelocity;
    var rotation = transform.rotation;
    playerNetworkController.PlayerStateSend(position, velocity, angularVelocity, rotation);
  }

  // Keeps camera situated above ball
  public virtual void setCameraPosition() {
      var radius = GetComponent<SphereCollider>().radius;
      GameAttributes.camera.GetComponent<Transform>().position = GetComponent<Transform>().position + new Vector3(0,radius,0);    
  }

  // For debugging - has player move in circles instead following camera
	void UpdateComputerControlledDirection() {
    computerControlledDirection = Quaternion.AngleAxis(90.0f, Vector3.up) * computerControlledDirection;
  }


  ///////////////////////////////////////
  // Methods Relating to Other Players //
  ///////////////////////////////////////

  // Receive from server direction player is currently facing
  public void ReceivePlayerDirection(Vector3 networkDir) {
    direction = networkDir;
  }

  // Receive  from server updated position/velocity and compare
  public void ReceivePlayerState(Vector3 position, Vector3 velocity, Vector3 angularVelocity, Quaternion rotation) {
    var resetState = false;
    var bodyPosition = transform.position;
    var bodyRotation = transform.rotation;
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
    if(Quaternion.Dot(bodyRotation, rotation) < 0.95) {
      Debug.Log("ROTATION MISMATCH:" + Quaternion.Dot(bodyRotation, rotation));
      resetState = true;
    }
    if(resetState) {
      transform.position = position;
      body.velocity = velocity;
      body.angularVelocity = angularVelocity;
      transform.rotation = rotation;
    }
  }


  void SetupBoost() {
    if (!GameAttributes.VR) {
      retFillNonVR.type = Image.Type.Filled;
      retFillNonVR.fillClockwise = true;
      retFillNonVR.fillAmount = 0;
    } else {
      retFillVR.type = Image.Type.Filled;
      retFillVR.fillClockwise = true;
      retFillVR.fillAmount = 0;
    }
    particles = GameAttributes.camera.GetComponentInChildren<ParticleSystem>();
  }
  void CheckBoost() {

    var ray = GameAttributes.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    RaycastHit hit = new RaycastHit();

    Physics.Raycast(ray, out hit, 1000f);

    // If pointing at target who is not larger than self.
    if (hit.collider != null
        && hit.collider.name == "TriggerSphere"
        //Prevents player from boosting into larger objects
        && GetComponent<PlayerAttributes>().playerMass >= hit.transform.GetComponent<PlayerAttributes>().playerMass
        && GetComponent<PlayerAttributes>().playerMass >= 7.5f
        ) {
      if(boost) {
        charge -= 80 * Time.deltaTime;
        if(charge <= 0) {
          Invoke("ToggleBoostTimer", 2);
          EndBoost();
        }
      }
      else {
        charge += 80 * Time.deltaTime;
        if(charge >= maxCharge) {
          //Will require server call to change playerMass
          GetComponent<PlayerAttributes>().playerMass -= 2.5f;
          Boost();
        }
      }
    }
    // If not pointing at target
    else {
      if(charge > 0) {
        if(boost) {
          charge -= 80 * Time.deltaTime;
        } else {
          charge -= 40 * Time.deltaTime;
        }
      }
      else if (charge <= 0 && boost) {
        Invoke("ToggleBoostTimer", 2);
        EndBoost();
      }
    }
    if (!GameAttributes.VR) {
      retFillNonVR.fillAmount = (charge)/maxCharge;
    } else {
      retFillVR.fillAmount = (charge)/maxCharge;
    }
  }
  void ToggleBoostTimer() {
    EndBoost();
  }
}