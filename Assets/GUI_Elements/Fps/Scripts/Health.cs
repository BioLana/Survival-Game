using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public float health = 1f;
	public bool InstantiateSomething;
	public GameObject Object;
	public int destroyDelay = 5;
	public float pushForce;
	void Update()
	{
		if (health <= 0) {
			if (InstantiateSomething == false){
			Destroy(gameObject);
			}else{
				Destroy(gameObject);
				GameObject obj = (GameObject)Instantiate(Object,transform.position,transform.rotation);
				if(obj.GetComponent<Rigidbody>())
				{
					obj.GetComponent<Rigidbody>().AddForce(transform.forward * pushForce);
				}
				Destroy(obj,destroyDelay);
			}
		}
	}
}
