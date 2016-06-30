using UnityEngine;
using System.Collections;
// using System.Collections.Generic;
using SocketIO;

public class NetworkController : MonoBehaviour {

	// Use this for initialization
  static SocketIOComponent socket;
  public GameObject myPlayer;
  public bool disableLandscape = false;
  
  
  void Awake () {
    socket = GetComponent<EESocketIOComponent>();
  }
	void Start () {
    socket.On("open", OnConnected);
    socket.On("load", BuildTerrain);
	}

  void OnConnected(SocketIOEvent e) {
    Debug.Log("Connected to server.");
  }

  void BuildTerrain(SocketIOEvent e) {
    if(disableLandscape) return;
    Debug.Log("Building Terrain...");
    var ter = GetComponent<CreateTerrainMesh>();
    ter.BuildMesh(e.data["terrain"]);
    myPlayer.GetComponent<Rigidbody>().useGravity = true;
  }

  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

  public static string VectorToJSON (Vector3 position) {
    return string.Format(@"{{""x"":""{0}"", ""y"":""{1}"", ""z"":""{2}""}}", position.x, position.y, position.z);
  }
}
