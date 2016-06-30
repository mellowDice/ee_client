using UnityEngine;
using System.Collections;

// TODO: Just drag grvmain -> camera into the scene inspector so the cam variable doesn't need to be overridden
public class PlayerMovementVR : PlayerMovement {
  public GameObject gvrmain;

  ////////////////////////////
  // Unity Built-in Methods //
  ////////////////////////////

  // START: Camera used now is GRVMAIN Camera
  public override void Start () {
    base.Start();
    cam = gvrmain.GetComponentInChildren<Camera>();
  }


  //////////////////////
  // Override Methods //
  //////////////////////

  // SET CAMERA POSTION: Uses GVRMAIN in place of single camera
  override public void setCameraPosition() {
    var radius = GetComponent<SphereCollider>().radius;
    gvrmain.GetComponent<Transform>().position = GetComponent<Transform>().position + new Vector3(0,radius,0);    
  }
}
