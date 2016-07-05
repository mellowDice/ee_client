using UnityEngine;
using System.Collections;

public class ApplicationManager : MonoBehaviour {
	

	public void Quit () {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

  public void Instructions () {
    Debug.Log("Instructions");
  }

  public void StartButton () {
    StartCoroutine(LoadLevel());
  }

  IEnumerator LoadLevel () {
    float fadeTime = GetComponent<Fade>().BeginFade(1);
    yield return new WaitForSeconds(fadeTime);
    Application.LoadLevel(1);
  }
}
