using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour {

	public Transform[] SpawnPoints;
	public float maxHealth = 1f;
	public float health = 1f;
	public bool InstantiateSomething;
	public GameObject Object;
	public int destroyDelay = 5;
	public float pushForce;

	Inventory inventory;


	public Image HungerImg;
	public Image ThirstImg;
	public Image HealthImg;

	public float Hunger;
	public float MaxHunger = 1.00f;

	public float Thirst;
	public float MaxThirst = 1.00f;

	public float LowerHungerSpeed = 0.005f;
	public float LowerThirstSpeed = 0.005f;
	public float LowerHealthSpeed = 0.005f;
	public float HealthRegenSpeed = 0.005f;
	public float RunningModifier;
	float RunningDrainDecreaseHunger;
	float RunningDrainDecreaseThirst;
	float defaultLHE;
	float defaultLTE;

	void Start()
	{
		if (GameObject.FindGameObjectWithTag ("Inventory")) {
			inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory> ();
		} else {
			Debug.Log("Hmm does not look you have the canvas object in your scene. can you add that for me?");
		}
		RunningDrainDecreaseHunger = LowerHungerSpeed * RunningModifier;
		RunningDrainDecreaseThirst = LowerThirstSpeed * RunningModifier;
		defaultLHE = LowerHungerSpeed;
		defaultLTE = LowerThirstSpeed;
		Hunger = MaxHunger;
		Thirst = MaxThirst;
		health = maxHealth;
	}
	void Update()
	{

		Thirst -= Time.deltaTime * LowerThirstSpeed;
		Hunger -= Time.deltaTime * LowerHungerSpeed;

		HealthImg.fillAmount = health;
		HungerImg.fillAmount = Hunger;
		ThirstImg.fillAmount = Thirst;

		if (GetComponent<PlayerMovementInput> ().enabled == true) {
			LowerHungerSpeed = RunningDrainDecreaseHunger;
			LowerThirstSpeed = RunningDrainDecreaseThirst;
		} else {
			LowerHungerSpeed = defaultLHE;
			LowerThirstSpeed = defaultLTE;
		}

		if (Hunger <= 0 || Thirst <=0) {
			health -= Time.deltaTime * LowerHealthSpeed;
		}
		if (Hunger >= 0.9f && Thirst >=0.9f) {
			health += Time.deltaTime * HealthRegenSpeed;
		}
		if (health <= 0) {
			Die();
		}

		if (health > maxHealth) {
			health = maxHealth;
		}
		if (Hunger > MaxHunger) {
			Hunger = MaxHunger;
		}
		if (Thirst > MaxThirst) {
			Thirst = MaxThirst;
		}
	}
	public void Die()
	{
		if (InstantiateSomething == false){
			inventory.DropAll();
			transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
			Hunger = MaxHunger;
			Thirst = MaxThirst;
			health = maxHealth;
		}else{
			inventory.DropAll();
			GameObject obj = (GameObject)Instantiate(Object,transform.position,transform.rotation);
			if(obj.GetComponent<Rigidbody>())
			{
				obj.GetComponent<Rigidbody>().AddForce(transform.forward * pushForce);
			}
			Destroy(obj,destroyDelay);
			transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
			Hunger = MaxHunger;
			Thirst = MaxThirst;
			health = maxHealth;

		}
	}
}
