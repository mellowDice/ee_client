using UnityEngine;
using System.Collections;

public class FoodController : MonoBehaviour {
  public string _id;
  public string id
    { get { return _id; } set { _id = value; } }

  void Start () {
    NewPos();
  }

  public void NewPos () {
    var hit = GameAttributes.RayDown(transform);
    transform.position = new Vector3 (transform.position.x, hit.point.y + 1f, transform.position.z);
  }
}