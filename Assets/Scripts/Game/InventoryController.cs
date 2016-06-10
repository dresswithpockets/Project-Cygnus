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
	
	public Weapon LeftHand { get; private set; }
	public Weapon RightHand { get; private set; }

	public Armor[] EquippedArmor { get; private set; }

	private List<Item> EquippableItems = new List<Item>();
	private List<Item> MiscItems = new List<Item>();

	private List<Ability> LearnedAbilities = new List<Ability>();

	public Ability[] Abilities {
		get {
			return new Ability[] { ActiveAbility1, ActiveAbility2, ActiveAbility3, ActiveAbility4, PassiveAbility1, PassiveAbility2 };
		}
	}

	public Ability[] ActiveAbilities {
		get {
			return new Ability[] { ActiveAbility1, ActiveAbility2, ActiveAbility3, ActiveAbility4 };
		}
	}

	public Ability[] PassiveAbilities {
		get {
			return new Ability[] { PassiveAbility1, PassiveAbility2 };
		}
	}

	public Ability ActiveAbility1 { get; private set; }
	public Ability ActiveAbility2 { get; private set; }
	public Ability ActiveAbility3 { get; private set; }
	public Ability ActiveAbility4 { get; private set; }

	public Ability PassiveAbility1 { get; private set; }
	public Ability PassiveAbility2 { get; private set; }

	public Pawn Owner { get; private set; }

	public bool IsUsingWeapon {
		get {
			return RightHand.Using || LeftHand.Using;
		}
	}

	public Weapon this[WeaponSlot slot] {
		get {
			return GetEquippedItem(slot);
		}
		set {
			Equip(value, slot);
		}
	}

	public Weapon GetEquippedItem(WeaponSlot slot) {
		switch (slot) {
			case WeaponSlot.LEFT_HAND:
				return LeftHand;
			case WeaponSlot.RIGHT_HAND:
				return RightHand;
		}
		return null;
	}

	public void Pickup(Item item) {
		if (item.IsEquippable) EquippableItems.Add(item);
		else MiscItems.Add(item);

		item.Owner = Owner;
		item.transform.SetParent(transform);
		item.ToggleItem(false);

		GameController.InvokePickedUpItem(this, Owner, item);
	}

	public void Pickup(Ability ability) {
		if (LearnedAbilities.Contains(ability)) return;

		LearnedAbilities.Add(ability);
		GameController.InvokeLearnedAbility(this, Owner, ability);
	}

	public void Drop(Item item) {
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

	public void Equip(Weapon weapon, WeaponSlot slot) {
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
		EquippedArmor = new Armor[Enum.GetNames(typeof(ArmorSlot)).Length];
	}

	void Update() {
		
	}
}
