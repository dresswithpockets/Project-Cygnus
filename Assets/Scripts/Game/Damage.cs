using UnityEngine;
using System.Collections;

public struct Damage {
	public Entity attacker;
	public DamageType type;
	public float damage;

	public Damage(float damage, Entity attacker, DamageType type = DamageType.PHYSICAL) {

		this.damage = damage;
		this.type = type;
		this.attacker = attacker;
	}
}
