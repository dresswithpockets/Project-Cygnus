using UnityEngine;
using System.Collections;



public enum ItemClassification
{
	Consumable,
	Material
}

public enum WeaponClassification
{
	Melee,
	RangedProjectile,
	RangedHitscan
}

public enum WeaponHandiness
{
	OneHanded,
	TwoHanded
}

public enum EquipmentClassification
{
	Head,
	Torso,
	Legs,
	Shoulders,
	Hands,
	Feet,
	Neck,
	Pet,
	Boat,
	Flight
}

public enum ItemOwner
{
	NPC,
	Player,
	None
}

public enum DamageType
{
	Physical,
	Magic,
	Pure
}

public enum Stat
{
	Armor, // Resistance to physical
	Resi, // Resistance to magic
	HP, // Total health modifier
	Crit, // Chance of getting a crit hit. Percentage
	Tempo, // Speed multiplier for attacks. Percentage
	Reg // HP Regen per second
}

public enum StatModifierType
{
	Product,
	Sum
}

public enum InventoryCategory
{
	Weapon,
	Item,
	Equipment
}