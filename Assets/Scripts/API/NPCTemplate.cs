using UnityEngine;
using System.Collections;

public abstract class NPC_Template {

	private GameObject m_owner;
	public GameObject owner {

		get {

			return m_owner;
		}
		internal set {

			m_owner = value;
		}
	}

	private NPCController m_NPC;
	public NPCController NPC {
		get {

			return m_NPC;
		}
		internal set {

			m_NPC = value;
		}
	}

	public abstract string name { get; }

	public abstract string description { get; }

	public abstract float current_hp { get; }

	public abstract float current_max_hp { get; }

	public abstract bool use_default_hp_bar { get; }

	public virtual void spawned() { }

	public virtual void died(GameObject attacker) { }

	public virtual void update() { }

	public virtual void fixed_update() { }

	public virtual void late_update() { }

	public virtual void do_damage(Damage damage) { }

	public virtual void goto_pos(Vector3 location, bool pathfind = true) {

		// TODO: Implement default pathfind and goto features for AI.
	}

	public virtual void follow(Transform target, bool pathfind = true) {

		// TODO: implement default pathfind and follow-target features for AI.
	}
}
