using UnityEngine;
using System.Collections;

public class quit : MonoBehaviour {
	
	void OnMouseDown() {
		Debug.Log ("Quitting APplication");
		Application.Quit ();

	}
	
}
