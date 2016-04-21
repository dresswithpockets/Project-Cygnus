using UnityEngine;
using System.Collections;

public sealed class Weapon : Item {

	public Weapon_Template weapon_template {

		get {

			return (Weapon_Template)m_template;
		}
		internal set {

			m_template.game_object = gameObject;
			((Weapon_Template)m_template).weapon_object = this;
			m_template = value;
		}
	}

	#region Ownership

	internal Entity m_NPC_target = null;

	public Entity NPC_target {

		get {

			return m_NPC_target;
		}
	}

	public void set_target(GameObject target) {

		if (m_NPC_owner == null) {

			Debug.LogError("Cannot set item target because this item is not owned by an NPC.");
			return;
		}

		m_NPC_target = (Entity)target;
	}

	public void set_target(Entity target) {

		if (m_NPC_owner == null) {

			Debug.LogError("Cannot set item target because this item is not owned by an NPC.");
			return;
		}

		m_NPC_target = target;
	}

	internal void drop_owner() {

		player_owner = null;
		NPC_owner = null;
	}

	#endregion

	public override void Update() {
		base.Update();
	}

	internal void active_update() {
		switch (ownership) {
			case Item_Owner.NONE:

				Debug.LogError("Can't call active_update on the weapon because no owner was assigned before the event was completed", this);

				break;
			case Item_Owner.NPC:

				if (template != null) weapon_template.active_update(m_NPC_owner);

				break;
			case Item_Owner.PLAYER:

				if (template != null) weapon_template.active_update(m_player_owner);

				break;
		}
	}

	internal void use() {
		switch (ownership) {
			case Item_Owner.NONE:

				Debug.LogError("Cannot use weapon because no owner was assigned before the event was completed.", this);

				break;
			case Item_Owner.NPC:

				if (template != null && weapon_template.AI_can_use_on_target(m_NPC_owner, m_NPC_target))
					weapon_template.primary_used(m_NPC_owner);

				break;
			case Item_Owner.PLAYER:

				if (template != null) weapon_template.primary_used(m_player_owner);

				break;
		}
	}

	internal void equip(Weapon_Slot slot) {
		switch (ownership) {
			case Item_Owner.NONE:

				Debug.LogError("Cannot equip weapon as no NPC or Player owns this item.", this);

				break;
			case Item_Owner.NPC:

				if (template != null) weapon_template.equipped(m_NPC_owner, slot);

				break;
			case Item_Owner.PLAYER:

				if (template != null) weapon_template.equipped(m_player_owner, slot);

				break;
		}
	}

	internal void unequip() {
		switch (ownership) {
			case Item_Owner.NONE:

				Debug.LogError("Cannot unequip weapon as no NPC or Player owns this item.", this);

				break;
			case Item_Owner.NPC:

				if (template != null) weapon_template.unequipped(m_NPC_owner);

				break;
			case Item_Owner.PLAYER:

				if (template != null) weapon_template.unequipped(m_player_owner);

				break;
		}
	}
}
