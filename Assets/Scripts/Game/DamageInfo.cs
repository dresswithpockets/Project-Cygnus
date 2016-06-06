using UnityEngine;
using System.Collections;

public struct DamageInfo {

	public readonly DamageType Type;
	public readonly DamageElement Element;
	public readonly float Damage;
	public readonly Pawn Source;

	public DamageInfo(DamageType type, DamageElement element, float dmg, Pawn source) {
		Type = type;
		Element = element;
		Damage = dmg;
		Source = source;
	}

	public static explicit operator vp_DamageInfo(DamageInfo di) {
		return new vp_DamageInfo(di.Damage, di.Source.transform);
	}
}
