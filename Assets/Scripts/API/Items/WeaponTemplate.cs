﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class Weapon_Template : Item_Template {

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

	public abstract Weapon_Classification classification { get; }

	public abstract Weapon_Handiness handiness { get; }

	public abstract List<Stat_Modifier> stat_mods { get; }

	public virtual void active_update(Player_Controller player) { }

	public virtual void active_update(NPC npc) { }

	public virtual void primary_used(Player_Controller player) { }

	public virtual void primary_used(NPC npc) { }

	public virtual void primary_interrupted(Player_Controller player) { }

	public virtual void primary_interrupted(NPC npc) { }

	public virtual void alternate_used(Player_Controller player) { }

	public virtual void alternate_used(NPC npc) { }

	public virtual void alternate_interrupted(Player_Controller player) { }

	public virtual void alternate_interrupted(NPC npc) { }

	public virtual void equipped(Player_Controller player, int slot) { }

	public virtual void equipped(NPC npc, int slot) { }

	// Used for AI, only called if the AI in question has a target and the item can be used by AI
	// @See UsedByAI
	public virtual bool AI_can_use_on_target(NPC npc, Entity target) { return false; }
}