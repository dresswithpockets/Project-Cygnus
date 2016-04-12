using UnityEngine;
using System.Collections.Generic;
using System;

public class Inventory_Controller : MonoBehaviour {
	internal List<Item> m_item_inv = new List<Item>();
	internal List<Equipment> m_equipment_inv = new List<Equipment>();
	internal List<Weapon> m_weapon_inv = new List<Weapon>();

	public bool is_casting_ability = false;
	internal Dictionary<string, Ability_Template> m_learned_abilities = new Dictionary<string, Ability_Template>();
	internal Ability_Template m_ability_1;
	internal Ability_Template m_ability_2;
	internal Ability_Template m_ability_3;
	internal Ability_Template m_ability_4;

	private bool m_player_inv = false;

	#region Properties

	public bool player_inventory {

		get {

			return m_player_inv;
		}
		internal set {

			m_player_inv = value;
		}
	}

	public List<Item> item_inv {

		get {

			return m_item_inv;
		}
		internal set {

			m_item_inv = value;
		}
	}

	public List<Equipment> equipment_inv {

		get {

			return m_equipment_inv;
		}
		internal set {

			m_equipment_inv = value;
		}
	}

	public List<Weapon> weapon_inv {
		get {
			return m_weapon_inv;
		}
		internal set {
			m_weapon_inv = value;
		}
	}

	#endregion

	public void pickup(Item item) {

		item_inv.Add(item);

		MeshRenderer[] renderers = item.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers) {
			r.enabled = true;
		}

		if (player_inventory) {
			Game_Controller.invoke_player_picked_up(item);
		}
	}

	public void pickup(Equipment equipment) {

		equipment_inv.Add(equipment);

		MeshRenderer[] renderers = equipment.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers) {
			r.enabled = true;
		}

		if (player_inventory) {
			Game_Controller.invoke_player_picked_up(equipment);
		}
	}

	public void pickup(Weapon weapon) {

		weapon_inv.Add(weapon);

		MeshRenderer[] renderers = weapon.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers) {
			r.enabled = true;
		}

		if (player_inventory) {
			Game_Controller.invoke_player_picked_up(weapon);
		}
	}

	public void drop(Inventory_Category tab, int slot) {

		GameObject dropped_item = null;

		switch (tab) {
			case Inventory_Category.EQUIPMENT:
				dropped_item = equipment_inv[slot].gameObject;

				if (player_inventory) {
					Game_Controller.invoke_player_dropped(equipment_inv[slot]);
				}
				break;
			case Inventory_Category.ITEM:
				dropped_item = item_inv[slot].gameObject;

				if (player_inventory) {
					Game_Controller.invoke_player_dropped(item_inv[slot]);
				}
				break;
			case Inventory_Category.WEAPON:
				dropped_item = weapon_inv[slot].gameObject;

				if (player_inventory) {
					Game_Controller.invoke_player_dropped(weapon_inv[slot]);
				}
				break;
		}

		MeshRenderer[] renderers = dropped_item.GetComponentsInChildren<MeshRenderer>();
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
