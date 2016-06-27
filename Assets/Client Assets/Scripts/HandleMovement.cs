using UnityEngine;
using System.Collections;

public class HandleMovement : MonoBehaviour {
	// Update is called once per frame
	void Update () {
    if (Input.GetButtonDown("Fire1")) {
      Clicked();
    }
  }

  void Clicked () {
    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
    RaycastHit hit = new RaycastHit();
    if (Physics.Raycast(ray, out hit, 1000f)) {
      var clickMove = hit.collider.gameObject.GetComponent<ClickMove>();
      clickMove.OnClick(hit.point);
    }
  }
}
