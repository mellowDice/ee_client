using UnityEngine;
using System.Collections;

public class TempScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() {
    float revealOffset = (float)(Time.timeSinceLevelLoad % 10) / 10.1F; 
    gameObject.GetComponent<Renderer>().material.SetFloat ("_Cutoff", revealOffset);
  }
}
