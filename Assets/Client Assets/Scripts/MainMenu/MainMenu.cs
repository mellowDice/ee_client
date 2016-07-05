using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  public int nextScene = 1;
  public bool changeScenes = true;
  public Animator fadeToBlack;
  public AnimationClip fadeClip;

  private Panels panel;
	// Use this for initialization
	void Awake () {
    panel = GetComponent<Panels>();
	}
	
	// Update is called once per frame
	public void StartButton () {
    Invoke("DelayedLoad", fadeClip.length * 0.5f);
    fadeToBlack.SetTrigger("fade");
  }

  void LoadDelayed () {
    panel.HideMenu();
    SceneManager.LoadScene(nextScene);
  }
}
