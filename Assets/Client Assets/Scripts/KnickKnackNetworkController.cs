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

  public static void ObstacleCollision (string playerId, string obstacleId) {
    var objectId = new JSONObject();
    objectId.AddField("obstacle_id", obstacleId);
    objectId.AddField("player_id", playerId);
    socket.Emit("collision", objectId);
  }
}
