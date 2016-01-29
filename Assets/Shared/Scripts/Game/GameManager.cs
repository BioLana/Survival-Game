using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private bool _isPaused = false;
	private bool _playerPause = false;
	private CursorLockMode lockMode = CursorLockMode.Locked;

	public GameObject player;
	PlayerRotationInput playerRotationScript;
	PlayerMovementInput playerMovementScript;

	public bool GamePaused {
		get {
			return _isPaused;
		}
		set {
			_isPaused = value;
			Cursor.visible = value;
			if (value) {
				lockMode = CursorLockMode.None;
			} else if (!value) {
				lockMode = CursorLockMode.Confined;
			}
			Cursor.lockState = lockMode;
		}
	}

	public bool PlayerPause {
		get {
			return _playerPause;
		}
		set {
			_playerPause = value;
			playerRotationScript.enabled = !value;
			playerMovementScript.enabled = !value;
		}
	}

	// Use this for initialization
	void Start () {
		playerRotationScript = player.GetComponent<PlayerRotationInput> ();
		playerMovementScript = player.GetComponent<PlayerMovementInput> ();
		Debug.Log (playerRotationScript);
		Debug.Log (playerMovementScript);
		GamePaused = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Game Pause")) {
			GamePaused = !GamePaused;
			PlayerPause = !PlayerPause;
		}
	}
}
