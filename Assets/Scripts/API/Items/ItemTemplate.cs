using UnityEngine;
using System.Collections;

// an "Item" is the base class for all inventory-able entities.
// equipment, weapons, materials, consumables, pet foods and pet items all fall under this category.
// entities such as projectiles, NPCs, players, and pets do not fall under this category.
// however, a pet's inventory representation (pet item) does fall under this category.
//
//		- TH 4/12/2016
//
public abstract class Item_Template  {

	private GameObject m_owner;
	public GameObject owner {

		get {

			return m_owner;
		}
		internal set {

			m_owner = value;
		}
	}

	public abstract string name { get; }

	public abstract string description { get; }

	public abstract bool used_by_AI { get; }

	public abstract string model_ID { get; }

	public virtual void spawned() { }

	public virtual void exists_update() { }

	public virtual void passive_update(Player_Controller player) { }

	public virtual void passive_update(NPC npc) { }

	public virtual void fixed_update() { }

	public virtual void late_update() { }

	public virtual void picked_up(Player_Controller player) { }

	public virtual void picked_up(NPC npc) { }

	public virtual void dropped(Player_Controller player) { }

	public virtual void dropped(NPC npc) { }
}
