using UnityEngine;
using System.Collections;

public struct Damage {
	public Entity attacker;
	public Damage_Type type;
	public float damage;

	public Damage(float damage, Entity attacker, Damage_Type type = Damage_Type.PHYSICAL) {

		this.damage = damage;
		this.type = type;
		this.attacker = attacker;
	}
}
