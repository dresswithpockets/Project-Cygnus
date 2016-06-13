using UnityEngine;
using System.Collections;

public struct StatMod {
	public readonly CharStat Stat;
	public readonly StatModifierType Type;
	public readonly float Value;

	public StatMod(CharStat stat, float value, StatModifierType type = StatModifierType.SUM) {
		Stat = stat;
		Value = value;
		Type = type;
	}
}

public abstract class Equippable : Item {
	public bool Active { get; set; }

	public abstract EquippableSlot Slot { get; }

	public abstract StatMod[] StatModifiers { get; }
}
