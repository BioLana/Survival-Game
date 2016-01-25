using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class WeaponHandler : MonoBehaviour {
	public string GunWeWantToEquipped;// this holds the gun we want to be equipped
	public string CurrentlyEquipped;// this holds the currently equipped gun
	public Weapons[] Weapons = new Weapons[1];// an array of weapons

	public int whatGun;// used to determine what gun we are working with
	//public Text text;// used to display the weapons name
	//public InputField inputfield;// used to type in what gun we want 

	public bool isGunEquiped = false;// used to check if we already have a gun

	public void EquipGun()
	{
	UnequipGun();// calling this so if we try to equip a gun when we already have one it will unequipped first. you can remove this if you dont want it
		if (isGunEquiped == false) {// so if we dont have a gun we can equipped one 
			for (whatGun = 0; whatGun < Weapons.Length; whatGun++) {// looping through all of the array
				if (GunWeWantToEquipped == Weapons [whatGun].WeapoName) {// until we find one matching the imputed name
					Weapons [whatGun].WeaponModel.SetActive (true);// set that gun to active
					//text.text = (Weapons [whatGun].WeapoName);// setting the text to display the weapons name
					CurrentlyEquipped = GunWeWantToEquipped;// setting the currently equipped gun 
					GunWeWantToEquipped = "";// clearing the gun we want to equip so we can enter others
					isGunEquiped = true;// setting it so that the script knows we have a gun
					break;// stop the loop
				} else {
					//text.text = "";// if does not find a name matching it will set the text to blank
					UnequipGun();// and we will call unequipped 
				}
			}
		}
	}
	public void UnequipGun()
	{
		if (isGunEquiped == true) {// if a gun is already equipped
			for (whatGun = 0; whatGun < Weapons.Length; whatGun++) {// loop through the array
				if (CurrentlyEquipped == Weapons [whatGun].WeapoName) {//until we find a matching name 
					Weapons [whatGun].WeaponModel.GetComponent<Weapon>().StopCoroutine("Reload");
					Weapons [whatGun].WeaponModel.GetComponent<Weapon>().canFire = true;
					Weapons [whatGun].WeaponModel.GetComponent<Weapon>().reloading= false;
					Weapons[whatGun].WeaponModel.GetComponent<Weapon>().thirdPersonSettings.thirdPersonWraponModel.SetActive(false);
					Weapons [whatGun].WeaponModel.SetActive (false);// disable the gun
					isGunEquiped = false;// set the bool to false
					CurrentlyEquipped ="";// clear currently  equipped
					break;// stop the loop
				} 
			}
		}
	}
}
[System.Serializable]
public class Weapons
{
	public string WeapoName;
	public GameObject WeaponModel;
}
