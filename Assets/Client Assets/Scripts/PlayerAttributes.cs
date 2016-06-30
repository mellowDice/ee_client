using UnityEngine;
using System.Collections;

public class PlayerAttributes : MonoBehaviour {
  public bool mainPlayer = false;
  private float pMass = 10;

  // PROPERTIES ///
  public float playerMass {
    get { return pMass; }
    set {
      pMass = value;
      var scale = Mathf.Pow(pMass, 1/3);
      GetComponent<Transform>().localScale = new Vector3(scale, scale, scale);
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
