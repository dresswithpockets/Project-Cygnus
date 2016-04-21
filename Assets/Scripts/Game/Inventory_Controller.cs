using UnityEngine;
using System.Collections.Generic;
using System;

public sealed class Inventory_Controller : MonoBehaviour {
	
	public bool is_casting_ability = false;

	private List<Material> m_material_list = new List<Material>();
	private List<Armor> m_armor_list = new List<Armor>();
	private List<Weapon> m_weapon_list = new List<Weapon>();
	private List<Consumable> m_consumable_list = new List<Consumable>();
	private List<Pet_Item> m_pet_item_list = new List<Pet_Item>();

	// index 0 = right hand weapons. One handed and two handed weapons go here.
	// index 1 = left hand weapons. One handed weapons go here.
	private Weapon[] m_active_weapon_list = new[] { (Weapon)null, null }; // 2 slots, 0 and 1

	/*
	HEAD = slot 0,
	SHOULDERS = slot 1,
	TORSO = slot 2,
	HANDS = slot 3,
	LEGS = slot 4,
	FEET = slot 5,
	NECK = slot 6
	*/
	private Armor[] m_active_armor_list = new[] { (Armor)null, null, null, null, null, null, null }; // 7 slots, 0 through 6

	private Dictionary<string, Ability_Template> m_learned_ability_list = new Dictionary<string, Ability_Template>();
	private Ability_Template[] m_active_ability_list = new[] { (Ability_Template)null, null, null, null }; // 4 slots, 0 through 3

	private Player_Controller m_player = null;
	private bool m_player_inv = false;

	#region Properties

	public Player_Controller player {

		get {

			return m_player;
		}
		internal set {

			m_player = value;
		}
	}

	public bool is_player_inventory {

		get {

			return m_player_inv;
		}
		internal set {

			m_player_inv = value;
		}
	}

	public List<Material> material_list {

		get {

			return m_material_list;
		}
		internal set {

			m_material_list = value;
		}
	}

	public List<Armor> armor_list {

		get {

			return m_armor_list;
		}
		internal set {

			m_armor_list = value;
		}
	}

	public List<Weapon> weapon_list {

		get {

			return m_weapon_list;
		}
		internal set {

			m_weapon_list = value;
		}
	}

	public List<Consumable> consumable_list {

		get {

			return m_consumable_list;
		}
		internal set {

			m_consumable_list = value;
		}
	}

	public List<Pet_Item> pet_item_list {

		get {

			return m_pet_item_list;
		}
		internal set {

			m_pet_item_list = value;
		}
	}

	public Weapon[] active_weapon_list {

		get {

			return m_active_weapon_list;
		}
		internal set {

			m_active_weapon_list = value;
		}
	}

	public Armor[] active_armor_list {

		get {

			return m_active_armor_list;
		}
		internal set {

			m_active_armor_list = value;
		}
	}

	public Dictionary<string, Ability_Template> learned_ability_list {

		get {

			return m_learned_ability_list;
		}
		internal set {

			m_learned_ability_list = value;
		}
	}

	public Ability_Template[] active_ability_list {

		get {

			return m_active_ability_list;
		}
		internal set {

			m_active_ability_list = value;
		}
	}

	#endregion

	public void Update() {
		foreach (Weapon weapon in active_weapon_list) {
			// active_weapon_list is filled with null values, duh
			if (weapon != null) weapon.active_update();
		}
		foreach (Armor armor in active_armor_list) {
			if (armor != null) armor.active_update();
		}
		// Note: abilities do not have active updates for when they are "equipped".
	}

	public void pickup(Material material) {

		material_list.Add(material);

		material.set_render(false);

		if (is_player_inventory) {
			Game_Controller.invoke_player_picked_up(material.template);

			material.set_owner(player);
			material.pick_up();
		}

		Debug.Log("Picked up material with name: " + material.name);


		// Note: inventories probably won't exist for NPCs, at least not yet.
		// so we dont really have functionality to call the API for npc ownership.
	}

	public void pickup(Armor armor) {

		armor_list.Add(armor);

		armor.set_render(false);

		if (is_player_inventory) {
			Game_Controller.invoke_player_picked_up(armor.template);

			armor.set_owner(player);
			armor.pick_up();
		}

		Debug.Log("Picked up armor with name: " + armor.template.name);

		// Note: inventories probably won't exist for NPCs, at least not yet.
		// so we dont really have functionality to call the API for npc ownership.
	}

	public void pickup(Weapon weapon) {

		weapon_list.Add(weapon);

		weapon.set_render(false);

		if (is_player_inventory) {
			Game_Controller.invoke_player_picked_up(weapon.template);

			weapon.set_owner(player);
			weapon.pick_up();
		}

		Debug.Log("Picked up weapon with name: " + weapon.template.name);

		// Note: inventories probably won't exist for NPCs, at least not yet.
		// so we dont really have functionality to call the API for npc ownership.
	}

	public void pickup(Consumable consumable) {

		consumable_list.Add(consumable);
		
		consumable.set_render(false);

		if (is_player_inventory) {
			Game_Controller.invoke_player_picked_up(consumable.template);

			consumable.set_owner(player);
			consumable.pick_up();
		}

		Debug.Log("Picked up consumable with name: " + consumable.template.name);

		// Note: inventories probably won't exist for NPCs, at least not yet.
		// so we dont really have functionality to call the API for npc ownership.
	}

	public void pickup(Pet_Item pet_item) {

		pet_item_list.Add(pet_item);

		pet_item.set_render(false);

		if (is_player_inventory) {
			Game_Controller.invoke_player_picked_up(pet_item.template);

			pet_item.set_owner(player);
			pet_item.pick_up();
		}

		Debug.Log("Picked up pet item with name: " + pet_item.template.name);

		// Note: inventories probably won't exist for NPCs, at least not yet.
		// so we dont really have functionality to call the API for npc ownership.
	}

	public void drop(Inventory_Category tab, int slot) {

		Item dropped_item = null;

		switch (tab) {

			case Inventory_Category.ARMOR:

				dropped_item = armor_list[slot];
				armor_list.RemoveAt(slot);

				break;
			case Inventory_Category.MATERIAL:

				dropped_item = material_list[slot];
				material_list.RemoveAt(slot);

				break;
			case Inventory_Category.WEAPON:

				dropped_item = weapon_list[slot];
				weapon_list.RemoveAt(slot);

				break;
			case Inventory_Category.CONSUMABLE:

				dropped_item = consumable_list[slot];
				consumable_list.RemoveAt(slot);
				
				break;
			case Inventory_Category.PET_ITEM:

				dropped_item = pet_item_list[slot];
				pet_item_list.RemoveAt(slot);

				break;
		}

		if (is_player_inventory) {
			
			Game_Controller.invoke_player_dropped(dropped_item.template);
			dropped_item.drop();
		}

		dropped_item.set_render(true);

		Debug.Log("Dropped item of type: " + Enum.GetName(typeof(Inventory_Category), tab) + " with name: " + dropped_item.name);
	}

	public void equip_weapon(int inv_slot, Weapon_Slot equip_slot) {

		if (weapon_list.Count < inv_slot + 1) {

			Debug.LogError("Can't equip weapon at inventory slot: " + inv_slot.ToString() + " because there is no weapon at that slot.");
			return;
		}

		Weapon weapon_to_equip = weapon_list[inv_slot];
		weapon_list[inv_slot] = null;

		Weapon left_wep = active_weapon_list[(int)Weapon_Slot.LEFT_HAND];
		Weapon right_wep = active_weapon_list[(int)Weapon_Slot.RIGHT_HAND];
		bool moved_inv_slot = false;

		// if there are no weapons equipped, equip this weapon to the right hand
		// -> players must always have either no weapons equipped a right handed weapon equipped.
		//		
		//		- TH 4/13/2016
		//
		if (left_wep == null && right_wep == null) equip_slot = Weapon_Slot.RIGHT_HAND;
		// Two handed weapons cannot be equipped with other
		// on handed or two handed weapons. Thus, we must move
		// any weapons that are equipped into the inventory.
		// moved_inv_slot is to flag and check whether or not we've already moved
		// a weapon into weapon_list[inv_slot]. If we have done so, we don't want to
		// overwrite it, so we use weapon_lost.Add() instead.
		//
		//		- TH 4/13/2016
		//
		else if (weapon_to_equip.weapon_template.handiness == Weapon_Handiness.TWO_HANDED) {
			if (left_wep != null) {
				weapon_list[inv_slot] = left_wep;
				weapon_list[inv_slot].set_render(false);

				moved_inv_slot = true;

				active_weapon_list[(int)Weapon_Slot.LEFT_HAND] = null;
			}
			if (right_wep != null) {
				if (moved_inv_slot) {
					weapon_list.Add(right_wep);
					weapon_list[weapon_list.Count - 1].set_render(false);
				}
				else {
					weapon_list[inv_slot] = right_wep;
					weapon_list[inv_slot].set_render(false);

					moved_inv_slot = true;
				}

				active_weapon_list[(int)Weapon_Slot.RIGHT_HAND] = null;
			}

			equip_slot = Weapon_Slot.RIGHT_HAND;
		}
		else { // One handed weapon to equip
			if (right_wep != null && right_wep.weapon_template.handiness == Weapon_Handiness.TWO_HANDED) {
				if (moved_inv_slot) {
					weapon_list.Add(right_wep);
					weapon_list[weapon_list.Count - 1].set_render(false);
				}
				else {
					weapon_list[inv_slot] = right_wep;
					weapon_list[inv_slot].set_render(false);

					moved_inv_slot = true;
				}

				active_weapon_list[(int)Weapon_Slot.RIGHT_HAND] = null;

				equip_slot = Weapon_Slot.RIGHT_HAND;
			}
			else if (equip_slot == Weapon_Slot.LEFT_HAND && left_wep != null) {
				if (moved_inv_slot) {
					weapon_list.Add(left_wep);
					weapon_list[weapon_list.Count - 1].set_render(false);
				}
				else {
					weapon_list[inv_slot] = left_wep;
					weapon_list[inv_slot].set_render(false);

					moved_inv_slot = true;
				}

				active_weapon_list[(int)Weapon_Slot.LEFT_HAND] = null;
			}
			else if (equip_slot == Weapon_Slot.RIGHT_HAND && right_wep != null) {
				if (moved_inv_slot) {
					weapon_list.Add(right_wep);
					weapon_list[weapon_list.Count - 1].set_render(false);
				}
				else {
					weapon_list[inv_slot] = right_wep;
					weapon_list[inv_slot].set_render(false);

					moved_inv_slot = true;
				}

				active_weapon_list[(int)Weapon_Slot.RIGHT_HAND] = null;
			}
		}

		// This will only be true if we never replaced the weapon in the inventory.
		// (meaning the weapon slot we equipped to was empty)
		// If that slot is still null, get rid of the slot since its just wasting space.
		if (weapon_list[inv_slot] == null) {
			weapon_list.RemoveAt(inv_slot);
		}

		weapon_to_equip.set_render(true);
		active_weapon_list[(int)equip_slot] = weapon_to_equip;

		if (is_player_inventory) player.update_stats();

		Debug.Log("Equipped weapon with name: " + weapon_to_equip.template.name + ", at slot: " + ((int)equip_slot).ToString());
	}

	internal void equip_armor(int slot, Armor_Class equip_slot) {

		if (armor_list.Count < slot + 1) {

			Debug.LogError("Can't equip armor at inventory slot: " + slot.ToString() + " because there is no armor at that slot.");
			return;
		}

		Armor armor_to_equip = armor_list[slot];

		if (active_armor_list[(int)equip_slot] != null) {

			armor_list[slot] = active_armor_list[(int)equip_slot];

			armor_list[slot].set_render(false);

			Debug.Log("Replacing weapon: " + armor_list[slot].template.name + " with weapon: " + armor_to_equip.template.name);
		}

		// This will only be true if we never replaced the armor in the inventory.
		// If that slot is still null, get rid of the slot since its just wasting space.
		if (armor_list[slot] == null) armor_list.RemoveAt(slot);

		armor_to_equip.set_render(true);
		active_armor_list[(int)equip_slot] = armor_to_equip;

		if (is_player_inventory) player.update_stats();

		Debug.Log("Equipped armor with name: " + armor_to_equip.template.name);
	}

	public void equip_pet_item(int slot) {

		// TODO: Implement equipping pet items
	}

	public void unequip_weapon(Weapon_Slot unequip_slot) {
		Weapon weapon = active_weapon_list[(int)unequip_slot];
		if (weapon != null) {
			weapon_list.Add(weapon);
			active_weapon_list[(int)unequip_slot] = null;
			weapon.unequip();
		}

		Debug.Log("Unequipped weapon with name: " + weapon.template.name);
	}

	public void unequip_armor(Armor_Class unequip_slot) {
		Armor armor = active_armor_list[(int)unequip_slot];
		if (armor != null) {
			armor_list.Add(armor);
			active_armor_list[(int)unequip_slot] = null;
			armor.unequip();

			Debug.Log("Unequipped armor with name: " + armor.template.name);
		}
	}

	public bool has_learned(string ability_ID) {

		return m_learned_ability_list.ContainsKey(ability_ID);
	}

	internal void learn_ability(string ability_ID, Ability_Type ability) {

		if (!has_learned(ability_ID)) {
			
			if (ability.min_player_level <= player.player_level) {

				Ability_Template template = (Ability_Template)Activator.CreateInstance(ability.type);
				m_learned_ability_list.Add(ability_ID, template);
				
				if (is_player_inventory) {

					Game_Controller.invoke_player_learned(template, template.ability_level);
				}

				Debug.Log("Learned ability with ID: " + ability_ID);
				return;
			}

			Debug.LogError("Player can't learn ability with ID: " + ability_ID + " because the player is not a high enough level.");
			return;
		}

		Debug.LogError("Can't learn ability with ID: " + ability_ID + " because the player already knows this ability.");
	}

	public void equip_ability(string ability_ID, Ability_Slot active_slot) {

		if (!has_learned(ability_ID)) {

			Debug.LogError("Can't equip ability with ID: " + ability_ID + " because the player has not learned that ability.");
			return;
		}

		active_ability_list[(int)active_slot] = learned_ability_list[ability_ID];

		Debug.Log("Equipped ability with ID: " + ability_ID + " to slot: " + ((int)active_slot).ToString());
	}
}
