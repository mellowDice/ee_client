using UnityEngine;
using System.Collections;

public class CameraDirection : MonoBehaviour {


  public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
  public RotationAxes axes = RotationAxes.MouseXAndY;

  private float minimumY = -60F;
  private float maximumY = 60F;

  float rotationY = 0F;

  void Update ()
  {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
    if (Input.GetKeyDown(KeyCode.Space)) {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }
    if (axes == RotationAxes.MouseXAndY) {
      float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X");
       
      rotationY += Input.GetAxis("Mouse Y");
      rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
     
      transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    } else if (axes == RotationAxes.MouseX) {
      transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
    } else {
      rotationY += Input.GetAxis("Mouse Y");
      rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
     
      transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
    }
  }
}
