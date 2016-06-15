using UnityEngine;
using System.Collections;

public struct DamageInfo {

	// For damage over time
	public readonly bool Passed;

	public readonly DamageType Type;
	public readonly DamageElement Element;
	public readonly float Damage;
	public readonly Pawn Source;

	public DamageInfo(DamageType type, DamageElement element, float dmg, Pawn source, bool passed = false) {
		Type = type;
		Element = element;
		Damage = dmg;
		Source = source;
		Passed = passed;
	}

	public DamageInfo(DamageInfo info, bool passed = false) : this(info.Type, info.Element, info.Damage, info.Source, passed) { }

	public static explicit operator vp_DamageInfo(DamageInfo di) {
		return new vp_DamageInfo(di.Damage, di.Source.transform);
	}
}
