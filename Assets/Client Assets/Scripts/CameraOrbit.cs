using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

	public GameObject ball;

	void Update () {
	  transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y + 1f, ball.transform.position.z);
	}
}
