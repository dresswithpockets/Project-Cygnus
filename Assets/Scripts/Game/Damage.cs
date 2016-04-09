using UnityEngine;
using System.Collections;

public struct Damage
{
	public Entity attacker;
	public DamageType damageType;
	public float damage;

	public Damage(float damage, Entity attacker, DamageType type = DamageType.Physical)
	{
		this.damage = damage;
		this.damageType = type;
		this.attacker = attacker;
	}
}
