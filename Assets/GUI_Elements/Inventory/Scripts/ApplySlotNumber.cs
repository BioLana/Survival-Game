using UnityEngine;
using System.Collections;
using System.Linq;
public class ApplySlotNumber : MonoBehaviour {
	public int NumberOfSlots;
	Inventory inventory;
	void Start () {
		inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory>();
		for(int i = 0; i<NumberOfSlots;i++)
		{
			inventory.Slots.Add(transform.GetChild(i).gameObject);
			if(transform.GetChild(i).GetComponent<HotbarScript>())
			{
			transform.GetChild(i).GetComponent<HotbarScript>().slotNumber = inventory.Slots.Count -1;
			}
			inventory.Items.Add(new Item());
		}
	}
}
