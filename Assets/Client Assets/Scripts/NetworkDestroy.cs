using UnityEngine;
using System.Collections;
using SocketIO;

// network destroy will pool the objects.
// it will keep track of active obstacles and food

public class NetworkDestroy : MonoBehaviour {

  static SocketIOComponent socket;
  public GameObject food;
  public GameObject obstacle; // send obstacle id
  public GameObject player;

  void OnDestroy () {
    // data to send - amount of food left
    //              - id of obstacle
  }

}