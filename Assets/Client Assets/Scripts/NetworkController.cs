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
  static bool loaded;
  static CreateTerrainMesh ter;
  static ObstaclesController obs;
  static FoodsController foods;
  static bool initialized = false;

  void Awake () {
    loaded = false;
    if (socket == null) {
      ter = GetComponent<CreateTerrainMesh>();
      obs = GetComponent<ObstaclesController>();
      foods = GetComponent<FoodsController>();
      socket = GetComponent<EESocketIOComponent>();
    }
  }
	void Start () {
    if (!initialized) {
      socket.On("open", OnConnected);
      socket.On("landscape", OnReceiveLandscape);
      socket.On("field_objects", OnReceiveFieldObjects);
      initialized = true;
    }
	}

  public static void OnReady(Callback cb) {
    if(loaded) {
      cb();
    }
    onReadyCallbacks.Add(cb);
  }
  public static void Ready() {
    Debug.Log("Ready");
    loaded = true;
    onReadyCallbacks.ForEach(delegate(Callback cb){
      cb();
    });
  }
  static void OnConnected(SocketIOEvent e) {
    Debug.Log("Connected to server.");
  }

  static void OnReceiveLandscape(SocketIOEvent e) {
    if(!GameAttributes.disableLandscape) {
      Debug.Log("Building Terrain...");
      ter.BuildMesh(e.data["terrain"]);
    }
    Ready();  // for now, assume everything is loaded once landscape is received
  }

  static void OnReceiveFieldObjects(SocketIOEvent e) {
    // var obs = GetComponent<ObstaclesController>();
    obs.CreateObstacle(e.data["obstacles"]);

    // var foods = GetComponent<FoodController>();
    foods.CreateFood(e.data["food"]);
  }
}
