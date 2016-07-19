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
  public GameObject _floor;
  public static GameObject floor;
  public LayerMask _myLayerMask;
  public static LayerMask myLayerMask;
  public ParticleSystem _myParticleSystem;
  public static ParticleSystem myParticleSystem;

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
    floor = _floor;
    camera = VR ? GVR.GetComponentInChildren<Camera>() : NonVRCamera;
    myLayerMask = _myLayerMask;
    myParticleSystem = _myParticleSystem;
  }

  void Start() {
    VRGroup.SetActive(VR);
    NonVRGroup.SetActive(!VR);
    floor.SetActive(disableLandscape);
  }

  public static float GetLandscapeY(float x, float z) {
    RaycastHit hit;
    if (Physics.Raycast(new Vector3(x, 50, z), Vector3.down, out hit, 1000f, myLayerMask)) {
      return hit.point.y;
    }
    Debug.Log("NO RAYCAST HIT");
    return 0;
  }
}
