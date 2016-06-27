using UnityEngine;
using System.Collections;
using SocketIO;

public class NetworkMove : MonoBehaviour {

  public SocketIOComponent socket;

  public void OnMove (Vector3 position) {
    socket.Emit("move", new JSONObject(NetworkController.VectorToJSON(position)));
  }
  public void Look (Vector3 direction) {
    socket.Emit("look", new JSONObject(NetworkController.VectorToJSON(direction)));
  }
}
