using UnityEngine;
using System.Collections;

public class PlayerMovementVR : PlayerMovement {
  public GameObject gvrmain;
  private SphereCollider sphereCollider;

  // Use this for initialization
  public override void Start () {
    cam = new Camera(); // new camera to avoid error
    sphereCollider = GetComponent<SphereCollider>();
    base.Start();
  }

  override public Vector3 getDirection() {
    var ray = gvrmain.GetComponentInChildren<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    return ray.direction;
  }

  override public void setCameraPosition() {
    var radius = sphereCollider.radius;
    gvrmain.GetComponent<Transform>().position = GetComponent<Transform>().position + new Vector3(0,radius,0);    
  }
}
