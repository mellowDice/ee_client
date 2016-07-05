using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

	GameObject mainPlayer;

  void Start () {
    mainPlayer = GameAttributes.mainPlayer;
  }
	void Update () {
    var cameraDirection = GameAttributes.camera.transform.forward;
	  transform.position = new Vector3(mainPlayer.transform.position.x, mainPlayer.transform.position.y + 3f, mainPlayer.transform.position.z) - cameraDirection * 6f;
	}
}
