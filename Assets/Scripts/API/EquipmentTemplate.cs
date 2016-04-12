using UnityEngine;
using System.Collections.Generic;

public abstract class Equipment_Template  {

	private GameObject m_owner;
	public GameObject owner {

		get {

			return m_owner;
		}
		internal set {

			m_owner = value;
		}
	}

	private Equipment m_equipment_object;
	public Equipment equipment_object {

		get {

			return m_equipment_object;
		}
		internal set {

			m_equipment_object = value;
		}
	}

	public abstract string name { get; }

	public abstract string description { get; }

	public abstract bool used_by_AI { get; }

	public abstract Equipment_Classification classification { get; }

	public abstract List<Stat_Modifier> stat_mods { get; }

	public abstract string model_ID { get; }

	public virtual void spawned() { }

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
}
