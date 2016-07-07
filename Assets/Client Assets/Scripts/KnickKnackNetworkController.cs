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
    foodPrefab.GetComponent<FoodsController>().CreateFood(e.data["food"]);
    obstaclePrefab.GetComponent<ObstaclesController>().CreateObstacle(e.data["obstacles"]);
  }

  ////////////////////////////////////
  //     Methods Related to Food    //
  ////////////////////////////////////
  void ToggleFoodState (SocketIOEvent e) {
    foodPrefab.GetComponent<FoodsController>().CreateFood(e.data["food"]);
  }

  public static void FoodEaten (string playerId, string foodId) {
    var foodData = new JSONObject();
    foodData.AddField("food_id", foodId);
    foodData.AddField("player_id", playerId);

    socket.Emit("eat", foodData);
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
