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
    if (other.gameObject.CompareTag("Zombie") || other.gameObject.CompareTag("Player")) {
      var otherPlayer = other.gameObject;
      var otherPlayerAttributes = otherPlayer.GetComponent<PlayerAttributes>();
      var playerAttributes = GetComponent<PlayerAttributes>();
      Debug.Log("PlayerMass" + otherPlayerAttributes.playerMass);
      Debug.Log("MyselfMass" + playerAttributes.playerMass);
      // If main player is smaller
      if (otherPlayerAttributes.playerMass > playerAttributes.playerMass) {
        PlayerNetworkController.Kill(playerAttributes.id, GetComponent<Transform>().position.x, GetComponent<Transform>().position.z);
      }

      // If other player is smaller
      else if (otherPlayerAttributes.playerMass < playerAttributes.playerMass){
        PlayerNetworkController.Kill(otherPlayerAttributes.id, other.GetComponent<Transform>().position.x, other.GetComponent<Transform>().position.z);
        Debug.Log(GameAttributes.mainPlayer.GetComponent<PlayerAttributes>().id + " " + playerAttributes.id);
        if(GameAttributes.mainPlayer.GetComponent<PlayerAttributes>().id == playerAttributes.id) particles.Play();
        if (other.gameObject.CompareTag("Zombie")) {
          ZombieSpawner.ZombieCollide(other.gameObject.transform.parent.gameObject);
        }
      }
    }

    if (other.gameObject.CompareTag("Obstacle")) {
      var playerId = GetComponent<PlayerAttributes>().id;
      var obstacleId = other.GetComponent<ObstacleController>().id;
      // Debug.Log(id);
      // other.gameObject.transform.parent.gameObject.GetComponent<ObstaclesController>().ToggleState(obstacleId);
      KnickKnackNetworkController.ObstacleCollision(playerId, obstacleId);
      GetComponent<PlayerMovement>().HitObstacle();
    }

    if (other.gameObject.CompareTag("Food")) {
      var playerId = GetComponent<PlayerAttributes>().id;
      var foodId = other.GetComponent<FoodController>().id;
      // Debug.Log(id);
      other.gameObject.transform.parent.gameObject.GetComponent<FoodsController>().ToggleState(foodId);
      KnickKnackNetworkController.FoodEaten(playerId, foodId);
    }
  }
}
