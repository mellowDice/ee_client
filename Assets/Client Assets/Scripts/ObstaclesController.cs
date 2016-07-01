using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObstaclesController : MonoBehaviour {

  public GameObject obstaclePrefab;
  // Dictionary<string, GameObject> obstacles = new Dictionary<string, GameObject>();

  public void CreateObstacle (JSONObject obstacles)
  {
    var length = obstacles.list.Count;
    for (var i = 0; i < length; i++) {
      var position = new Vector3(GetJSONFloat(obstacles[i], "x"),
                                 GetJSONFloat(obstacles[i], "y") * 50f,
                                 GetJSONFloat(obstacles[i], "z")
                                 );
      var obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity) as GameObject;
      // obstacles.Add(obstacles[i]["id"].ToString(), obstacle);
      // obstacle.GetComponent<ObstacleController>().id = obstacles[i]["id"];
    }
  }

  public void DestroyObstacle (string id)
  {
    // var destroy = obstaclePrefab.GetComponent<NetworkDestroy>();
    // destroy.OnDestroy(id);
    // obstacles["id"].SetActive(false);
    // obstacles.Remove(id);
  }

  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

}