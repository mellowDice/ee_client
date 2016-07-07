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
      var position = new Vector3(GetJSONFloat(obstacles[i], "x"),
                                                            100f,
                                 GetJSONFloat(obstacles[i], "z")
                                 );

      if (!obstaclesDict.ContainsKey(id)) {
        var obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity) as GameObject;
        obstacle.transform.parent = transform;
        obstacle.GetComponent<ObstacleController>().id = id;
        obstaclesDict.Add(obstacle.GetComponent<ObstacleController>().id, obstacle);
      } else {
        ToggleState(id);
      }
    }
  }

  public void ToggleState (string id) {
    objectState = obstaclesDict[id].activeSelf;
    obstaclesDict[id].SetActive(!objectState);
  }


  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

  public static string GetJSONString (JSONObject data, string key) {
    return data[key].ToString().Trim('"');
  }

}