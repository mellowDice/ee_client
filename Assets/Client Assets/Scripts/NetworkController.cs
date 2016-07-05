using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

// Define Callback Delegate;
public delegate void Callback();

public class NetworkController : MonoBehaviour {

	// Use this for initialization
  public static SocketIOComponent socket;
  static List<Callback> onReadyCallbacks = new List<Callback>();
  static bool loaded = false;

  void Awake () {
    socket = GetComponent<EESocketIOComponent>();
  }
	void Start () {
    socket.On("open", OnConnected);
    socket.On("load", OnLoad);
	}

  public static void OnReady(Callback cb) {
    if(loaded) {
      cb();
    }
    onReadyCallbacks.Add(cb);
  }
  public void Ready() {
    Debug.Log("Ready");
    loaded = true;
    onReadyCallbacks.ForEach(delegate(Callback cb){
      cb();
    });
  }
  void OnConnected(SocketIOEvent e) {
    Debug.Log("Connected to server.");
  }

  void OnLoad(SocketIOEvent e) {
    if(GameAttributes.disableLandscape) return;
    Debug.Log("Building Terrain...");
    var ter = GetComponent<CreateTerrainMesh>();
    ter.BuildMesh(e.data["terrain"]);

    Ready();    
    PlayerNetworkController.InstantiateMainPlayer(PlayerNetworkController.GetJSONString(e.data, "id"), PlayerNetworkController.GetJSONFloat(e.data, "mass"));

    var obs = GetComponent<ObstaclesController>();
    obs.CreateObstacle(e.data["obstacles"]);

    var foods = GetComponent<FoodController>();
    foods.CreateFood(e.data["food"]);

  }
}
