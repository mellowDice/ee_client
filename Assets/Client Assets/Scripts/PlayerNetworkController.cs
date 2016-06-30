using UnityEngine;
using SocketIO;
using System.Collections;
using System.Collections.Generic;

public class PlayerNetworkController : MonoBehaviour {

  public SocketIOComponent socket;
  Dictionary<string, GameObject> players;
  public GameObject playerPrefab;
  public NetworkController networkController;


  ////////////////////////////
  // Unity Built-in Methods //
  ////////////////////////////

  void Start () {
    socket.On("spawn", OnOtherPlayerSpawn);
    socket.On("onEndSpawn", OnOtherPlayerDespawn);
    socket.On("otherPlayerLook", OnOtherPlayerLook);
    socket.On("otherPlayerStateInfo", OnOtherPlayerStateReceived);
    players = new Dictionary<string, GameObject> ();

    networkController.OnReady(delegate() {
      GameObject.Find("Player").GetComponent<Rigidbody>().useGravity = true;
    });

  }


  ////////////////////////////////////
  // Methods Related to Main Player //
  ////////////////////////////////////

  // LOOK: Sends data about direction player is facing to server
  public void Look (Vector3 direction) {
    var j = new JSONObject(JSONObject.Type.OBJECT);

    // What direction the user is facing
    j.AddField("look_x", direction.x);
    j.AddField("look_y", direction.y);
    j.AddField("look_z", direction.z);

    socket.Emit("look", j);
  }

  // PLAYER STATE DATA SEND: Sends data about current player state to server
  public void PlayerStateSend (Vector3 position, Vector3 velocity, Vector3 angularVelocity) {
    var j = new JSONObject(JSONObject.Type.OBJECT);

    // User position
    j.AddField("position_x", position.x);
    j.AddField("position_y", position.y);
    j.AddField("position_z", position.z);

    // User velocity
    j.AddField("velocity_x", velocity.x);
    j.AddField("velocity_y", velocity.y);
    j.AddField("velocity_z", velocity.z);

    // User angular velocity
    j.AddField("angular_velocity_x", angularVelocity.x);
    j.AddField("angular_velocity_y", angularVelocity.y);
    j.AddField("angular_velocity_z", angularVelocity.z);

    // Add angular position and velocity
    socket.Emit("player_state_reconcile", j);
  }


  ///////////////////////////////////////
  // Methods Relating to Other Players //
  ///////////////////////////////////////

  // ON OTHER PLAYER SPAWN: Creates the player when server sends message of new player
  void OnOtherPlayerSpawn(SocketIOEvent e) {
    networkController.OnReady(delegate() {
      var player = Instantiate(playerPrefab);
      player.GetComponent<Transform>().position = new Vector3(125f, -50f, 125f); // Drop below the map, will be corrected upon start
      players.Add(e.data["id"].ToString(), player);
    });
  }

  // ON OTHER PLAYER DESPAWN: Deletes the player when server sends message of player exiting
  void OnOtherPlayerDespawn(SocketIOEvent e) {
    var id = e.data["id"].ToString();
    var player = players[id];
    Destroy(player);
    players.Remove(id);
  }

  // ON OTHER PLAYER LOOK: Acts on message from server about direction other players are facing
  void OnOtherPlayerLook(SocketIOEvent e) {
    var player = players[e.data["id"].ToString()];
    var navigate = player.GetComponent<PlayerMovement>();
    var direction = new Vector3(GetJSONFloat(e.data, "look_x"), GetJSONFloat(e.data, "look_y"), GetJSONFloat(e.data, "look_z"));
    navigate.ReceivePlayerDirection(direction);
  }

  // ON OTHER PLAYER STATE RECEIVED: Acts on message from server about current state of other players
  void OnOtherPlayerStateReceived(SocketIOEvent e) {
    Debug.Log("received player state");
    var player = players[e.data["id"].ToString()];
    var navigate = player.GetComponent<PlayerMovement>();
    var position = new Vector3(GetJSONFloat(e.data, "position_x"), GetJSONFloat(e.data, "position_y"), GetJSONFloat(e.data, "position_z"));
    var velocity = new Vector3(GetJSONFloat(e.data, "velocity_x"), GetJSONFloat(e.data, "velocity_y"), GetJSONFloat(e.data, "velocity_z"));
    var angularVelocity = new Vector3(GetJSONFloat(e.data, "angular_velocity_x"), GetJSONFloat(e.data, "angular_velocity_y"), GetJSONFloat(e.data, "angular_velocity_z"));
    navigate.ReceivePlayerState(position, velocity, angularVelocity);
  }


  ////////////////////
  // Helper Methods //
  ////////////////////

  float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString().Substring(1, -1));
  }
}
