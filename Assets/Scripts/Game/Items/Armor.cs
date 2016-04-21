using UnityEngine;
using System.Collections;

public sealed class Armor : Item {
	
	public Armor_Template armor_template {

		get {

			return (Armor_Template)m_template;
		}
		internal set {

			m_template.game_object = gameObject;
			((Armor_Template)m_template).armor_object = this;
			m_template = value;
		}
	}

	public void active_update() {
		switch (ownership) {
			case Item_Owner.NONE:

				Debug.LogError("Can't call active_update on this armor because no owner was assigned before the event was completed", this);

				break;
			case Item_Owner.NPC:

				if (template != null) armor_template.active_update(m_NPC_owner);

				break;
			case Item_Owner.PLAYER:

				if (template != null) armor_template.active_update(m_player_owner);

				break;
		}
	}

	public void equip(int slot) {

		switch (ownership) {

			case Item_Owner.NONE:

				Debug.LogError("Cannot equip armor as no NPC or Player owns this item.", this);

				break;
			case Item_Owner.NPC:

				if (template != null) armor_template.equipped(m_NPC_owner, slot);

				break;
			case Item_Owner.PLAYER:

				if (template != null) armor_template.equipped(m_player_owner, slot);

				break;
		}
	}

	internal void unequip() {
		switch (ownership) {
			case Item_Owner.NONE:

				Debug.LogError("Cannot unequip armor as no NPC or Player owns this item.", this);

				break;
			case Item_Owner.NPC:

				if (template != null) armor_template.unequipped(m_NPC_owner);

				break;
			case Item_Owner.PLAYER:

				if (template != null) armor_template.unequipped(m_player_owner);

				break;
		}
	}
}
