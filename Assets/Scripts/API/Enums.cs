using UnityEngine;
using System;
using System.Collections;

public enum AbilityTier {

	BASIC = 0,
	INTERMEDIATE,
	ADVANCED
}

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

public enum ArmorClass {

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

	ARMOR = 0, // Resistance to physical
	RESI, // Resistance to magic
	HP, // Total health modifier
	CRIT, // Chance of getting a crit hit. Percentage
	TEMPO, // Speed multiplier for attacks. Percentage
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
	WEAPON_RIGHT_HAND = 0,
	WEAPON_LEFT_HAND = 1,

	HEAD = 0,
	SHOULDERS = 1,
	TORSO = 2,
	HANDS = 4,
	LEGS = 6,
	FEET = 8,
	NECK = 16
}