using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

  public GameObject prefab;
	// Use this for initialization
	void Start () {
	
	}

  void Update () {
    if (Input.anyKeyDown) {
      Instantiate(prefab, new Vector3(Random.value * 100 + 1, Random.value * 100 + 1, Random.value * 100 + 1), Quaternion.identity);
    }
  }
}
