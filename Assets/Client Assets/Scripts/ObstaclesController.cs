using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObstaclesController : MonoBehaviour {

  public GameObject obstaclePrefab;
  bool objectState;
  static Dictionary<string, GameObject> obstaclesDict = new Dictionary<string, GameObject>();


  public void CreateOrMoveObstacle(string id, float x, float z) {
    var position = new Vector3( x,
                                GameAttributes.GetLandscapeY(x, z) + 1f,
                                z );
    if (!obstaclesDict.ContainsKey(id)) {
      var obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity) as GameObject;
      obstacle.transform.parent = transform;
      obstacle.GetComponent<ObstacleController>().id = id;
      obstaclesDict.Add(obstacle.GetComponent<ObstacleController>().id, obstacle);
    } else {
      obstaclesDict[id].transform.position = position;
      obstaclesDict[id].SetActive(true);
    }
  }


  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

  public static string GetJSONString (JSONObject data, string key) {
    return data[key].ToString().Trim('"');
  }

}