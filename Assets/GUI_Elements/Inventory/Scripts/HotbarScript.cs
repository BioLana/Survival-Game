using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HotbarScript : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler ,IDragHandler{
	public KeyCode ActivateKey;
	public Item item;
	Image itemImage;
	public int slotNumber;
	Inventory inventory;
	Text itemAmount;
	WeaponHandler weaponhandler;
	public bool EquippedUneuiped;
	BuildingManager buildingManager;
	public bool BuildNoBuild;
	PlayerStats stats;
	// Use this for initialization
	void Start () {
		weaponhandler = GameObject.FindGameObjectWithTag ("WeaponHandler").GetComponent<WeaponHandler> ();
		itemAmount = gameObject.transform.GetChild (1).GetComponent<Text> ();
		inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory>();
		itemImage = gameObject.transform.GetChild (0).GetComponent<Image> ();
		buildingManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<BuildingManager>();
		stats = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (inventory.Items[slotNumber].itemName != null) {
			itemAmount.enabled = false;
			itemImage.enabled = true;
			itemImage.sprite = Resources.Load<Sprite>(inventory.Items[slotNumber].itemName);
			
			if (inventory.Items[slotNumber].Stackable == true){
				itemAmount.enabled = true;
				itemAmount.text = ""+ inventory.Items[slotNumber].itemValue;
				if (inventory.Items[slotNumber].itemValue == 0)
				{
					inventory.Items[slotNumber] = new Item();
				}
			}
			if (inventory.Items[slotNumber].itemType == Item.ItemType.Weapon){
				itemAmount.enabled = true;
				itemAmount.text = inventory.Items[slotNumber].currentAmmo + "/"+ inventory.Items[slotNumber].clipSize;
			}
		} else {
			itemImage.enabled = false;
			itemAmount.enabled = false;
		}
		if (Input.GetKeyDown (ActivateKey)) {
			if (inventory.Items [slotNumber].itemType == Item.ItemType.Weapon) {
				if (inventory.Items [slotNumber].itemName != null) {
					EquippedUneuiped = !EquippedUneuiped;
					if (EquippedUneuiped == true ) {
						weaponhandler.GunWeWantToEquipped = inventory.Items[slotNumber].itemName;
						weaponhandler.EquipGun();
						weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().slotNumber = slotNumber;
						weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().weaponStats.ClipSize = inventory.Items[slotNumber].clipSize;
						weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().weaponStats.CurrentAmmo = inventory.Items[slotNumber].currentAmmo;
					} else if (EquippedUneuiped == false) {
						weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().canFire = true;
						weaponhandler.GunWeWantToEquipped = inventory.Items[slotNumber].itemName;
						weaponhandler.UnequipGun();
						weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().slotNumber = -1;
					}
				}
			}
			if (inventory.Items [slotNumber].itemType == Item.ItemType.MeleeWeapon) {
				if (inventory.Items [slotNumber].itemName != null) {
					EquippedUneuiped = !EquippedUneuiped;
					if (EquippedUneuiped == true ) {
						weaponhandler.GunWeWantToEquipped = inventory.Items[slotNumber].itemName;
						weaponhandler.EquipGun();
						weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().slotNumber = slotNumber;
					} else if (EquippedUneuiped == false) {
						weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().canFire = true;
						weaponhandler.GunWeWantToEquipped = inventory.Items[slotNumber].itemName;
						weaponhandler.UnequipGun();
						weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().slotNumber = -1;
					}
				}
			}
			if (inventory.Items [slotNumber].itemType == Item.ItemType.Food) {
				if (inventory.Items [slotNumber].itemName != null) {
					stats.Hunger += inventory.Items[slotNumber].FeedAmount;
					stats.Thirst += inventory.Items[slotNumber].DrinkAmount;
					inventory.Items[slotNumber].itemValue -= 1;
				}
			}
			if (inventory.Items [slotNumber].itemType == Item.ItemType.Drink) {
				if (inventory.Items [slotNumber].itemName != null) {
					stats.Hunger += inventory.Items[slotNumber].FeedAmount;
					stats.Thirst += inventory.Items[slotNumber].DrinkAmount;
					inventory.Items[slotNumber].itemValue -= 1;
				}
			}
			if (inventory.Items [slotNumber].itemType == Item.ItemType.BuildableObject) {
				if (inventory.Items [slotNumber].itemName != null) {
					BuildNoBuild = !BuildNoBuild;
					if(BuildNoBuild == true){
						buildingManager.BuildingWeWantToEquipped = inventory.Items[slotNumber].itemName;
						buildingManager.BuildingOn();
						buildingManager.SlotNumber = slotNumber;
					}else if(BuildNoBuild == false){
						buildingManager.BuildingOff();
						buildingManager.SlotNumber = -1;
					}
				}
			}
		}
	}
	public void	OnPointerDown(PointerEventData data)
	{
		if (inventory.Items [slotNumber].itemType == Item.ItemType.Weapon) {
			if (inventory.Items [slotNumber].itemName != null) {
				EquippedUneuiped = !EquippedUneuiped;
				if (EquippedUneuiped == true ) {
					weaponhandler.GunWeWantToEquipped = inventory.Items[slotNumber].itemName;
					weaponhandler.EquipGun();
					weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().slotNumber = slotNumber;
					weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().weaponStats.ClipSize = inventory.Items[slotNumber].clipSize;
					weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().weaponStats.CurrentAmmo = inventory.Items[slotNumber].currentAmmo;
				} else if (EquippedUneuiped == false) {
					weaponhandler.GunWeWantToEquipped = inventory.Items[slotNumber].itemName;
					weaponhandler.UnequipGun();
					weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().slotNumber = -1;
				}
			}
		}
		if (inventory.Items [slotNumber].itemName == null && inventory.DraggingItem) {
			inventory.Items[slotNumber] = inventory.DraggedItem;
			inventory.HideDraggedItem();
		}else if(inventory.DraggingItem && inventory.Items[slotNumber].itemName != null)
		{
			inventory.Items[inventory.indexOfDraggeditem] = inventory.Items[slotNumber];
			inventory.Items[slotNumber] = inventory.DraggedItem;
			inventory.HideDraggedItem();
		}
	}
	public void OnPointerEnter(PointerEventData data)
	{
		if (inventory.Items [slotNumber].itemName != null) {
			inventory.ShowToolTip(inventory.Items[slotNumber]);
		}
	}
	public void OnPointerExit(PointerEventData data)
	{
		if (inventory.Items [slotNumber].itemName != null) {
			inventory.HideToolTip ();
		}
	}
	public void OnDrag(PointerEventData data)
	{
		if (inventory.Items [slotNumber].itemName != null) {
			inventory.ShowDraggedItem(inventory.Items[slotNumber],slotNumber);
			inventory.Items[slotNumber] = new Item();
			itemAmount.enabled = false;
		}
	}
}
