using UnityEngine;
using SocketIO;

public class KnickKnackNetworkController : MonoBehaviour {

	// Use this for initialization
  public GameObject foodPrefab;
  public GameObject obstaclePrefab;
  static SocketIOComponent socket;

  void Awake () {
    socket = NetworkController.socket;
  }

	void Start () {
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
    foodPrefab.GetComponent<FoodsController>().CreateFood(e.data);
  }
  public void FoodEaten (string id) {
    socket.Emit("eat", new JSONObject(id));
  }

  ////////////////////////////////////
  //  Methods Related to Obstacles  //
  ////////////////////////////////////
  void ToggleObstacleState (SocketIOEvent e) {
    obstaclePrefab.GetComponent<ObstaclesController>().CreateObstacle(e.data);
  }
  public void ObstacleCollision (string id) {
    socket.Emit("collision", new JSONObject(id));
  }
}
