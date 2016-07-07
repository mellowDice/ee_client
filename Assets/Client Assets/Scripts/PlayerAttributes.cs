using UnityEngine;
using System.Collections;

public class PlayerAttributes : MonoBehaviour {
  public bool mainPlayer = false;
  private float pMass = 10;
  public string id;
  public float TEMP;
  public bool zombiePlayer = false;

  // PROPERTIES ///
  public float playerMass {
    get { return pMass; }
    set {
      pMass = value;
      var scale = Mathf.Pow(pMass, 1f/3f);
      transform.localScale = new Vector3(scale*0.95f, scale, scale);
      GetComponent<Rigidbody>().mass = 1; //Mathf.Pow(pMass/50, 1/2);

      // Also set drag and speed to avoid accidental mismatch
      GetComponent<Rigidbody>().drag = 0.6f;
      GetComponent<PlayerMovement>().force = 11;
      GetComponent<PlayerMovement>().maxVelocity = 1 * Mathf.Log10(pMass/2+5);




      TEMP = pMass;
    }
  }

  // METHODS ///
	void Start () {
    playerMass = pMass; // Allow size-derived properties to be set
	}

	// Update is called once per frame
	void Update () {

	}
}
