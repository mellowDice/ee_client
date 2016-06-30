using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
  public GameObject particles;
  public Image retFill;
  public Image retFill2;
  // public GameObject gvrmain;
  public Camera cam;
  public GameObject zombieSpawner;
  private float charge = 0;
  private float maxCharge = 100;
  private bool boost = false;
  public bool vr = false;
  PlayerMovement playerMovement;

  void Start() {
    retFill.type = Image.Type.Filled;
    retFill.fillClockwise = true;
    if (vr) {
	    retFill2.type = Image.Type.Filled;
	    retFill2.fillClockwise = true;
    }
    playerMovement = vr ? GetComponent<PlayerMovementVR>() : GetComponent<PlayerMovement>();
  }

	void Update() {
    var ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
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

    retFill.fillAmount = (charge)/maxCharge;
    if (vr) {
	    retFill2.fillAmount = (charge)/maxCharge;
    }
  }
  
  /////////////////////
  //COLLISION CHECKER//
  /////////////////////
	void OnTriggerEnter(Collider other) {
    particles.GetComponent<ParticleSystem>().Play();
    zombieSpawner.GetComponent<ZombieSpawner>().ZombieCollide(other.transform.parent.gameObject);
  }
}
