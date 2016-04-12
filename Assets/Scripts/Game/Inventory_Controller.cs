using UnityEngine;
using System.Collections.Generic;
using System;

public class Inventory_Controller : MonoBehaviour {
	
	public bool is_casting_ability = false;

	internal List<Material> m_material_list = new List<Material>();
	internal List<Equipment> m_equipment_list = new List<Equipment>();
	internal List<Weapon> m_weapon_list = new List<Weapon>();
	internal List<Consumable> m_consumable_list = new List<Consumable>();
	internal List<Pet_Item> m_pet_item_list = new List<Pet_Item>();

	internal Dictionary<string, Ability_Template> m_learned_abilities = new Dictionary<string, Ability_Template>();
	internal Ability_Template m_ability_1;
	internal Ability_Template m_ability_2;
	internal Ability_Template m_ability_3;
	internal Ability_Template m_ability_4;

	private bool m_player_inv = false;

	#region Properties

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

	public List<Equipment> equipment_list {

		get {

			return m_equipment_list;
		}
		internal set {

			m_equipment_list = value;
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

	#endregion

	public void pickup(Material item) {

		material_list.Add(item);

		MeshRenderer[] renderers = item.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers) {
			r.enabled = true;
		}

		if (is_player_inventory) {
			Game_Controller.invoke_player_picked_up(item.template);
		}
	}

	public void pickup(Equipment equipment) {

		equipment_list.Add(equipment);

		MeshRenderer[] renderers = equipment.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers) {
			r.enabled = true;
		}

		if (is_player_inventory) {
			Game_Controller.invoke_player_picked_up(equipment.template);
		}
	}

	public void pickup(Weapon weapon) {

		weapon_list.Add(weapon);

		MeshRenderer[] renderers = weapon.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers) {
			r.enabled = true;
		}

		if (is_player_inventory) {
			Game_Controller.invoke_player_picked_up(weapon.template);
		}
	}

	public void pickup(Consumable consumable) {

		consumable_list.Add(consumable);

		MeshRenderer[] renderers = consumable.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers) {
			r.enabled = true;
		}

		if (is_player_inventory) {
			Game_Controller.invoke_player_picked_up(consumable.template);
		}
	}

	public void pickup(Pet_Item pet_item) {

		pet_item_list.Add(pet_item);

		MeshRenderer[] renderers = pet_item.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers) {
			r.enabled = true;
		}

		if (is_player_inventory) {
			Game_Controller.invoke_player_picked_up(pet_item.template);
		}
	}

	public void drop(Inventory_Category tab, int slot) {

		Item_Template dropped_item = null;

		switch (tab) {
			case Inventory_Category.EQUIPMENT:

				dropped_item = equipment_list[slot].template;
				equipment_list.RemoveAt(slot);

				break;
			case Inventory_Category.MATERIAL:

				dropped_item = material_list[slot].template;
				material_list.RemoveAt(slot);

				break;
			case Inventory_Category.WEAPON:

				dropped_item = weapon_list[slot].template;
				weapon_list.RemoveAt(slot);

				break;
			case Inventory_Category.CONSUMABLE:

				dropped_item = consumable_list[slot].template;
				consumable_list.RemoveAt(slot);
				
				break;
			case Inventory_Category.PET_ITEM:

				dropped_item = pet_item_list[slot].template;
				pet_item_list.RemoveAt(slot);

				break;
		}

		if (is_player_inventory) {
			Game_Controller.invoke_player_dropped(dropped_item);
		}

		MeshRenderer[] renderers = dropped_item.owner.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers) {
			r.enabled = true;
		}
	}

	public bool has_learned(string abilityID) {

		return m_learned_abilities.ContainsKey(abilityID);
	}

	internal void learn_ability(string abilityID, Ability_Type ability) {

		if (!has_learned(abilityID)) {

			m_learned_abilities.Add(abilityID, (Ability_Template)Activator.CreateInstance(ability.type));
		}
	}
}
