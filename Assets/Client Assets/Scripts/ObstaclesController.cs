using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObstaclesController : MonoBehaviour {

  public GameObject obstaclePrefab;
  bool objectState;
  Dictionary<string, GameObject> obstaclesDict = new Dictionary<string, GameObject>();

  public void CreateObstacle (JSONObject obstacles)
  {


    var length = obstacles.list.Count;
    for (var i = 0; i < length; i++) {
      var id = GetJSONString(obstacles[i], "id");
      var idVal = GetJSONFloat(obstacles[i], "id");
      var x = GetJSONFloat(obstacles[i], "x");
      var z = GetJSONFloat(obstacles[i], "z");
      var position = new Vector3( x,
                                  GameAttributes.GetLandscapeY(transform.position.x, transform.position.z) + 1f,
                                  z );

      if (!obstaclesDict.ContainsKey(id)) {
        var obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity) as GameObject;
        obstacle.transform.parent = transform;
        obstacle.GetComponent<ObstacleController>().id = id;
        obstaclesDict.Add(obstacle.GetComponent<ObstacleController>().id, obstacle);
      } if (idVal < 0 && x <= 0 && z <= 0) {
        obstaclesDict[id].SetActive(false);
      } else {
        obstaclesDict[id].transform.position = position;
        obstaclesDict[id].SetActive(true);
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