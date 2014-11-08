using UnityEngine;
using System.Collections;

public class mainmanu : MonoBehaviour {

	void OnMouseEnter() {
		guiText.color = Color.red;
	}
	
	void OnMouseExit() {
		
		guiText.color = Color.white;
	}
}
