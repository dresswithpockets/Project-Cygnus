using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class Weapon_Template {

	private GameObject m_owner;
	public GameObject owner {

		get {

			return owner;
		}
		internal set {

			m_owner = value;
		}
	}

	private Weapon m_weapon;
	public Weapon weapon {

		get {

			return m_weapon;
		}
		internal set {

			m_weapon = value;
		}
	}

	public abstract string name { get; }

	public abstract string description { get; }

	public abstract bool used_by_AI { get; }

	public abstract Weapon_Classification classification { get; }

	public abstract Weapon_Handiness handiness { get; }

	public abstract List<Stat_Modifier> stat_mods { get; }

	public abstract string model_ID { get; }

	public virtual void spawned() { }

	public virtual void used(Player_Controller player) { }

	public virtual void used(NPC npc) { }

	public virtual void exists_update() { }

	public virtual void passive_update(Player_Controller player) { }

	public virtual void passive_update(NPC npc) { }

	public virtual void fixed_update() { }

	public virtual void late_update() { }

	public virtual void picked_up(Player_Controller player) { }

	public virtual void picked_up(NPC npc) { }

	public virtual void dropped(Player_Controller player) { }

	public virtual void dropped(NPC npc) { }

	public virtual void equipped(Player_Controller player, int slot) { }

	public virtual void equipped(NPC npc, int slot) { }

	// Used for AI, only called if the AI in question has a target and the item can be used by AI
	// @See UsedByAI
	public virtual bool AI_can_use_on_target(NPC npc, Entity target) { return false; }
}