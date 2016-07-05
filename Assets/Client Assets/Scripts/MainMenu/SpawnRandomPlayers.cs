using UnityEngine;
using System.Collections;

public class SpawnRandomPlayers : MonoBehaviour {

  public GameObject prefab;
  public float height = 20;
  public float deltTime = 2;
  public float area = 20;
  private float timer;

  // Dictionary<string, GameObject> zombiePlayers;
  // Use this for initialization
  void Awake () {
    // zombiePlayers = new Dictionary<string, GameObject>();
    timer = Time.time + deltTime;
  }

  void Update () {
    var count = GameObject.FindGameObjectsWithTag("RandomPlayer");
    if ((timer < Time.time) && (count.Length < 15)) {
      var zombie = GameObject.Instantiate(prefab, new Vector3(Random.Range(-1f, 1f) * area + 1, height, Random.Range(-1f, 1f) * area + 1), Quaternion.identity) as GameObject;
      zombie.transform.parent = transform;
      // zombiePlayers.Add(timer.ToString(), zombie);
      timer = Time.time + 3;
    }
    // Debug.Log(count.Length);
  }

  public void ZombieCollide (GameObject child) {
    Destroy(child);
  }
}
