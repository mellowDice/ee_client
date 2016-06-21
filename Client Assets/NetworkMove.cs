using UnityEngine;
using System.Collections;
using SocketIO;

public class NetworkMove : MonoBehaviour {

  public SocketIOComponent socket;

  public void OnMove (Vector3 position) {
    Debug.Log("sending: " + VectorToJSON(position));
    socket.Emit("move", new JSONObject(VectorToJSON(position)));
  }

  string VectorToJSON (Vector3 position) {
    return string.Format(@"{{""x"":""{0}"", ""y"":""{1}"", ""z"":""{2}""}}", position.x, position.y, position.z);
  }
}
