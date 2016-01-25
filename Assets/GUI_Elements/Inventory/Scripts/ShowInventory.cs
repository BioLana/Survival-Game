using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShowInventory : MonoBehaviour {
	public GameObject inventory;
	public bool showInv;
	public KeyCode openInventory = KeyCode.Tab;
	WeaponHandler weaponhandler;
	Component[] image;
	void Start () {
		weaponhandler = GameObject.FindGameObjectWithTag ("WeaponHandler").GetComponent<WeaponHandler> ();
	}
	
	// Update is called once per frame
	void Update () {
		image = inventory.GetComponentsInChildren<Image>();
		if (Input.GetKeyDown (openInventory)) {
			showInv = !showInv;
			GetComponent<pause>().paused = showInv;
		}
		foreach (Image img in image) {
			img.enabled = showInv;
		}
		if(weaponhandler.isGunEquiped && showInv == true){// remove this if you dont want the weapon to be unequipped.
			weaponhandler.UnequipGun();
		}
	}
}
