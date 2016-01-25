using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Craft : MonoBehaviour ,IPointerDownHandler{
	public string itemThatItMakes;
	Inventory inventory;
	int amountOfItemsNeeded;
	int currentAmountOfItems;
	public ItemsNeeded[] itemsNeeded = new ItemsNeeded[1];
	void Start()
	{
		inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
		amountOfItemsNeeded = itemsNeeded.Length;
	}
	public void	OnPointerDown(PointerEventData data){
		currentAmountOfItems = 0;
		for (int amountOfItems = 0; amountOfItems < itemsNeeded.Length; amountOfItems++) {
			for (int i = 0; i < inventory.Items.Count; i++) {
				addItem(amountOfItems,i);
			}
		}
	}
	void addItem(int amountOfItems,int i)
	{
		if (inventory.Items [i].itemType == Item.ItemType.craftingMat) {
			if (inventory.Items[i].itemName == itemsNeeded[amountOfItems].Name)
			{
				if (inventory.Items[i].itemValue >= itemsNeeded[amountOfItems].Amount)
				{
					itemsNeeded[amountOfItems].hasItems = true;
					if (itemsNeeded[amountOfItems].hasItems == true)
					{
						currentAmountOfItems ++;
						if (currentAmountOfItems == amountOfItemsNeeded) {
							RemoveItems();
							GameObject itemAsGameobject = (GameObject)Instantiate (Resources.Load<GameObject>(itemThatItMakes), GameObject.FindGameObjectWithTag ("Player").transform.position, Quaternion.identity);
							inventory.addExistingItem(itemAsGameobject.GetComponent<DroppedItem>().item);
							Destroy(itemAsGameobject);
						}
					}
				}
			}
		}
	}
	void RemoveItems()
	{
		for (int amountOfItems = 0; amountOfItems < itemsNeeded.Length; amountOfItems++) {
			for (int i = 0; i < inventory.Items.Count; i++) {
				if (inventory.Items [i].itemType == Item.ItemType.craftingMat) {
					if (inventory.Items[i].itemName == itemsNeeded[amountOfItems].Name)
					{
						inventory.Items[i].itemValue -= itemsNeeded[amountOfItems].Amount;
					}
				}
			}
		}
	}
}
[System.Serializable]
public class ItemsNeeded
{
	public string Name;
	public int Amount;
	[HideInInspector]
	public bool hasItems = false;
}