using UnityEngine;
using System.Collections;
// using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour {

  public GameObject prefab;
  private float timer;

  // Dictionary<string, GameObject> zombiePlayers;
	// Use this for initialization
	void Awake () {
    // zombiePlayers = new Dictionary<string, GameObject>();
	  timer = Time.time + 3;
	}

  void Update () {
    if (Input.GetKeyDown(KeyCode.W)) {
      Instantiate(prefab, new Vector3(Random.value * 240 + 1, 50, Random.value * 240 + 1), Quaternion.identity);
    }
    var count = GameObject.FindGameObjectsWithTag("Zombie");
    if ((timer < Time.time) && (count.Length < 15)) {
      var zombie = GameObject.Instantiate(prefab, new Vector3(Random.value * 240 + 1, 50, Random.value * 240 + 1), Quaternion.identity) as GameObject;
      zombie.transform.parent = transform;
      zombie.GetComponent<PlayerAttributes>().playerMass = Random.Range(1, 200);
      // zombiePlayers.Add(timer.ToString(), zombie);
      timer = Time.time + 3;
    }
    // Debug.Log(count.Length);
  }

  public void ZombieCollide (GameObject child) {
    Destroy(child);
  }
}
