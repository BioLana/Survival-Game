using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {
	public Vector3 firstPos;
	public Vector3 LastPos;
	float timer;

	void Update()
	{
		timer -= Time.deltaTime;
		transform.position = Vector3.Lerp (LastPos, firstPos, .5f + (Mathf.Sin (timer * 0.5f) / 2f));
	}
}
