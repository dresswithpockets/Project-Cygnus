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

	public abstract bool pets_can_eat { get; }

	public virtual void consume(Player_Controller player) { }

	public virtual void consume(NPC npc) { }

	public virtual void consume(Pet_Item_Template pet) { } // TODO: Once Pet_Item MonoBehaviour is implemented, change this param type to Pet_Item.
}
