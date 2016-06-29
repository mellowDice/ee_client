using UnityEngine;
using System.Collections;
using SocketIO;

public class NetworkMove : MonoBehaviour {

  public SocketIOComponent socket;

  public void OnMove (Vector3 position) {
    socket.Emit("move", new JSONObject(NetworkController.VectorToJSON(position)));
  }
  public void Look (Vector3 direction) {
    var j = new JSONObject(JSONObject.Type.OBJECT);

    // For backwards compatability -- remove when everyone has new client
    j.AddField("x", direction.x);
    j.AddField("y", direction.y);
    j.AddField("z", direction.z);

    // What direction the user is facing
    j.AddField("look_x", direction.x);
    j.AddField("look_y", direction.y);
    j.AddField("look_z", direction.z);

    // Add angular position and velocity
    socket.Emit("look", j);
  }
  public void PlayerStateReconcileSend (Vector3 position, Vector3 velocity, Vector3 angularVelocity) {
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
}
