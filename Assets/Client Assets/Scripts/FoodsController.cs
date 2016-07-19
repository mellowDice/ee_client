using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FoodsController : MonoBehaviour {

  public GameObject foodPrefab;
  Color[] colors = new Color[6];
  bool objectState;
  static Dictionary<string, GameObject> foodsDict = new Dictionary<string, GameObject>();

  public void Start() {
    ChangeColor();
  }


  public void CreateOrMoveFood (string id, float x, float z) {
    var position = new Vector3( x,
                                GameAttributes.GetLandscapeY(x, z) + 1f,
                                z );
    if (!foodsDict.ContainsKey(id)) {
      var food = Instantiate(foodPrefab, position, Quaternion.identity) as GameObject;
      food.transform.parent = transform;
      food.GetComponent<FoodController>().id = id;
      food.GetComponent<Renderer>().material.color = colors[Random.Range(0, colors.Length)];
      foodsDict.Add(food.GetComponent<FoodController>().id, food);
    }
    else {
      Debug.Log("Moving Food " + id + " " + position.x + " " + position.z);
      foodsDict[id].SetActive(false);
      foodsDict[id].transform.position = position;
    }
  }

  public static void DeactivateFood (string id) {
      foodsDict[id].SetActive(false);
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