using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class WeaponTemplate : ItemTemplate {

	private Weapon m_weapon_object;
	public Weapon weapon_object {

		get {

			return m_weapon_object;
		}
		internal set {

			m_weapon_object = value;
		}
	}

	public abstract string primary_avatar_ID { get; }

	public abstract string alternate_avatar_ID { get; }

	public abstract WeaponClassification classification { get; }

	public abstract WeaponHandiness handiness { get; }

	public abstract List<StatModifier> stat_mods { get; }

	public virtual void active_update(PlayerController player) { }

	public virtual void active_update(NPCController npc) { }

	public virtual void primary_used(PlayerController player) { }

	public virtual void primary_used(NPCController npc) { }

	public virtual void primary_interrupted(PlayerController player) { }

	public virtual void primary_interrupted(NPCController npc) { }

	public virtual void alternate_used(PlayerController player) { }

	public virtual void alternate_used(NPCController npc) { }

	public virtual void alternate_interrupted(PlayerController player) { }

	public virtual void alternate_interrupted(NPCController npc) { }

	public virtual void equipped(PlayerController player, WeaponSlot slot) { }

	public virtual void equipped(NPCController npc, WeaponSlot slot) { }

	public virtual void unequipped(PlayerController player) { }

	public virtual void unequipped(NPCController npc) { }

	// Used for AI, only called if the AI in question has a target and the item can be used by AI
	// @See UsedByAI
	public virtual bool AI_can_use_on_target(NPCController npc, Entity target) { return false; }
}