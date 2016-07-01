using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {

  public int id;

  // Use this for initialization
  void Start () {

  }

  // Update is called once per frame
  void Update () {

  }

  void OnTriggerEnter (Collider other) {

    if (other.gameObject.CompareTag("Player")) {
      // pass over id to server
      // setactive state to false
      Debug.Log("hit.");
    }

  }

}
