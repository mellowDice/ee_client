using UnityEngine;
using SocketIO;

public class KnickKnackNetworkController : MonoBehaviour {

	// Use this for initialization
  public GameObject foodPrefab;
  public GameObject obstaclePrefab;
  static SocketIOComponent socket;
  static FoodsController foodsController;
  static ObstaclesController obstaclesController;

  void Awake () {
    foodsController = foodPrefab.GetComponent<FoodsController>();
    obstaclesController = obstaclePrefab.GetComponent<ObstaclesController>();  
  }

	void Start () {
    socket = NetworkController.socket;
    socket.On("field_objects", CreateKnickknacks);
    socket.On("eaten", UpdateFoodState);
	}

  ////////////////////////////////////
  //       Create Knickknacks       //
  ////////////////////////////////////
  void CreateKnickknacks (SocketIOEvent e) {
    UpdateFoodState(e);
    CreateOrMoveObstacles(e);
  }

  ////////////////////////////////////
  //     Methods Related to Food    //
  ////////////////////////////////////

  public static void UpdateFoodState (SocketIOEvent e) {
    JSONObject foods = e.data["food"];
    var length = foods.list.Count;
    for (var i = 0; i < length; i++) {
      var idVal = GetJSONFloat(foods[i], "id");
      var id = GetJSONString(foods[i], "id");
      var x = GetJSONFloat(foods[i], "x");
      var z = GetJSONFloat(foods[i], "z");
      
      // negative ids represent food generated from player collision; being off map means being inactivated
      // TODO: server should send a field indicating inactivity
      if (idVal < 0 && x <= 0 && z <= 0) {
        FoodsController.DeactivateFood(id);
        return;
      }
      foodsController.CreateOrMoveFood(id, x, z);
    }
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

  public static void CreateOrMoveObstacles (SocketIOEvent e)
  {
    JSONObject obstacles = e.data["obstacles"];
    var length = obstacles.list.Count;
    for (var i = 0; i < length; i++) {
      var id = GetJSONString(obstacles[i], "id");
      var idVal = GetJSONFloat(obstacles[i], "id");
      var x = GetJSONFloat(obstacles[i], "x");
      var z = GetJSONFloat(obstacles[i], "z");
      var position = new Vector3( x,
                                  GameAttributes.GetLandscapeY(x, z) + 1f,
                                  z );
      obstaclesController.CreateOrMoveObstacle(id, x, z);
    }
  }
  public static void ObstacleCollision (string playerId, string obstacleId) {
    var objectId = new JSONObject();
    objectId.AddField("obstacle_id", obstacleId);
    objectId.AddField("player_id", playerId);
    socket.Emit("collision", objectId);
  }


  // Utilities
  static float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

  public static string GetJSONString (JSONObject data, string key) {
    return data[key].ToString().Trim('"');
  }
}
