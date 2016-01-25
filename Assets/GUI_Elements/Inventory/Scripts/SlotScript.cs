using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler ,IDragHandler{

	public Item item;
	Image itemImage;
	public int slotNumber;
	Inventory inventory;
	Text itemAmount;
	WeaponHandler weaponhandler;
	public bool EquippedUneuiped;
	ShowInventory showinv;
	// Use this for initialization
	void Start () {
		weaponhandler = GameObject.FindGameObjectWithTag ("WeaponHandler").GetComponent<WeaponHandler> ();
		itemAmount = gameObject.transform.GetChild (1).GetComponent<Text> ();
		inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory>();
		itemImage = gameObject.transform.GetChild (0).GetComponent<Image> ();
		showinv = GameObject.FindGameObjectWithTag ("Player").GetComponent<ShowInventory> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (inventory.Items[slotNumber].itemName != null && showinv.showInv ==true) {
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
					weaponhandler.Weapons[weaponhandler.whatGun].WeaponModel.GetComponent<Weapon>().canFire = true;
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
			weaponhandler.UnequipGun();
		}
	}
}
