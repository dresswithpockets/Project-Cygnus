using UnityEngine;
using System.Collections.Generic;

public abstract class ArmorTemplate : ItemTemplate  {

	private Armor m_armor_object;
	public Armor armor_object {

		get {

			return m_armor_object;
		}
		internal set {

			m_armor_object = value;
		}
	}

	public abstract ArmorClass classification { get; }

	public abstract List<StatModifier> stat_mods { get; }

	public virtual void active_update(PlayerController player) { }

	public virtual void active_update(NPCController npc) { }

	public virtual void equipped(PlayerController player, int slot) { }

	public virtual void equipped(NPCController npc, int slot) { }

	public virtual void unequipped(PlayerController player) { }

	public virtual void unequipped(NPCController npc) { }
}
