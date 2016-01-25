using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Weapon : MonoBehaviour {
	[HideInInspector]
	public int slotNumber = -1;

	public Camera PlayerCam;
	//public Text CurrentAmmot;

	[System.Serializable]
	public class GraphicsAndSounds
	{
		public Transform FirstPersonbarrel;
		public AudioClip Shoot;
		public AudioClip OutOfAmmo;
		public AudioClip Reloads;
		public int destroyDelay = 5;
		public GameObject ObjectWithAnimationsOnIt;
		public bool hasFireAnimation;
		public AnimationClip FirstPersonFireAnimation;
		public bool hasReloadAnimation;
		public AnimationClip FirstPersonReloadAnimation;
		public HitSounds Grass = new HitSounds();
		public HitSounds Rock = new HitSounds();
		public HitSounds Sand = new HitSounds();
		public HitSounds Snow = new HitSounds();
		public HitSounds Metal = new HitSounds();
		public HitSounds Wood = new HitSounds();
	}
	public GraphicsAndSounds graphicsAndSounds = new GraphicsAndSounds();
	[System.Serializable]
	public class HitSounds{
		public AudioClip[] sounds;
		public GameObject[] Bullethole;
	}
	[System.Serializable]
	public class WeaponStats
	{
		public float Range = 100f;
		public int ClipSize =30;
		public int CurrentAmmo = 30;
		public float reloadTime;
		public float fireInterval = 0.1f;
		public float FireDelay = 0f;
		public FireMode fireMode;
		public float Damage = 0.2f;
		public int NumberOfShotsInBurst = 3;
		public float spreadFactor = 0.05f;
		public float ShotgunNumberOfBits = 10;
	}
	public WeaponStats weaponStats = new WeaponStats();


	[System.Serializable]
	public class ZoomSettings
	{
		public float zoomFov = 30;
		public float deafultFov = 60;
		public float smoothTime = 5F;
		public Vector3 zoomimgPos;
		public bool ToggleAim;
	}
	public ZoomSettings zoomSettings = new ZoomSettings();




	[System.Serializable]
	public class RecoilSettings
	{

		public float lerpTime = 10;
		public float rotateSpeed = 0.1F;
		public float RecoilResetSpeed = 1F;
		public float CameraKick = 4;
		public Vector3 RecoilPosition;
		public Vector3 zoomedRecoilPosition;
	}
	public RecoilSettings recoilSettings = new RecoilSettings();

	[System.Serializable]
	public class ThirdPersonSettings
	{
		public WeaponType weaponType;
		public Animator anim;
		public GameObject thirdPersonWraponModel;
		public GameObject firstPersonWraponModel;
		public Transform thirdPersonBarrelPosition;
		[HideInInspector]
		public ThirdPerson thirdPersonScript;
		[HideInInspector]
		public bool thirdPerson;
	}
	public ThirdPersonSettings thirdPersonSettings = new ThirdPersonSettings();
	public enum WeaponType
	{
		Rifle,
		Pistol,
		melee
	}
	[HideInInspector]
	public bool reloading = false;
	float fireTime;
	bool isBurstFiring = false;
	bool isFiring;
	Vector3 targetRotation;
	[HideInInspector]
	public bool canFire = false;
	[HideInInspector]
	public bool aiming;
	Transform barrel;
	Texture tex;
	Inventory inven;
	AudioClip[] hitSoundsTemp;
	GameObject[] tempBulletHole;
	void Start ()
	{
		thirdPersonSettings.thirdPersonScript = transform.GetComponentInParent<ThirdPerson> ();
		inven = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
	}
	void Update () {
		thirdPersonSettings.thirdPerson = thirdPersonSettings.thirdPersonScript.thirdPerson;
		inven.Items[slotNumber].currentAmmo = weaponStats.CurrentAmmo;
			if (fireTime <= 0 && !isBurstFiring) {
			switch (weaponStats.fireMode) {
				case FireMode.Semi:
					if (Input.GetButtonDown ("Fire1")) {
						isFiring = true;
						Fire ();
					StartCoroutine(FireRaycast());
					fireTime = weaponStats.fireInterval;
					} else {
						isFiring = false;
					}
					break;
				case FireMode.Burst:
					if (Input.GetButtonDown ("Fire1")) {
						isFiring = true;
					StartCoroutine (BurstFire (weaponStats.fireInterval,weaponStats.NumberOfShotsInBurst));
					fireTime = weaponStats.fireInterval;
					} else {
						isFiring = false;
					}
					break;
				case FireMode.FullAuto:
					if (Input.GetButton ("Fire1")) {
						isFiring = true;
						Fire ();
					fireTime = weaponStats.fireInterval;
					} else {
						isFiring = false;
					}
					break;
				case FireMode.Shotgun:
					if (Input.GetButtonDown ("Fire1")) {
						isFiring = true;
						Fire();
						StartCoroutine(ShotgunFire());
						fireTime = weaponStats.fireInterval;
					} else {
						isFiring = false;
					}
					break;
			}
		}

		if (fireTime > 0){
			fireTime -= Time.deltaTime;
		}
		if (zoomSettings.ToggleAim == true) {
			if (Input.GetButtonDown ("Fire2")) {
				aiming = !aiming;
			} 
		} else {
			if (Input.GetButton ("Fire2")) {
				aiming = true;
			} else{
				aiming = false;
			}
		}
		if(isFiring != true){
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0,0,0),recoilSettings.RecoilResetSpeed * Time.deltaTime);
		}

		if (weaponStats.CurrentAmmo >= 1 && GameObject.FindGameObjectWithTag("Player").GetComponent<pause>().paused == false && reloading == false) {
			canFire = true;
		} else {
			canFire = false;
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			StartCoroutine("Reload");
		}
		if (thirdPersonSettings.thirdPerson == true) {
			barrel = thirdPersonSettings.thirdPersonBarrelPosition;
			thirdPersonSettings.firstPersonWraponModel.SetActive (false);
			thirdPersonSettings.thirdPersonWraponModel.SetActive (true);
		} else {
			barrel = graphicsAndSounds.FirstPersonbarrel;
			thirdPersonSettings.firstPersonWraponModel.SetActive (true);
			thirdPersonSettings.thirdPersonWraponModel.SetActive (false);
		}

		if (aiming == true) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, zoomSettings.zoomimgPos, Time.deltaTime * zoomSettings.smoothTime);
			PlayerCam.fieldOfView = zoomSettings.zoomFov;
		} else {
			PlayerCam.fieldOfView = zoomSettings.deafultFov;
		}
	}
	void Fire()
	{
		if (canFire == true) {
			if (graphicsAndSounds.hasFireAnimation == true) {
				graphicsAndSounds.ObjectWithAnimationsOnIt.GetComponent<Animation> ().Play (graphicsAndSounds.FirstPersonFireAnimation.name);
			}
			PlayerCam.GetComponent<SmoothMouseLook> ().rotationY += recoilSettings.CameraKick;
			if (aiming == false){

				transform.localPosition = Vector3.Lerp (transform.localPosition, recoilSettings.RecoilPosition, recoilSettings.lerpTime * Time.deltaTime);
			}else if (aiming == true){
				transform.localPosition = Vector3.Lerp (transform.localPosition, recoilSettings.zoomedRecoilPosition, recoilSettings.lerpTime * Time.deltaTime);
			}

			GetComponent<AudioSource> ().PlayOneShot (graphicsAndSounds.Shoot);
		} else {
			GetComponent<AudioSource> ().PlayOneShot (graphicsAndSounds.OutOfAmmo);
		}
	}
	IEnumerator FireRaycast()
	{
		yield return new WaitForSeconds (weaponStats.FireDelay);
		if (canFire == true) {
		weaponStats.CurrentAmmo -= 1;
		RaycastHit hit;
		if (Physics.Raycast (barrel.transform.position, graphicsAndSounds.FirstPersonbarrel.transform.forward, out hit, weaponStats.Range)) {
			if (hit.transform.gameObject.GetComponent<Health> ()) {
				hit.transform.gameObject.GetComponent<Health> ().health -= weaponStats.Damage;
			}
			if (hit.transform.gameObject.GetComponent<MeshRenderer> ()) {
				tex = hit.transform.gameObject.GetComponent<MeshRenderer> ().material.mainTexture;
			}
			if (hit.transform.tag == "Terrain") {
				tex = getTerrainTextureAt (transform.position);
			}
			if (tex != null) {
				if (tex.name.Contains ("Grass")) {
					hitSoundsTemp = graphicsAndSounds.Grass.sounds;
					tempBulletHole = graphicsAndSounds.Grass.Bullethole;
					PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
				} else if (tex.name.Contains ("Rock")) {
					hitSoundsTemp = graphicsAndSounds.Rock.sounds;
					tempBulletHole = graphicsAndSounds.Rock.Bullethole;
					PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
				} else if (tex.name.Contains ("Metal")) {
					hitSoundsTemp = graphicsAndSounds.Metal.sounds;
					tempBulletHole = graphicsAndSounds.Metal.Bullethole;
					PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
				} else if (tex.name.Contains ("Snow")) {
					hitSoundsTemp = graphicsAndSounds.Snow.sounds;
					tempBulletHole = graphicsAndSounds.Snow.Bullethole;
					PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
				} else if (tex.name.Contains ("Sand")) {
					hitSoundsTemp = graphicsAndSounds.Sand.sounds;
					tempBulletHole = graphicsAndSounds.Sand.Bullethole;
					PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
				} else if (tex.name.Contains ("Wood")) {
					hitSoundsTemp = graphicsAndSounds.Wood.sounds;
					tempBulletHole = graphicsAndSounds.Wood.Bullethole;
					PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
				}
			} else {
				hitSoundsTemp = graphicsAndSounds.Wood.sounds;
				tempBulletHole = graphicsAndSounds.Wood.Bullethole;
				PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
			}
		}
	}
	}
	IEnumerator ShotgunFire()
	{
		yield return new WaitForSeconds (weaponStats.FireDelay);
		if (canFire == true) {
			weaponStats.CurrentAmmo -= 1;
			RaycastHit hit;
			Vector3 direction = graphicsAndSounds.FirstPersonbarrel.transform.forward;
			for (var i = 0; i < weaponStats.ShotgunNumberOfBits; i++) {
				direction.x += UnityEngine.Random.Range (-weaponStats.spreadFactor, weaponStats.spreadFactor);
				direction.y += UnityEngine.Random.Range (-weaponStats.spreadFactor, weaponStats.spreadFactor);
				direction.z += UnityEngine.Random.Range (-weaponStats.spreadFactor, weaponStats.spreadFactor);
				if (Physics.Raycast (barrel.transform.position, direction, out hit, weaponStats.Range)) {
					if (hit.transform.gameObject.GetComponent<Health> ()) {
						hit.transform.gameObject.GetComponent<Health> ().health -= weaponStats.Damage;
					}
					if (hit.transform.gameObject.GetComponent<MeshRenderer> ()) {
						tex = hit.transform.gameObject.GetComponent<MeshRenderer> ().material.mainTexture;
					}
					if (hit.transform.tag == "Terrain") {
						tex = getTerrainTextureAt (transform.position);
					}
					if (tex != null) {
						if (tex.name.Contains ("Grass")) {
							hitSoundsTemp = graphicsAndSounds.Grass.sounds;
							tempBulletHole = graphicsAndSounds.Grass.Bullethole;
							PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
						} else if (tex.name.Contains ("Rock")) {
							hitSoundsTemp = graphicsAndSounds.Rock.sounds;
							tempBulletHole = graphicsAndSounds.Rock.Bullethole;
							PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
						} else if (tex.name.Contains ("Metal")) {
							hitSoundsTemp = graphicsAndSounds.Metal.sounds;
							tempBulletHole = graphicsAndSounds.Metal.Bullethole;
							PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
						} else if (tex.name.Contains ("Snow")) {
							hitSoundsTemp = graphicsAndSounds.Snow.sounds;
							tempBulletHole = graphicsAndSounds.Snow.Bullethole;
							PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
						} else if (tex.name.Contains ("Sand")) {
							hitSoundsTemp = graphicsAndSounds.Sand.sounds;
							tempBulletHole = graphicsAndSounds.Sand.Bullethole;
							PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
						} else if (tex.name.Contains ("Wood")) {
							hitSoundsTemp = graphicsAndSounds.Wood.sounds;
							tempBulletHole = graphicsAndSounds.Wood.Bullethole;
							PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
						}
					}else{
						hitSoundsTemp = graphicsAndSounds.Wood.sounds;
						tempBulletHole = graphicsAndSounds.Wood.Bullethole;
						PlayHitNoise (hit, hitSoundsTemp, tempBulletHole);
					}
				}
				
			}
		} 
	}
	void PlayHitNoise(RaycastHit hit,AudioClip[] hitNoise,GameObject[] bulletHoles)
	{
			GameObject audioSource = new GameObject("sound");
			audioSource.transform.position = hit.point;
			audioSource.AddComponent<AudioSource>();
			audioSource.GetComponent<AudioSource>().maxDistance = 5;
			audioSource.GetComponent<AudioSource>().PlayOneShot(hitSoundsTemp[UnityEngine.Random.Range(0,hitSoundsTemp.Length)]);
			Destroy(audioSource,graphicsAndSounds.destroyDelay);
			GameObject bulletHole = (GameObject)Instantiate (bulletHoles [UnityEngine.Random.Range (0, bulletHoles.Length)], hit.point, Quaternion.FromToRotation (Vector3.up, hit.normal));
			bulletHole.transform.parent = hit.transform;
			Destroy (bulletHole, graphicsAndSounds.destroyDelay);
	}
	IEnumerator Reload()
	{
		if (reloading == false){
			reloading = true;
			canFire = false;
			GetComponent<AudioSource> ().PlayOneShot (graphicsAndSounds.Reloads);
			if (graphicsAndSounds.hasReloadAnimation == true){
				graphicsAndSounds.ObjectWithAnimationsOnIt.GetComponent<Animation>().Play(graphicsAndSounds.FirstPersonReloadAnimation.name);
			}
		yield return new WaitForSeconds (weaponStats.reloadTime);
		for (int i = 0; i < inven.Items.Count; i++) {
			if (inven.Items [i].itemType == Item.ItemType.Ammo) {
				if (inven.Items [i].ammoType == inven.Items [slotNumber].ammoType) {
					int ammoToTake = weaponStats.ClipSize - weaponStats.CurrentAmmo;
					if (weaponStats.CurrentAmmo < weaponStats.ClipSize) {
						if (ammoToTake > inven.Items [i].itemValue) {
							ammoToTake = inven.Items [i].itemValue;
						}
						weaponStats.CurrentAmmo += ammoToTake;
						inven.Items [i].itemValue -= ammoToTake;
					}
				}
			}
		}
			reloading = false;
			canFire = true;
	}
	}
	IEnumerator BurstFire(float interval, int numberOfRounds){
		isBurstFiring = true;
		for (int i = 0; i < numberOfRounds; i++){
			Fire();
			yield return new WaitForSeconds(interval);
		}
		isBurstFiring = false;
	}
	public Texture getTerrainTextureAt( Vector3 position )
	{
		Texture retval    =    new Texture();
		Vector3 TS; // terrain size
		Vector2 AS; // control texture size
		
		TS = Terrain.activeTerrain.terrainData.size;
		AS.x = Terrain.activeTerrain.terrainData.alphamapWidth;
		AS.y = Terrain.activeTerrain.terrainData.alphamapHeight;
		
		// Lookup texture we are standing on:
		int AX = (int)( ( position.x/TS.x )*AS.x + 0.5f );
		int AY = (int)( ( position.z/TS.z )*AS.y + 0.5f );
		float[,,] TerrCntrl = Terrain.activeTerrain.terrainData.GetAlphamaps(AX, AY,1 ,1);
		
		TerrainData TD = Terrain.activeTerrain.terrainData;
		
		for( int i = 0; i < TD.splatPrototypes.Length; i++ )
		{
			if( TerrCntrl[0,0,i] > .5f )
			{
				retval    =    TD.splatPrototypes[i].texture;
			}
			
		}
		return retval;
	}
}
public enum FireMode
{
	Semi,
	Burst,
	FullAuto,
	Shotgun
};