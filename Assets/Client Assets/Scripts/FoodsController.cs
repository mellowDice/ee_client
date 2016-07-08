using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FoodsController : MonoBehaviour {

  public GameObject foodPrefab;
  bool objectState;
  Dictionary<string, GameObject> foodsDict = new Dictionary<string, GameObject>();

  public void CreateFood (JSONObject foods)
  {
    // Debug.Log("foods" + foods);
    // Debug.Log(foods[0]);
    // Debug.Log(foods[0]["id"]);
    var length = foods.list.Count;
    for (var i = 0; i < length; i++) {
      var id = GetJSONString(foods[i], "id");
      var idVal = GetJSONFloat(foods[i], "id");
      var x = GetJSONFloat(foods[i], "x");
      var z = GetJSONFloat(foods[i], "z");
      var position = new Vector3( x, 100f, z );

      if (!foodsDict.ContainsKey(id)) {
        var food = Instantiate(foodPrefab, position, Quaternion.identity) as GameObject;
        food.transform.parent = transform;
        food.GetComponent<FoodController>().id = id;
        foodsDict.Add(food.GetComponent<FoodController>().id, food);
      } if (idVal < 0 && x <= 0 && z <= 0) {
        foodsDict[id].SetActive(false);
      } else {
        foodsDict[id].transform.position = position;
        foodsDict[id].GetComponent<FoodController>().NewPos();
        foodsDict[id].SetActive(true);
      }
    }
  }

  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

  public static string GetJSONString (JSONObject data, string key) {
    return data[key].ToString().Trim('"');
  }

}