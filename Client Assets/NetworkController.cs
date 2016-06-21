using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class NetworkController : MonoBehaviour {

	// Use this for initialization
  static SocketIOComponent socket;
  public GameObject playerPrefab;

  Dictionary<string, GameObject> players;
  
  void Awake () {
    socket = GetComponent<SocketIOComponent>();
  }
	void Start () {
    // socket.On("abcd", BuildTerrain);
    socket.On("open", OnConnected);
    socket.On("spawn", OnSpawned);
    socket.On("clientDisconnect", OnDisconnect);
    socket.On("move", OnMove);

    players = new Dictionary<string, GameObject> ();
	}

  void OnConnected(SocketIOEvent e) {
    Debug.Log("Connected to server.");
    // socket.Emit("loggedIn");
  }

  void OnSpawned(SocketIOEvent e) {
    Debug.Log("New Player Spawned" + e.data);
    var player = Instantiate(playerPrefab);
    players.Add(e.data["id"].ToString(), player);

    Debug.Log("count: " + players.Count);
  }

  void BuildTerrain(SocketIOEvent e) {
    Debug.Log("Building Terrain...");
    // BuildingTerrain

    // Debug.Log(e);
  }

  void OnDisconnect(SocketIOEvent e) {
    Debug.Log("Client disconnected... " + e.data);
    var id = e.data["id"].ToString();
    var player = players[id];
    Destroy(player);
    players.Remove(id);
  }

  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

  void OnMove(SocketIOEvent e) {
    var player = players[e.data["id"].ToString()];
    var navigate = player.GetComponent<NavigatePosition>();
    var pos = new Vector3(GetJSONFloat(e.data, "x"), GetJSONFloat(e.data, "y"), GetJSONFloat(e.data, "z"));
    navigate.NavigateTo(pos);
  }
}
