using UnityEngine;
using System.Collections;

public class GameAttributes : MonoBehaviour {

  // Settings
  public bool _VR = true;
  public static bool VR;
  public bool _computerControlledMainPlayer = false;
  public static bool computerControlledMainPlayer;
  public bool _disableLandscape = false;
  public static bool disableLandscape;
  public GameObject _mainPlayer;
  public static GameObject mainPlayer;
  public static Camera camera;

  // Inputs
  public GameObject VRGroup;
  public GameObject NonVRGroup;
  public Camera NonVRCamera;
  public GameObject GVR;


  void Awake() {
    VR = _VR;
    computerControlledMainPlayer = _computerControlledMainPlayer;
    mainPlayer = _mainPlayer;
    disableLandscape = _disableLandscape;
    camera = VR ? GVR.GetComponentInChildren<Camera>() : NonVRCamera;
  }

  void Start() {
    VRGroup.SetActive(VR);
    NonVRGroup.SetActive(!VR);
  }

}
