using UnityEngine;
using System.Collections;
using SocketIO;

public class NetworkMove : MonoBehaviour {

  public SocketIOComponent socket;

  public void OnMove (Vector3 position) {
    Debug.Log("sending: " + position);
    socket.Emit("move");
  }
}
