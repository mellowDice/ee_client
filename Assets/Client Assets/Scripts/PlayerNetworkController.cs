using UnityEngine;
using SocketIO;
using System.Collections;
using System.Collections.Generic;

public class PlayerNetworkController : MonoBehaviour {

  static SocketIOComponent socket;
  static Dictionary<string, GameObject> players;
  static PlayerAttributes mainPlayerAttributes;
  public GameObject playerPrefab;


  ////////////////////////////
  // Unity Built-in Methods //
  ////////////////////////////

  void Start () {
    if (socket == null) {
      socket = NetworkController.socket;
      socket.On("spawn", OnOtherPlayerSpawn);
      socket.On("onEndSpawn", OnOtherPlayerDespawn);
      socket.On("player_killed", OnKilled);
      socket.On("otherPlayerLook", OnOtherPlayerLook);
      socket.On("otherPlayerStateInfo", OnOtherPlayerStateReceived);
      socket.On("other_player_collided_with_obstacle", OnOtherPlayerCollidedWithObstacle);
      socket.On("player_mass_update", OnPlayerMassUpdate);
      socket.On("initialize_main_player", InitializeMainPlayer);
      socket.On("initialize_zombie_player", InitializeZombiePlayer);
      players = new Dictionary<string, GameObject> ();
      mainPlayerAttributes = GameAttributes.mainPlayer.GetComponent<PlayerAttributes>();
      NetworkController.OnReady(delegate() {
        socket.Emit("initialize_main", new JSONObject());
        Debug.Log("Initializing");
      });
    }
  }


  ///////////////////////////////////
  // Methods Related to Any Player //
  ///////////////////////////////////

  // KILL: Emit kill message when any player is killed
  public static void Kill(string playerID, float xPosition, float zPosition) {
    var j = new JSONObject();
    j.AddField("id", playerID);
    j.AddField("x_position", xPosition);
    j.AddField("z_position", zPosition);
    socket.Emit("kill_player", j);
  }

  // ON KILLED: When notified that any player is killed
  public static void OnKilled(SocketIOEvent e) {
    GameObject killedPlayer;
    string id = GetJSONString(e.data, "id");
    if(players.TryGetValue(id, out killedPlayer)) {
      if(killedPlayer != GameAttributes.mainPlayer) {
        Destroy(killedPlayer, 0.0f);
      } else {
        killedPlayer.SetActive(false);
        socket.Emit("initialize_main", new JSONObject());
      }
      players.Remove(id);
    }
  }


  // ON KILLED: When notified that any player is killed
  public static void OnOtherPlayerCollidedWithObstacle(SocketIOEvent e) {
    GameObject collidedPlayer;
    string playerId = GetJSONString(e.data, "player_id");
    if(players.TryGetValue(playerId, out collidedPlayer)) {
      collidedPlayer.GetComponent<PlayerMovement>().HitObstacle();
    }
  }

  // ON PLAYER MASS UPDATE: Updates the mass of a player
  public static void OnPlayerMassUpdate(SocketIOEvent e) {

    Debug.Log("MASS UPDATE");
    var id = GetJSONString(e.data, "id");
    var mass = GetJSONFloat(e.data, "mass");
    Debug.Log("MASS UPDATE: " + id + " " + mass);
    GameObject massChangePlayer;
    if(players.TryGetValue(id, out massChangePlayer)) {
      massChangePlayer.GetComponent<PlayerAttributes>().id = id;
      massChangePlayer.GetComponent<PlayerAttributes>().playerMass = mass;
    }
  }


  ///////////////////////////////////////
  // Methods Related to Zombie Players //
  ///////////////////////////////////////

  public void InitializeZombiePlayer(SocketIOEvent e) {
    var id = GetJSONString(e.data, "id");
    var mass = GetJSONFloat(e.data, "mass");
    var x = GetJSONFloat(e.data, "x");
    var z = GetJSONFloat(e.data, "z");
    NetworkController.OnReady(delegate() {
      var player = Instantiate(playerPrefab);

      // initialize with location eventually
      players.Add(id, player);
      player.GetComponent<Transform>().position = new Vector3(x, 30f, z); // Drop below the map, will be corrected upon start
      player.GetComponent<PlayerAttributes>().id = id;
      player.GetComponent<PlayerAttributes>().playerMass = mass;
      player.GetComponent<PlayerAttributes>().zombiePlayer = true;
    });
  }


  ////////////////////////////////////
  // Methods Related to Main Player //
  ////////////////////////////////////

  // INITIALIZE MAIN PLAYER: Sets up player ID and mass upon game start
  public static void InitializeMainPlayer(SocketIOEvent e) {
    var player = GameAttributes.mainPlayer;
    var id = GetJSONString(e.data, "id");
    var mass = GetJSONFloat(e.data, "mass");
    var x = GetJSONFloat(e.data, "x");
    var z = GetJSONFloat(e.data, "z");
    if (player.activeSelf == false) { 
      player.SetActive(true);
    }
    players.Add(id, GameAttributes.mainPlayer);
    player.GetComponent<Transform>().position = new Vector3(x, 30f, z); // Drop below the map, will be corrected upon start
    player.GetComponent<PlayerAttributes>().id = id;
    player.GetComponent<PlayerAttributes>().playerMass = mass;
  }

  // LOOK: Broadcasts direction main player is facing
  public void Look (Vector3 direction) {
    var j = new JSONObject(JSONObject.Type.OBJECT);
    j.AddField("look_x", direction.x);
    j.AddField("look_y", direction.y);
    j.AddField("look_z", direction.z);
    socket.Emit("look", j);
  }

  // BOOST: Broadcasts that main player is using boost
  public void Boost (string playerId) {
    Debug.Log("EMITTING BOOOOOIOOST");
    var j = new JSONObject(JSONObject.Type.OBJECT);
    j.AddField("player_id", playerId);
    socket.Emit("boost", j);
  }

  // PLAYER STATE DATA SEND: Broadcasts main player state, so other players can reconcile
  public void PlayerStateSend (Vector3 position, Vector3 velocity, Vector3 angularVelocity, Quaternion rotation, string id) {
    var j = new JSONObject(JSONObject.Type.OBJECT);

    // User ID
    j.AddField("id", id);

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
      player.GetComponent<Transform>().position = new Vector3(Random.Range(0,1000), -50f, Random.Range(0,1000)); // Drop below the map, will be corrected upon start
      players.Add(GetJSONString(e.data, "id"), player);
      player.GetComponent<PlayerAttributes>().id = GetJSONString(e.data, "id");

      player.GetComponent<PlayerAttributes>().playerMass = GetJSONFloat(e.data, "mass");
      player.GetComponent<PlayerCollision>().enabled = false;
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
