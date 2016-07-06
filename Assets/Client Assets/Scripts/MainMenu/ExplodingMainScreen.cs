using UnityEngine;
using System.Collections;

public class ExplodingMainScreen : MonoBehaviour {
  float charge = 0;
  bool isCharged = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    var ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    RaycastHit hit = new RaycastHit();

    Physics.Raycast(ray, out hit, 1000f);

	  if (hit.collider != null
      && hit.collider.name == "TriggerSphere"
      ) {
      hit.collider.GetComponent<Explode>().Boom();
    }
	}

  void ToggleCharge() {
    isCharged = false;
  }
}
