using UnityEngine;
using System.Collections;
using SocketIO;

// network destroy will pool the objects.
// it will keep track of active obstacles and food

public class NetworkDestroy : MonoBehaviour {

  static SocketIOComponent socket;

  public void OnDestroy (string id) {
    // data to send - amount of food left
    //              - id of obstacle
    Debug.Log(id);
  }

}