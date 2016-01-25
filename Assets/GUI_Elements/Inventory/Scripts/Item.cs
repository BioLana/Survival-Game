using UnityEngine;
using System.Collections;
[System.Serializable]
public class Item {

	public string itemName;
	public string itemDesc;
	[System.NonSerialized]public Sprite itemIcon;
	[System.NonSerialized]public GameObject itemModel;
	public ItemType itemType;
	public int itemValue;
	public int clipSize;
	public int currentAmmo;
	public AmmoType ammoType;
	public float FeedAmount;
	public float DrinkAmount;
	public bool Stackable;

	public enum ItemType
	{
		Weapon,
		MeleeWeapon,
		Ammo,
		craftingMat,
		BuildableObject,
		Food,
		Drink
	}
	public enum AmmoType
	{
		None,
		Rifle,
		Pistol
	}
	public Item(string name,string desc, ItemType type,int value,AmmoType ammo,int clips,int currenta,bool stack,float food,float drink)
	{
		itemName = name;
		itemDesc = desc;
		itemType = type;
		itemIcon = Resources.Load<Sprite>(name);
		itemValue = value;
		ammoType = ammo;
		clipSize = clips;
		currentAmmo = currenta;
		itemModel = Resources.Load<GameObject>(name);
		Stackable = stack;
		DrinkAmount = drink;
		FeedAmount = food;
	}

	public Item()
	{

	}
}
