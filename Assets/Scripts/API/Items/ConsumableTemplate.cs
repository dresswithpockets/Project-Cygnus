using UnityEngine;
using System.Collections;

public abstract class ConsumableTemplate : ItemTemplate {

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

	public virtual void consume(PlayerController player) { }

	public virtual void consume(NPCController npc) { }

	public virtual void consume(PetItem pet) { }
}
