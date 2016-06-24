using UnityEngine;
using System.Collections;

public class NavigatePosition : MonoBehaviour {
  
  private CharacterController agent;
  private Vector3 position;
  private float speed = 10.0f;
  // private float gravity = 20.0f;
  private bool moving = false;
  // private Vector3 moveDirection = Vector3.zero;
  public Camera cam1;
  public Camera cam2;


	void Start () {
    agent = GetComponent<CharacterController>();
	}

  void Update() {
    if (moving) {
      var offset = position - transform.position + new Vector3(0,2,0);
      if (offset.magnitude > 1f) {
        // Debug.Log("offset" + position);
        // Debug.Log("CurrPos" + transform.position);
        offset = offset.normalized*speed;
        agent.Move(offset * Time.deltaTime);
      } else {
        moving = false;
      }
    }
    RaycastHit hit = new RaycastHit();
    if (Physics.Raycast(transform.position, -Vector3.up, out hit)) {
      if (transform.position.y < (hit.point.y + 1)) {
        transform.position += Vector3.up * 7.5f * Time.deltaTime;
      } else if (transform.position.y > (hit.point.y + 5)) {
        transform.position += Vector3.down * 7.5f * Time.deltaTime;
      }
    }
  }

  public void NavigateTo (Vector3 nposition) {
    position = nposition;
    moving = true;
  }
}
