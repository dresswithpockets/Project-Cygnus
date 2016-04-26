using UnityEngine;
using System.Collections.Generic;

public abstract class Armor_Template : Item_Template  {

	private Armor m_armor_object;
	public Armor armor_object {

		get {

			return m_armor_object;
		}
		internal set {

			m_armor_object = value;
		}
	}

	public abstract Armor_Class classification { get; }

	public abstract List<Stat_Modifier> stat_mods { get; }

	public virtual void active_update(Player_Controller player) { }

	public virtual void active_update(NPC_Controller npc) { }

	public virtual void equipped(Player_Controller player, int slot) { }

	public virtual void equipped(NPC_Controller npc, int slot) { }

	public virtual void unequipped(Player_Controller player) { }

	public virtual void unequipped(NPC_Controller npc) { }
}
