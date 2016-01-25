using UnityEngine;
using System.Collections;

public class ThirdPerson : MonoBehaviour {
	public Shader transparent;// this is the invisible shadow caster shader provided you can chnage this tho if you dont like shadows
	public Shader Normal; // chnage this to what ever skin shader thing you are using
	public GameObject Model;// this is the player model btw
	public bool thirdPerson;
	public KeyCode thirdPersonToggle = KeyCode.KeypadEnter;
	public Transform FirstPerson;// this is the empty gameobject where your head is
	public Transform Thirdperson;// this is somewhere behind the player for third person
	WeaponHandler wh;
	HeadFollow hf;
	SmoothMouseLook ml;
	float thirdPersonMaxY = 20;
	public Animator anim;

	void Start(){
		wh = GameObject.FindGameObjectWithTag ("WeaponHandler").GetComponent<WeaponHandler> ();
		hf = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<HeadFollow> ();
		ml = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<SmoothMouseLook> ();
	}
	void Update()
	{
		if (wh.isGunEquiped == false && thirdPerson == false) {
			Model.GetComponent<SkinnedMeshRenderer> ().materials [2].shader = Normal;
		} else if(wh.isGunEquiped == true && thirdPerson == false){
			Model.GetComponent<SkinnedMeshRenderer>().materials[2].shader = transparent;
		}
		if (Input.GetKeyDown (thirdPersonToggle)) {
			thirdPerson = !thirdPerson;
		}

		if (thirdPerson == true) {
			hf.target = Thirdperson;
			Model.GetComponent<SkinnedMeshRenderer>().materials[1].shader = Normal;
		Model.GetComponent<SkinnedMeshRenderer>().materials[2].shader = Normal;
			ml.minimumY = -38;
			ml.maximumY = 15;
		}else{
			hf.target = FirstPerson;
			ml.minimumY = -90;
		ml.maximumY = 90;
			Model.GetComponent<SkinnedMeshRenderer>().materials[1].shader = transparent;
		}

		Weapon wtp = wh.Weapons[wh.whatGun].WeaponModel.GetComponent<Weapon>() ;
		if (wtp.thirdPersonSettings.weaponType == Weapon.WeaponType.Pistol && wh.isGunEquiped == true)// if you wish to add more weapon types copy and paste this if statement and change it as neeed
		{
			anim.SetLayerWeight(1,0);
			anim.SetLayerWeight(2,1);
			if (Input.GetButton ("Fire2")) {
				anim.SetBool("Aiming",true);
			} else {
				anim.SetBool("Aiming",false);
			}
		}
		if (wtp.thirdPersonSettings.weaponType == Weapon.WeaponType.Rifle && wh.isGunEquiped == true)
		{
			anim.SetLayerWeight(1,1);
			anim.SetLayerWeight(2,0);
			if (Input.GetButton ("Fire2")) {
				anim.SetBool("Aiming",true);
			} else {
				anim.SetBool("Aiming",false);
			}
		}
		if (wh.isGunEquiped == false) {
			anim.SetLayerWeight(1,0);
			anim.SetLayerWeight(2,0);
		}
	}
	}
