using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropBox : MonoBehaviour, IPointerDownHandler{
	Inventory inventory;
	WeaponHandler weaponhandler;
	// Use this for initialization
	void Start () {
		inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory> ();
		weaponhandler = GameObject.FindGameObjectWithTag ("WeaponHandler").GetComponent<WeaponHandler> ();;
	}

	public void	OnPointerDown(PointerEventData data){
		if (inventory.DraggingItem) {
			dropItem(inventory.DraggedItem);
			inventory.HideDraggedItem();
		}
	}
	void dropItem(Item item){
		weaponhandler.UnequipGun ();
		GameObject itemAsGameobject = (GameObject)Instantiate (Resources.Load<GameObject>(item.itemName), GameObject.FindGameObjectWithTag ("Player").transform.position, Quaternion.identity);
		itemAsGameobject.GetComponent<DroppedItem> ().item = item;
	}
}
