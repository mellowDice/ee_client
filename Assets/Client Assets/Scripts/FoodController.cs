using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FoodController : MonoBehaviour {

  public GameObject foodPrefab;
  Dictionary<string, GameObject> foodsDict = new Dictionary<string, GameObject>();

  public void CreateFood (JSONObject foods)
  {
    var length = foods.list.Count;
    for (var i = 0; i < length; i++) {
      var position = new Vector3(GetJSONFloat(foods[i], "x"),
                                 GetJSONFloat(foods[i], "y") * 50f,
                                 GetJSONFloat(foods[i], "z")
                                 );
      var food = Instantiate(foodPrefab, position, Quaternion.identity) as GameObject;
      foodsDict.Add(foods[i]["id"].ToString(), food);
    }
  }

  void DestroyFood (string id)
  {
    foodsDict["id"].SetActive(false);
    foodsDict.Remove(id);
  }

  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

}