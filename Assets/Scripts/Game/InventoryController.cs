using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Standard Inventory component for a Pawn with:
/// 2 Weapon Slots
/// 4 Ability Slots
/// 2 Skill Slots
/// 7 Armor Slots
/// 1 Lantern Slot
/// 1 Boat Slot
/// 1 Pet Slot
/// 
/// A page for each category of items.
/// </summary>
[RequireComponent(typeof(Pawn))]
public class InventoryController : MonoBehaviour {
	
	private AWeapon LeftHand = null;
	private AWeapon RightHand = null;

	private AArmor[] EquippedArmor;

	private List<AItem> EquippableItems = new List<AItem>();
	private List<AItem> MiscItems = new List<AItem>();

	private Pawn Owner;

	public bool IsUsingWeapon {
		get {
			return RightHand.Using || LeftHand.Using;
		}
	}

	public AWeapon this[WeaponSlot slot] {
		get {
			return GetEquippedItem(slot);
		}
		set {
			Equip(value, slot);
		}
	}

	public AWeapon GetEquippedItem(WeaponSlot slot) {
		switch (slot) {
			case WeaponSlot.LEFT_HAND:
				return LeftHand;
			case WeaponSlot.RIGHT_HAND:
				return RightHand;
		}
		return null;
	}

	public void Pickup(AItem item) {
		if (item.IsEquippable) EquippableItems.Add(item);
		else MiscItems.Add(item);

		item.Owner = Owner;
		item.transform.SetParent(transform);
		item.ToggleItem(false);

		GameController.InvokePickedUpItem(this, Owner, item);
	}

	public void Pickup(AAbility ability) {
		// TODO: Add ability storing nad management
	}

	public void Drop(AItem item) {
		if (LeftHand == item) Unequip(WeaponSlot.LEFT_HAND);
		else if (RightHand == item) Unequip(WeaponSlot.RIGHT_HAND);
		else if (EquippedArmor.Contains(item)) EquippedArmor.Remove(item);
		else if (EquippableItems.Contains(item)) EquippableItems.Remove(item);
		else if (MiscItems.Contains(item)) MiscItems.Remove(item);
		else return;

		item.transform.SetParent(null);
		item.ToggleItem(true);
		GameController.InvokeDroppedItem(this, Owner, item);
	}

	public void Equip(AWeapon weapon, WeaponSlot slot) {
		if (!EquippableItems.Contains(weapon)) Pickup(weapon);

		EquippableItems.Remove(weapon);
		
		weapon.Active = true;
		weapon.ToggleItem(true);

		weapon.transform.localPosition = new Vector3(0f, 1f, 0f);
		weapon.transform.localRotation = Quaternion.identity;

		switch (slot) {
			case WeaponSlot.LEFT_HAND:

				Unequip(WeaponSlot.LEFT_HAND);
				if (weapon.Handiness == WeaponHandiness.TWO_HANDED) {
					Unequip(WeaponSlot.RIGHT_HAND);
					RightHand = weapon;
				}
				else LeftHand = weapon;

				break;
			case WeaponSlot.RIGHT_HAND:

				if (weapon.Handiness == WeaponHandiness.TWO_HANDED) Unequip(WeaponSlot.LEFT_HAND);
				Unequip(WeaponSlot.RIGHT_HAND);
				RightHand = weapon;

				break;
		}

		GameController.InvokeEquippedWeapon(this, Owner, weapon);
	}
	
	public void Unequip(WeaponSlot slot) {

		switch (slot) {
			case WeaponSlot.LEFT_HAND:
				if (LeftHand != null) {
					EquippableItems.Add(LeftHand);
					LeftHand.Active = false;
					LeftHand.ToggleItem(false);
					GameController.InvokeUnequippedWeapon(this, Owner, LeftHand);
				}
				LeftHand = null;
				break;
			case WeaponSlot.RIGHT_HAND:
				if (RightHand != null) {
					EquippableItems.Add(RightHand);
					RightHand.Active = false;
					RightHand.ToggleItem(false);
					GameController.InvokeUnequippedWeapon(this, Owner, RightHand);
				}
				RightHand = null;
				break;
		}
	}

	void Start() {
		Owner = GetComponent<Pawn>();
		EquippedArmor = new AArmor[Enum.GetNames(typeof(ArmorSlot)).Length];
	}
}
