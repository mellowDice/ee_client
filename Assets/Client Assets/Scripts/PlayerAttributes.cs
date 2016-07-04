using UnityEngine;
using System.Collections;

public class PlayerAttributes : MonoBehaviour {
  public bool mainPlayer = false;
  private float pMass = 10;
  public string id;
  public float TEMP;

  // PROPERTIES ///
  public float playerMass {
    get { return pMass; }
    set {
      pMass = value;
      var scale = Mathf.Pow(pMass, 1f/3f);
      transform.localScale = new Vector3(scale*0.95f, scale, scale);
      GetComponent<Rigidbody>().mass = pMass/40;
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
