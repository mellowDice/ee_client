using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
  ParticleSystem particles;
  PlayerMovement playerMovement;

  void Awake() {
    playerMovement = GetComponent<PlayerMovement>();
  }

  void Start() {
    particles = GameAttributes.camera.GetComponentInChildren<ParticleSystem>();
  }
  
  /////////////////////
  //COLLISION CHECKER//
  /////////////////////
	void OnTriggerEnter(Collider other) {
      Debug.Log("---COLLISION--- " + other);

    if (other.gameObject.CompareTag("Zombie") || other.gameObject.CompareTag("Player")) {
      var otherPlayer = other.transform.parent.gameObject;
      var otherPlayerAttributes = otherPlayer.GetComponent<PlayerAttributes>();
      Debug.Log("---COLLISION--- " + otherPlayerAttributes.id);
      var playerAttributes = GetComponent<PlayerAttributes>();

      // If main player is smaller
      if (otherPlayerAttributes.playerMass > playerAttributes.playerMass) {
        PlayerNetworkController.Kill(playerAttributes.id, GetComponent<Transform>().position.x, GetComponent<Transform>().position.z);
      }

      // If other player is smaller
      else if (otherPlayerAttributes.playerMass < playerAttributes.playerMass){
        PlayerNetworkController.Kill(otherPlayerAttributes.id, other.GetComponent<Transform>().position.x, other.GetComponent<Transform>().position.z);
        if(GameAttributes.mainPlayer == gameObject) particles.Play();
        if (other.gameObject.CompareTag("Zombie")) {
          ZombieSpawner.ZombieCollide(other.transform.parent.gameObject);
        }
      }
    }

    if (other.gameObject.CompareTag("Obstacle")) {
      // decrease player mass fn needed
      // other.gameObject.GetComponent<ObstacleController>().DestroyObstacle(other.id);
    }

    // if (other.gameObject.CompareTag("Food")) {
    //   // increase player mass
    //   foodPrefab.GetComponent<FoodController>().DestroyFood(other.id);
    // }

  }
}
