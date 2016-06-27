using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class NetworkController : MonoBehaviour {

	// Use this for initialization
  static SocketIOComponent socket;
  public GameObject playerPrefab;
  public GameObject myPlayer;

  Dictionary<string, GameObject> players;
  
  void Awake () {
    socket = GetComponent<SocketIOComponent>();
  }
	void Start () {
    socket.On("open", OnConnected);
    socket.On("load", BuildTerrain);
    socket.On("spawn", OnSpawned);
    socket.On("onEndSpawn", OnEndSpawn);
    socket.On("playerMove", OnMove);
    socket.On("requestPosition", OnRequestPosition);
    socket.On("updatePosition", OnUpdatePosition);
    players = new Dictionary<string, GameObject> ();
	}

  void OnConnected(SocketIOEvent e) {
    Debug.Log("Connected to server.");
  }

  void OnSpawned(SocketIOEvent e) {
    var player = Instantiate(playerPrefab);
    players.Add(e.data["id"].ToString(), player);
  }

  void BuildTerrain(SocketIOEvent e) {
    Debug.Log("Building Terrain...");
    var ter = GetComponent<CreateTerrainMesh>();
    ter.BuildMesh(e.data["terrain"]);
  }

  void OnEndSpawn(SocketIOEvent e) {
    // Debug.Log("Client disconnected... " + e.data);
    var id = e.data["id"].ToString();
    var player = players[id];
    Destroy(player);
    players.Remove(id);
  }

  void OnMove(SocketIOEvent e) {
    var player = players[e.data["id"].ToString()];
    var navigate = player.GetComponent<NavigatePosition>();
    var pos = new Vector3(GetJSONFloat(e.data, "x"), GetJSONFloat(e.data, "y"), GetJSONFloat(e.data, "z"));
    navigate.NavigateTo(pos);
  }

  void OnRequestPosition(SocketIOEvent e) {
    socket.Emit("playerPosition", new JSONObject(VectorToJSON(myPlayer.transform.position)));
  }

  void OnUpdatePosition(SocketIOEvent e) {
    var player = players[e.data["id"].ToString()];
    var pos = new Vector3(GetJSONFloat(e.data, "x"), GetJSONFloat(e.data, "y"), GetJSONFloat(e.data, "z"));
    player.transform.position = pos;
  }

  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

  public static string VectorToJSON (Vector3 position) {
    return string.Format(@"{{""x"":""{0}"", ""y"":""{1}"", ""z"":""{2}""}}", position.x, position.y, position.z);
  }
}
