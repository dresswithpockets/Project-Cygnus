using UnityEngine;
using System.Collections;

// Pet Items are inventory-able or item representations of pets. 
// Pet_Item_Template is also where all the template magic happens
// for custom pets. Similar to a type that inherits NPC_Template.
//
//		- TH 4/12/2016
//
public abstract class Pet_Item_Template : Item_Template {

	private Pet_Item_Template m_pet_item_object;
	public Pet_Item_Template pet_item_object {

		get {

			return m_pet_item_object;
		}
		internal set {

			m_pet_item_object = value;
		}
	}

	public abstract float current_hp { get; }

	public abstract float current_max_hp { get; }

	public abstract bool use_default_hp_bar { get; }

	public virtual void active_update(Player_Controller player) { }

	public virtual void active_update(NPC npc) { }
}
