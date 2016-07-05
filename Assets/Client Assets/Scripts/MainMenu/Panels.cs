using UnityEngine;
using System.Collections;

public class Panels : MonoBehaviour {

  public GameObject menuPanel;
  public GameObject instructionsPanel;

  public void ShowMenu() {
  	menuPanel.SetActive(true);
  }

	public void HideMenu() {
		menuPanel.SetActive(false);
	}

	public void ShowInstructions() {
		instructionsPanel.SetActive(true);
	}

	public void HideInstructions() {
		instructionsPanel.SetActive(false);
	}
}
