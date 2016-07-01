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
  private float maxCharge = 100;
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

    // If pointing at target
    if (hit.collider != null && hit.collider.name == "TriggerSphere") {
      if(boost) {
        charge--;
        if(charge <= 0) {
          boost = false;
          playerMovement.EndBoost();
        }
      }
      else {
        charge++;
        if(charge == maxCharge) {
          boost = true;
          playerMovement.Boost();
        }
      }
    }

    // If not pointing at target
    else {
      if(charge > 0) {
        charge--;
      }
      else if (charge <= 0 && boost) {
        boost = false;
        playerMovement.EndBoost();
      }
    }
    if (GameAttributes.VR) {
      retFill.fillAmount = (charge)/maxCharge;
    } else {
      retFill2.fillAmount = (charge)/maxCharge;
    }
  }
  
  /////////////////////
  //COLLISION CHECKER//
  /////////////////////
	void OnTriggerEnter(Collider other) {

    if (other.gameObject.CompareTag("Zombie")) {
      particles.Play();
      zombieSpawner.GetComponent<ZombieSpawner>().ZombieCollide(other.transform.parent.gameObject);
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
