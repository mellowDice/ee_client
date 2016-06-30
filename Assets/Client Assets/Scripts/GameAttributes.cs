using UnityEngine;
using System.Collections;

public class GameAttributes : MonoBehaviour {

  // Settings
  public bool _VR = true;
  public static bool VR;
  public bool _computerControlledMainPlayer = false;
  public static bool computerControlledMainPlayer;

  // Inputs
  public GameObject VRGroup;
  public GameObject NonVRGroup;

  void Awake() {
    VR = _VR;
    computerControlledMainPlayer = _computerControlledMainPlayer;
  }
  void Start() {
    VRGroup.SetActive(VR);
    NonVRGroup.SetActive(!VR);
  }

}
