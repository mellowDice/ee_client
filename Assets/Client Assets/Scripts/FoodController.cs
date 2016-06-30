using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FoodController : MonoBehaviour {

  public GameObject foodPrefab;

  // Dictionary<string, string> foods = new Dictionary<string, string>();

  public void CreateFood (JSONObject foods)
  {
    var length = foods.list.Count;
    for (var i = 0; i < length; i++) {
      var position = new Vector3(GetJSONFloat(foods[i], "x"),
                                 GetJSONFloat(foods[i], "y") * 50f,
                                 GetJSONFloat(foods[i], "z")
                                 );
      var food = Instantiate(foodPrefab, position, Quaternion.identity) as GameObject;
    }
  }
  // void DestroyObject (string id)
  // {
  //   // obstacle active state to false
  //   if (other.gameObject.CompareTag("Player"))
  //   {
  //     obstaclePrefab.SetActive(false);
  //   }

  //   var obstacle = GetComponent;
  //   var destroy = obstacles.GetComponent<NetworkDestroy>();
  //   destroy.OnDestroy(obstacleId, obstaclePosition);
  //   // remove from obstacles object - still need id
  //   foods.Remove(id);
  // }
  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

}