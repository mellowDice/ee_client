using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FoodsController : MonoBehaviour {

  public GameObject foodPrefab;
  Color[] colors = new Color[6];
  bool objectState;
  Dictionary<string, GameObject> foodsDict = new Dictionary<string, GameObject>();

  public void Start() {
    ChangeColor();
  }

  public void CreateFood (JSONObject foods)
  {
    var length = foods.list.Count;
    for (var i = 0; i < length; i++) {
      var id = GetJSONString(foods[i], "id");
      var idVal = GetJSONFloat(foods[i], "id");
      var x = GetJSONFloat(foods[i], "x");
      var z = GetJSONFloat(foods[i], "z");
      var position = new Vector3( x,
                                  GameAttributes.GetLandscapeY(transform.position.x, transform.position.z) + 1f,
                                  z );

      if (!foodsDict.ContainsKey(id)) {
        var food = Instantiate(foodPrefab, position, Quaternion.identity) as GameObject;
        food.transform.parent = transform;
        food.GetComponent<FoodController>().id = id;
        food.GetComponent<Renderer>().material.color = colors[Random.Range(0, colors.Length)];
        foodsDict.Add(food.GetComponent<FoodController>().id, food);

      } if (idVal < 0 && x <= 0 && z <= 0) {
        foodsDict[id].SetActive(false);
      } else {
        foodsDict[id].transform.position = position;
        foodsDict[id].SetActive(true);
      }
    }
  }

  // public void ToggleState (string id) {
  //   objectState = foodsDict[id].activeSelf;
  //   Debug.Log("state" + objectState);
  //   foodsDict[id].SetActive(!objectState);
  // }


  public void ChangeColor() {
    colors[0] = Color.cyan;
    colors[1] = Color.red;
    colors[2] = Color.green;
    colors[3] = new Color(255, 165, 0);
    colors[4] = Color.yellow;
    colors[5] = Color.magenta;
  }
  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

  public static string GetJSONString (JSONObject data, string key) {
    return data[key].ToString().Trim('"');
  }

}