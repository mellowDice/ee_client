using UnityEngine;
using System.Collections;

public class ComputerMovement : MonoBehaviour {
  Rigidbody body;
  Vector3 direction = new Vector3(1,0,0);
  float speed = 5f;

	// Use this for initialization
	void Start () {
  body = GetComponent<Rigidbody>();
	InvokeRepeating("UpdateComputerControlledDirection", 2.5f, 5f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
    body.AddForce(direction * speed); 
  }

  void UpdateComputerControlledDirection() {
    direction = Quaternion.AngleAxis(90.0f, Vector3.up) * direction;
  }
}
