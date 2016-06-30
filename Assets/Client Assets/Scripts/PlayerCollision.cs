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

  void Start() {
    retFill.type = Image.Type.Filled;
    retFill.fillClockwise = true;
	retFill2.type = Image.Type.Filled;
	retFill2.fillClockwise = true;
  }

	void Update() {
    var ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    RaycastHit hit = new RaycastHit();
    if (Physics.Raycast(ray, out hit, 1000f)) {
      if (hit.collider.name == "TriggerSphere") {
        fillReticle();
      }
    }
    if (charge > 0) {
      charge--;
    }
    if (charge < 1 && !vr) {
      boost = false;
      GetComponent<PlayerMovement>().speedMultiplier = 1f;
    } else if (charge < 1 && vr) {
      boost = false;
      GetComponent<PlayerMovementVR>().speedMultiplier = 1f;
    }
    retFill.fillAmount = (charge)/maxCharge;
	retFill2.fillAmount = (charge)/maxCharge;
  }
  /////////////////////
  //COLLISION CHECKER//
  /////////////////////
	void OnTriggerEnter(Collider other) {
    particles.GetComponent<ParticleSystem>().Play();
    zombieSpawner.GetComponent<ZombieSpawner>().ZombieCollide(other.transform.parent.gameObject);
  }

  void fillReticle() {
    if (charge < maxCharge && boost == false) {
      charge += 2;
    } else if (!vr) {
      GetComponent<PlayerMovement>().speedMultiplier = 3f;
      boost = true;
    } else if (vr) {
      GetComponent<PlayerMovementVR>().speedMultiplier = 3f;
      boost = true;
    }
  }
}
