using System;
using UnityEngine;


   
	public class HeadFollow : MonoBehaviour
	{
	public Transform target;
	public float speed;
	void Update() {
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.position, step);
	}
    }

