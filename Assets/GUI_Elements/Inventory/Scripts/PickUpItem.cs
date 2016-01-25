using UnityEngine;
using System.Collections;

public class PickUpItem : MonoBehaviour {
	Inventory inventory;
	public KeyCode interact = KeyCode.F;
	public float pickupDistance = 5f;
	// Update is called once per frame
	void Start()// when i work on multiplayer support it will no longer use tags as its realy just a pain
	{
		if (GameObject.FindGameObjectWithTag ("Inventory")) {
			inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory> ();
		} else {
			Debug.Log("Hmm does not look you have the canvas object in your scene. can you add that for me?");
		}
	}
	void Update () {
		if (Input.GetKeyDown (interact)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out hit,pickupDistance))
			{
				if(hit.transform.tag == "Item")
				{
					inventory.addExistingItem(hit.transform.GetComponent<DroppedItem>().item);
					Destroy(hit.transform.gameObject);
				}
			}
		}
	}
}
