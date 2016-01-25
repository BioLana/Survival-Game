using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Dragable : MonoBehaviour {

	public void Drag()
	{
		transform.position = Input.mousePosition;
	}
}
