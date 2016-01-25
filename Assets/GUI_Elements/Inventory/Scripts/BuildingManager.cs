using UnityEngine;
using System.Collections;

public class BuildingManager : MonoBehaviour {
	public string BuildingWeWantToEquipped;
	public string CurrentlyEquippedBuilding;
	public int WhatObject;
	public BuilableObjects[] builableObjects = new BuilableObjects[1];
	public bool isBuilding = false;
	public int SlotNumber;
	Inventory inventory;
	void Start()
	{
		if (GameObject.FindGameObjectWithTag ("Inventory")) {
			inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory> ();
		} else {
			Debug.Log("Hmm does not look you have the canvas object in your scene. can you add that for me?");
		}
	}
	void Update()
	{
		if (isBuilding == true) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit,5)) {
				builableObjects[WhatObject].Object.SetActive(true);
				builableObjects[WhatObject].Object.transform.position = hit.point;
				builableObjects[WhatObject].Object.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
				if(Input.GetKeyDown(KeyCode.Mouse0))
				{
					GameObject itemAsGameobject = (GameObject)Instantiate (builableObjects[WhatObject].instantiatedObject, builableObjects[WhatObject].Object.transform.position, Quaternion.identity);
					BuildingOff();
					inventory.Items[SlotNumber].itemValue -=1;
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.H)) {
			BuildingWeWantToEquipped = "CampFire";
			BuildingOn();
		}
	}
	public void BuildingOn()
	{
		if (isBuilding == false) {
			for (WhatObject = 0; WhatObject < builableObjects.Length; WhatObject++) {
				if (BuildingWeWantToEquipped == builableObjects[WhatObject].Name)
				{
					builableObjects[WhatObject].Object.SetActive(true);
					CurrentlyEquippedBuilding = BuildingWeWantToEquipped;
					BuildingWeWantToEquipped = "";
					isBuilding = true;
					break;
				}else{
					BuildingOff();
				}
			}
		}
	}
	public void BuildingOff()
	{
		if (isBuilding == true) {
			for (WhatObject = 0; WhatObject < builableObjects.Length; WhatObject++) {
				if (CurrentlyEquippedBuilding == builableObjects[WhatObject].Name){
					builableObjects[WhatObject].Object.SetActive(false);
					isBuilding = false;
					CurrentlyEquippedBuilding = "";
					break;
				}
			}
		}
	}

}
[System.Serializable]
public class BuilableObjects
{
	public string Name;
	public GameObject Object;
	public GameObject instantiatedObject;
}