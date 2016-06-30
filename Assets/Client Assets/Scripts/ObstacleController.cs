using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObstacleController : MonoBehaviour {

  public GameObject obstaclePrefab;

  // Dictionary<string, string> obstacles = new Dictionary<string, string>();

  public void CreateObstacle (JSONObject obstacles)
  {
    var length = obstacles.list.Count;

    for (var i = 0; i < length; i++) {
      var position = new Vector3(GetJSONFloat(obstacles[i], "x"),
                                 GetJSONFloat(obstacles[i], "y") * 50f,
                                 GetJSONFloat(obstacles[i], "z")
                                 );
      var obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity) as GameObject;
    }
  }
  // void DestroyFood (string id)
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
  //   obstacles.Remove(id);
  // }
  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Replace("\"", ""));
  }

}