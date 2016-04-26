using UnityEngine;
using System.Collections;

public abstract class Consumable_Template : Item_Template {

	private Consumable m_consumable_object;
	public Consumable consumable_object {
		
		get {

			return m_consumable_object;
		}
		internal set {

			m_consumable_object = value;
		}
	}

	public abstract bool is_pet_food { get; } // A consumable can either be player/npc food or it can be pet food.

	public virtual void consume(Player_Controller player) { }

	public virtual void consume(NPC_Controller npc) { }

	public virtual void consume(Pet_Item pet) { }
}
