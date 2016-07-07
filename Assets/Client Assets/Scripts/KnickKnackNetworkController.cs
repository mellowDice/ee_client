using UnityEngine;
using SocketIO;

public class KnickKnackNetworkController : MonoBehaviour {

	// Use this for initialization
  public GameObject foodPrefab;
  public GameObject obstaclePrefab;
  static SocketIOComponent socket;

  void Awake () {

  }

	void Start () {
    socket = NetworkController.socket;
    socket.On("field_objects", CreateKnickknacks);
    socket.On("eaten", ToggleFoodState);
    socket.On("collided", ToggleObstacleState);
	}

  ////////////////////////////////////
  //       Create Knickknacks       //
  ////////////////////////////////////
  void CreateKnickknacks (SocketIOEvent e) {
    Debug.Log("food" + e.data["food"]);
    foodPrefab.GetComponent<FoodsController>().CreateFood(e.data["food"]);
    obstaclePrefab.GetComponent<ObstaclesController>().CreateObstacle(e.data["obstacles"]);
  }

  ////////////////////////////////////
  //     Methods Related to Food    //
  ////////////////////////////////////
  void ToggleFoodState (SocketIOEvent e) {
    Debug.Log("newFood!" + e.data);
    foodPrefab.GetComponent<FoodsController>().CreateFood(e.data["food"]);
  }

  public static void FoodEaten (string id) {
    var foodId = new JSONObject();
    foodId.AddField("id", id);
    Debug.Log("foodID" + id + foodId);
    socket.Emit("eat", foodId);
  }

  ////////////////////////////////////
  //  Methods Related to Obstacles  //
  ////////////////////////////////////
  void ToggleObstacleState (SocketIOEvent e) {
    obstaclePrefab.GetComponent<ObstaclesController>().CreateObstacle(e.data["obstacles"]);
  }

  public static void ObstacleCollision (string id) {
    var objectId = new JSONObject();
    objectId.AddField("id", id);
    socket.Emit("collision", objectId);
  }
}
