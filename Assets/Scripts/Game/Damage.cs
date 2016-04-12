using UnityEngine;
using System.Collections;

public struct Damage
{
	public Entity attacker;
	public Damage_Type damageType;
	public float damage;

	public Damage(float damage, Entity attacker, Damage_Type type = Damage_Type.PHYSICAL)
	{
		this.damage = damage;
		this.damageType = type;
		this.attacker = attacker;
	}
}
