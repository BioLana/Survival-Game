using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class Inventory : MonoBehaviour {

	public List<GameObject> Slots = new List<GameObject> ();
	public List<Item> Items = new List<Item>();
	ItemDatabase dataBase;
	public GameObject slots;
	int tempx;
	int tempy;
	public int x;
	public int y;
	public int spacing = 55;
	public int Rows;
	public int Collums;

	public Text textInvText;
	public GameObject ToolTip;
	public GameObject draggedItemGameObject;
	public bool DraggingItem = false;
	public Item DraggedItem;
	public int indexOfDraggeditem;
	WeaponHandler weaponhandler;
	void Update()
	{
		if (DraggingItem) {
			Vector3 posi = (Input.mousePosition);
			draggedItemGameObject.GetComponent<RectTransform>().position = new Vector3(posi.x +20 , posi.y - 20, posi.z);
		}
	}
	public void ShowToolTip(Item item)
	{
		ToolTip.SetActive (true);
		ToolTip.transform.GetChild (0).GetComponent<Text> ().text = item.itemName;
		ToolTip.transform.GetChild (1).GetComponent<Text> ().text = item.itemDesc;
	}
	public void HideToolTip()
	{
		ToolTip.SetActive (false);
	}
	public void ShowDraggedItem(Item item, int slotNumber){
		indexOfDraggeditem = slotNumber;
		draggedItemGameObject.SetActive (true);
		DraggedItem = item;
		DraggingItem = true;
		draggedItemGameObject.GetComponent<Image> ().sprite = Resources.Load<Sprite>(item.itemName);
	}

	public void InvTextShow()
	{
	
	}
	public void HideDraggedItem()
	{
		DraggingItem = false;
		draggedItemGameObject.SetActive (false);

	}
	void Start () {
		weaponhandler = GameObject.FindGameObjectWithTag ("WeaponHandler").GetComponent<WeaponHandler> ();
		int tempx = x;
		int tempy = y;
		int slotAmount = 0;
		if (GameObject.FindGameObjectWithTag ("ItemDatabase")) {
			dataBase = GameObject.FindGameObjectWithTag ("ItemDatabase").GetComponent<ItemDatabase> ();
		} else {
			Debug.Log("Hmm does not look you have the ItemDatabse object on your player. can you add that for me?");
		}


		for (int i = 0; i <Collums; i++) {
			for (int k = 0; k<Rows; k++){
				GameObject slot = (GameObject)Instantiate(slots);
				slot.GetComponent<SlotScript>().slotNumber = slotAmount;
				Items.Add(new Item());
				slotAmount++;
				Slots.Add(slot);
				slot.transform.SetParent (this.gameObject.transform);
				slot.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				slot.name = "slot" +i +"," +k;
				slot.GetComponent<RectTransform>().localPosition = new Vector3(tempx,tempy,0);
				tempx = tempx + spacing;
				if (k == Rows -1){
					tempx = x;
					tempy = tempy -spacing;
				}
			}
		}


	}
	public void checkIfItemExists(string itemId,Item item)
	{
		for (int i = 0; i < Items.Count; i++) {
			if(Items[i].itemName == itemId)
			{
				Items[i].itemValue = Items[i].itemValue +item.itemValue;
				break;
			}else if (i == Items.Count -1){
				addItemAtEmptySlot(item);
			}
		}
	}
	public void AddItem(string id)
	{
		for (int i = 0; i <  dataBase.items.Count; i++) {
			if(dataBase.items[i].itemName == id)
			{
				Item item = dataBase.items[i];
				if(dataBase.items[i].Stackable == true){
					checkIfItemExists(id, item);
					break;
				}else{
					addItemAtEmptySlot(item);
				}
			}
		}
	}
	public void addExistingItem(Item item){
		if (item.Stackable == true) {
			checkIfItemExists(item.itemName,item);
		}else{
			addItemAtEmptySlot(item);
		}
	}
	void addItemAtEmptySlot(Item item){
		for (int i = 0; i < Items.Count; i++) {
			if (Items[i].itemName == null)
			{
				Items[i] = item;
				break;
			}
		}
	}
	public void SaveInv()
	{
		FileStream fs = new FileStream("Inventory.dat", FileMode.Create);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(fs, Items);
		fs.Close();
	}
	public void LoadInv()
	{
		using (Stream stream = File.Open("Inventory.dat", FileMode.Open))
		{
			var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			List<Item>  items = (List<Item>)bformatter.Deserialize(stream);
			Items = items;
		}
	}
	public void DropAll()
	{
		weaponhandler.UnequipGun ();
		for(int i =0; i< Items.Count; i++)
		{
			if (Items[i].itemName != null)
			{
			Vector3 pos = GameObject.FindGameObjectWithTag ("Player").transform.position;
			pos.y+=1;
			GameObject itemAsGameobject = (GameObject)Instantiate (Resources.Load<GameObject>(Items[i].itemName), pos, Quaternion.identity);
			itemAsGameobject.GetComponent<DroppedItem> ().item = Items[i];
			Items[i] = new Item();
			}
		}
	}
}
