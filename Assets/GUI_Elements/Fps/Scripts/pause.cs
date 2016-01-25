using UnityEngine;
using System.Collections;

public class pause : MonoBehaviour {
	public GameObject camera;
	public bool paused = false;
	// Update is called once per frame
	void Update () {// this is the script that you would acces if you wanted to display gui when were paused but to make it work properly
		//			   you may need to also check if the inventory is closed as when it is open it calls this script and sets paused to true
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			paused = !paused;
		}

		if (paused == true) {
			transform.GetComponent<SmoothMouseLook> ().enabled = false;
			camera.transform.GetComponent<SmoothMouseLook> ().enabled = false;
			Screen.lockCursor = false;
			Cursor.visible = true;
		}
		else{
			transform.GetComponent<SmoothMouseLook> ().enabled = true;
			camera.transform.GetComponent<SmoothMouseLook> ().enabled = true;
			Screen.lockCursor = true;
			Cursor.visible = false;
		}
		}
}
