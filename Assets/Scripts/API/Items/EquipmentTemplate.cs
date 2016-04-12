using UnityEngine;
using System.Collections.Generic;

public abstract class Equipment_Template : Item_Template  {

	private Equipment m_equipment_object;
	public Equipment equipment_object {

		get {

			return m_equipment_object;
		}
		internal set {

			m_equipment_object = value;
		}
	}

	public abstract Equipment_Classification classification { get; }

	public abstract List<Stat_Modifier> stat_mods { get; }

	public virtual void equipped(Player_Controller player, int slot) { }

	public virtual void equipped(NPC npc, int slot) { }
}
