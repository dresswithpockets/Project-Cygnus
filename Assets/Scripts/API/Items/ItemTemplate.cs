using UnityEngine;
using System.Collections;

// an "Item" is the base class for all inventory-able entities.
// armor, weapons, materials, consumables, pet foods and pet items all fall under this category.
// entities such as projectiles, NPCs, players, and pets do not fall under this category.
// however, a pet's inventory representation (pet item) does fall under this category.
//
//		- TH 4/12/2016
//
public abstract class Item_Template  {

	private GameObject m_game_object;
	public GameObject game_object {

		get {

			return m_game_object;
		}
		internal set {

			m_game_object = value;
		}
	}

	public abstract string name { get; }

	public abstract string description { get; }

	public abstract bool used_by_AI { get; }

	public abstract string model_ID { get; }

	public virtual void spawned() { }

	public virtual void exists_update() { }

	public virtual void passive_update(Player_Controller player) { }

	public virtual void passive_update(NPC_Controller npc) { }

	public virtual void fixed_update() { }

	public virtual void late_update() { }

	public virtual void picked_up(Player_Controller player) { }

	public virtual void picked_up(NPC_Controller npc) { }

	public virtual void dropped(Player_Controller player) { }

	public virtual void dropped(NPC_Controller npc) { }

	public bool is_weapon() {
		return typeof(Weapon_Template) == this.GetType();
	}

	public bool is_material() {
		return typeof(Material_Template) == this.GetType();
	}

	public bool is_pet_item() {
		return typeof(Pet_Item_Template) == this.GetType();
	}

	public bool is_armor() {
		return typeof(Armor_Template) == this.GetType();
	}

	public bool is_consumable() {
		return typeof(Consumable_Template) == this.GetType();
	}
}
