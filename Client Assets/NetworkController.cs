using UnityEngine;
using System.Collections;
using SocketIO;

public class NetworkController : MonoBehaviour {

	// Use this for initialization
  static SocketIOComponent socket;
  public GameObject playerPrefab;
  
  void Awake () {
    socket = GetComponent<SocketIOComponent>();
  }
	void Start () {
   socket.On("open", OnConnected);
   // socket.On("buildterrain", BuildTerrain);
   socket.On("spawn", OnSpawned);
	}

  void OnConnected(SocketIOEvent e) {
    Debug.Log("Connected to server.");
    socket.Emit("loggedIn");
  }

  void OnSpawned(SocketIOEvent e) {
    Debug.Log("New Player Spawned");
    Instantiate(playerPrefab);
  }

  void BuildTerrain(SocketIOEvent e) {
    Debug.Log("Building Terrain...");
    Debug.Log(e);

  }
}
