using UnityEngine;
using System.Collections;

public class Triggers : MonoBehaviour {

	void OnTriggerEnter(Collider collisionInfo) {
		if (collisionInfo.gameObject.tag == "Ladder") {
			GetComponentInParent<CharacterControlls>().onLadder = true;
		} 
		if (collisionInfo.gameObject.tag == "Water") {
			GetComponentInParent<CharacterControlls>().inWater = true;
		} 
		if (collisionInfo.gameObject.tag == "MovingPlatform") {
			transform.parent = collisionInfo.gameObject.transform;
		} 
	}
	void OnTriggerExit(Collider collisionInfo) {
		if (collisionInfo.gameObject.tag == "Ladder") {
			GetComponentInParent<CharacterControlls>().onLadder = false;
		}
		if (collisionInfo.gameObject.tag == "Water") {
			GetComponentInParent<CharacterControlls>().inWater = false;
		}
		if (collisionInfo.gameObject.tag == "MovingPlatform") {
			transform.parent = null;
		} 
	}
}
