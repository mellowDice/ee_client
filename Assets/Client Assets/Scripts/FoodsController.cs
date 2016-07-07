using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FoodsController : MonoBehaviour {

  public GameObject foodPrefab;
  bool objectState;
  Dictionary<string, GameObject> foodsDict = new Dictionary<string, GameObject>();

  public void CreateFood (JSONObject foods)
  {


    var length = foods.list.Count;
    for (var i = 0; i < length; i++) {
      var id = GetJSONString(foods[i], "id");
      var position = new Vector3(GetJSONFloat(foods[i], "x"),
                                                            100f,
                                 GetJSONFloat(foods[i], "z")
                                 );

      if (!foodsDict.ContainsKey(id)) {
        var food = Instantiate(foodPrefab, position, Quaternion.identity) as GameObject;
        food.transform.parent = transform;
        food.GetComponent<FoodController>().id = id;
        foodsDict.Add(food.GetComponent<FoodController>().id, food);
      } else {
        ToggleState(id);
      }
    }
  }

  public void ToggleState (string id) {
    objectState = foodsDict[id].activeSelf;
    Debug.Log("state" + objectState);
    foodsDict[id].SetActive(!objectState);
  }


  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

  public static string GetJSONString (JSONObject data, string key) {
    return data[key].ToString().Trim('"');
  }

}