using UnityEngine;
using System;
using System.Collections;

public enum ItemClassification {

	CONSUMABLE = 0,
	MATERIAL
}

public enum WeaponClassification {

	MELEE = 0,
	RANGED_PROJECTILE,
	RANGED_HITSCAN
}

public enum WeaponHandiness {

	ONE_HANDED = 0,
	TWO_HANDED
}

public enum ArmorSlot {

	HEAD = 0,
	SHOULDERS,
	TORSO,
	HANDS,
	LEGS,
	FEET,
	NECK,
}

public enum ItemOwner {

	NPC = 0,
	PLAYER,
	NONE
}

public enum DamageType {

	PHYSICAL = 0,
	MAGIC
}

public enum DamageElement {

	NONE = 0,
	FIRE,
	ICE
}

public enum CharStat {

	/// <summary>
	/// Total Armor modifier.
	/// Resistance to physical damage.
	/// </summary>
	ARMOR = 0, // Resistance to physical

	/// <summary>
	/// Total Magic Resistance modifier.
	/// Reistance to magic damage.
	/// </summary>
	RESI, // Resistance to magic

	/// <summary>
	/// Total HP Modifier.
	/// </summary>
	HP, // Total health modifier

	/// <summary>
	/// Total Magic modifier.
	/// </summary>
	MP, // Total magic modifier

	/// <summary>
	/// Total Stamina modifier.
	/// </summary>
	STAM, // Total stamina modifier

	/// <summary>
	/// Stamina regeneration per second.
	/// </summary>
	DEX, // Stamina regen per second

	/// <summary>
	/// Total critical hit chance modifier.
	/// Value ranging from 0 to 1, chance of getting a critical hit.
	/// </summary>
	CRIT, // Chance of getting a crit hit. Percentage

	/// <summary>
	/// Total Tempo modifier.
	/// Value ranging from 0 to infinity.
	/// Delay between next attack = 1 / Tempo.
	/// </summary>
	TEMPO, // Speed multiplier for attacks. Percentage

	/// <summary>
	/// Total HP Regeneration Per Second modifier.
	/// Every frame, HPRegen * Time.deltaTime is added to the player's HP pool if the player hasn't recently been damaged.
	/// </summary>
	REG // HP Regen per second
}

public enum StatModifierType {

	PRODUCT = 0,
	SUM
}

public enum InventoryCategory {

	WEAPON = 0,
	ARMOR,
	MATERIAL,
	CONSUMABLE,
	PET_ITEM
}

public enum ModelSection {

	VERTS = 0,
	COLS,
	TRIS
}

public enum WeaponSlot {

	RIGHT_HAND = 0,
	LEFT_HAND,
}

public enum AbilitySlot {

	SLOT_1 = 0,
	SLOT_2,
	SLOT_3,
	SLOT_4
}

public enum ImageType {

	PNG = 0,
	JPG
}

public enum ScdFlag {

	CHANGES_POSITION = 0x0,
	CHANGES_ROTATION = 0x1,
	CHANGES_SCALE = 0x2,
}

public enum InterpType {

	ASSIGN = 0, // Assign
	LINEAR, // Lerp
	SPHERICAL // Slerp
}

public enum DebugWarningLevel {
	NORMAL = 0,
	WARNING,
	ERROR
}

public enum EquippableSlot {
	// Armor

	HEAD = 0,
	SHOULDERS = 1,
	TORSO = 2,
	HANDS = 3,
	LEGS = 4,
	FEET = 5,
	NECK = 6,

	// Weapons

	RIGHT_HAND = 7,
	LEFT_HAND = 8,
	BOTH_HANDS = 9,

	// Lantern

	LANTERN = 10,

	// Boat

	BOAT = 11,

	// Pets

	PET = 12
}

public enum AbilityType {
	ACTIVE = 0,
	PASSIVE = 1
}