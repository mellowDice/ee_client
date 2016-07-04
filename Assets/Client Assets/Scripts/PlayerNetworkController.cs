using UnityEngine;
using SocketIO;
using System.Collections;
using System.Collections.Generic;

public class PlayerNetworkController : MonoBehaviour {

  static SocketIOComponent socket;
  static Dictionary<string, GameObject> players;
  public GameObject playerPrefab;


  ////////////////////////////
  // Unity Built-in Methods //
  ////////////////////////////

  void Start () {
    socket = NetworkController.socket;
    socket.On("spawn", OnOtherPlayerSpawn);
    socket.On("onEndSpawn", OnOtherPlayerDespawn);
    socket.On("player_killed", OnKilled);
    socket.On("otherPlayerLook", OnOtherPlayerLook);
    socket.On("otherPlayerStateInfo", OnOtherPlayerStateReceived);
    socket.On("player_mass_update", OnPlayerMassUpdate);
    players = new Dictionary<string, GameObject> ();
  }


  ///////////////////////////////////
  // Methods Related to Any Player //
  ///////////////////////////////////

  public static void Kill(string playerID, float xPosition, float zPosition) {
    var j = new JSONObject();
    j.AddField("id", playerID);
    j.AddField("x_position", xPosition);
    j.AddField("z_position", zPosition);
    socket.Emit("kill_player", j);
    Debug.Log("kill player " + j["id"].ToString());
  }

  public static void OnKilled(SocketIOEvent e) {
    GameObject killedPlayer;
    if(players.TryGetValue(GetJSONString(e.data, "id"), out killedPlayer)) {
      Destroy(killedPlayer, 0.0f);
    }
  }


  ////////////////////////////////////
  // Methods Related to Main Player //
  ////////////////////////////////////

  public static void InstantiateMainPlayer(string id, float mass) {
    players.Add(id, GameAttributes.mainPlayer);
    GameAttributes.mainPlayer.GetComponent<PlayerAttributes>().id = id;
    GameAttributes.mainPlayer.GetComponent<PlayerAttributes>().playerMass = mass;
    Debug.Log("id " + id);
  }

  public static void OnPlayerMassUpdate(SocketIOEvent e) {
    var id = e.data["id"].ToString();
    var mass = GetJSONFloat(e.data, "mass");
    players[id].GetComponent<PlayerAttributes>().playerMass = mass;
  }
  public static void Die(SocketIOEvent e) {

  }

  // LOOK: Sends data about direction player is facing to server
  public void Look (Vector3 direction) {
    var j = new JSONObject(JSONObject.Type.OBJECT);

    // What direction the user is facing
    j.AddField("look_x", direction.x);
    j.AddField("look_y", direction.y);
    j.AddField("look_z", direction.z);

    socket.Emit("look", j);
  }

  public void Boost () {
    socket.Emit("Boost", new JSONObject());
  }

  // PLAYER STATE DATA SEND: Sends data about current player state to server
  public void PlayerStateSend (Vector3 position, Vector3 velocity, Vector3 angularVelocity, Quaternion rotation) {
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

    // User angular velocity
    j.AddField("rotation_x", rotation.x);
    j.AddField("rotation_y", rotation.y);
    j.AddField("rotation_z", rotation.z);
    j.AddField("rotation_w", rotation.w);

    // Add angular position and velocity
    socket.Emit("player_state_reconcile", j);
  }


  ///////////////////////////////////////
  // Methods Relating to Other Players //
  ///////////////////////////////////////

  // ON OTHER PLAYER SPAWN: Creates the player when server sends message of new player
  void OnOtherPlayerSpawn(SocketIOEvent e) {
    NetworkController.OnReady(delegate() {
      var player = Instantiate(playerPrefab);
      player.GetComponent<Transform>().position = new Vector3(125f, -50f, 125f); // Drop below the map, will be corrected upon start
      players.Add(GetJSONString(e.data, "id"), player);
      player.GetComponent<PlayerAttributes>().id = GetJSONString(e.data, "id");
      Debug.Log("MASSD " + GetJSONFloat(e.data, "mass"));
      player.GetComponent<PlayerAttributes>().playerMass = GetJSONFloat(e.data, "mass");
    });
  }

  // ON OTHER PLAYER DESPAWN: Deletes the player when server sends message of player exiting
  void OnOtherPlayerDespawn(SocketIOEvent e) {
    var id = GetJSONString(e.data, "id");
    var player = players[id];
    Destroy(player);
    players.Remove(id);
  }

  // ON OTHER PLAYER LOOK: Acts on message from server about direction other players are facing
  void OnOtherPlayerLook(SocketIOEvent e) {
    var player = players[GetJSONString(e.data, "id")];
    var navigate = player.GetComponent<PlayerMovement>();
    var direction = new Vector3(GetJSONFloat(e.data, "look_x"), GetJSONFloat(e.data, "look_y"), GetJSONFloat(e.data, "look_z"));
    navigate.ReceivePlayerDirection(direction);
  }

  // ON OTHER PLAYER STATE RECEIVED: Acts on message from server about current state of other players
  void OnOtherPlayerStateReceived(SocketIOEvent e) {
    var player = players[GetJSONString(e.data, "id")];
    var navigate = player.GetComponent<PlayerMovement>();
    var position = new Vector3(GetJSONFloat(e.data, "position_x"), GetJSONFloat(e.data, "position_y"), GetJSONFloat(e.data, "position_z"));
    var velocity = new Vector3(GetJSONFloat(e.data, "velocity_x"), GetJSONFloat(e.data, "velocity_y"), GetJSONFloat(e.data, "velocity_z"));
    var angularVelocity = new Vector3(GetJSONFloat(e.data, "angular_velocity_x"), GetJSONFloat(e.data, "angular_velocity_y"), GetJSONFloat(e.data, "angular_velocity_z"));
    var rotation = new Quaternion(GetJSONFloat(e.data, "rotation_x"), GetJSONFloat(e.data, "rotation_y"), GetJSONFloat(e.data, "rotation_z"), GetJSONFloat(e.data, "rotation_w"));
    navigate.ReceivePlayerState(position, velocity, angularVelocity, rotation);
  }


  ////////////////////
  // Helper Methods //
  ////////////////////

  public static float GetJSONFloat (JSONObject data, string key) {
    return float.Parse(data[key].ToString());
  }
  public static string GetJSONString (JSONObject data, string key) {
    return data[key].ToString().Trim('"');
  }
}
