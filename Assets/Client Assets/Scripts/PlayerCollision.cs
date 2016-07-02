using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
  ParticleSystem particles;
  public Image retFill;
  public Image retFill2;
  public GameObject zombieSpawner;
  public GameObject obstaclePrefab;
  private float charge = 0;
  private float maxCharge = 150;
  private bool boost = false;
  PlayerMovement playerMovement;

  void Awake() {
    playerMovement = GetComponent<PlayerMovement>();
  }

  void Start() {
    if (GameAttributes.VR) {
      retFill.type = Image.Type.Filled;
      retFill.fillClockwise = true;
    } else {
      retFill2.type = Image.Type.Filled;
      retFill2.fillClockwise = true;
    }
    particles = GameAttributes.camera.GetComponentInChildren<ParticleSystem>();
  }
	void Update() {
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
        charge -= 2;
        if(charge <= 0) {
          Invoke("ToggleBoostTimer", 2);
          playerMovement.EndBoost();
        }
      }
      else {
        charge += 2;
        if(charge >= maxCharge) {
          //Will require server call to change playerMass
          GetComponent<PlayerAttributes>().playerMass -= 2.5f;
          boost = true;
          playerMovement.Boost();
        }
      }
    }
    // If not pointing at target
    else {
      if(charge > 0) {
        if(boost) {
          charge -= 2;
        } else {
          charge--;
        }
      }
      else if (charge <= 0 && boost) {
        Invoke("ToggleBoostTimer", 2);
        playerMovement.EndBoost();
      }
    }
    if (GameAttributes.VR) {
      retFill.fillAmount = (charge)/maxCharge;
    } else {
      retFill2.fillAmount = (charge)/maxCharge;
    }
  }

  void ToggleBoostTimer() {
    boost = false;
  }
  
  /////////////////////
  //COLLISION CHECKER//
  /////////////////////
	void OnTriggerEnter(Collider other) {
    // if (other.gameObject.CompareTag("Zombie")) {
    //   particles.Play();
    //   zombieSpawner.GetComponent<ZombieSpawner>().ZombieCollide(other.transform.parent.gameObject);
    // }

    if (other.gameObject.CompareTag("Zombie") || other.gameObject.CompareTag("Player")) {
      var otherPlayer = other.transform.parent.gameObject;
      var otherPlayerAttributes = otherPlayer.GetComponent<PlayerAttributes>();
      var mainPlayerAttributes = GetComponent<PlayerAttributes>();

      if (otherPlayerAttributes.playerMass > mainPlayerAttributes.playerMass) {
        PlayerNetworkController.Kill("main player", GetComponent<Transform>().position.x, GetComponent<Transform>().position.z);
      }
      else if (otherPlayerAttributes.playerMass < mainPlayerAttributes.playerMass){
        PlayerNetworkController.Kill(otherPlayerAttributes.id, other.GetComponent<Transform>().position.x, other.GetComponent<Transform>().position.z);
        if (other.gameObject.CompareTag("Zombie")) {
          particles.Play();
          zombieSpawner.GetComponent<ZombieSpawner>().ZombieCollide(other.transform.parent.gameObject);
        }
      }
    }

    if (other.gameObject.CompareTag("Obstacle")) {
      // decrease player mass fn needed
      // other.gameObject.GetComponent<ObstacleController>().DestroyObstacle(other.id);
    }

    // if (other.gameObject.CompareTag("Food")) {
    //   // increase player mass
    //   foodPrefab.GetComponent<FoodController>().DestroyFood(other.id);
    // }

  }
}
