using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  public int nextScene = 1;
  public bool changeScenes = true;
  public Animator fadeToBlack;
  public AnimationClip fadeClip;
	// Use this for initialization
	
	// Update is called once per frame
	public void StartButton () {
    Invoke("DelayedLoad", fadeClip.length * 0.5f);
    fadeToBlack.SetTrigger("fade");
  }

  void LoadDelayed () {
    SceneManager.LoadScene(nextScene);
  }
}
