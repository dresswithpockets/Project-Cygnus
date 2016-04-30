using UnityEngine;
using System;
using System.Collections;

public enum Ability_Tier {

	BASIC = 0,
	INTERMEDIATE,
	ADVANCED
}

public enum Item_Classification {

	CONSUMABLE = 0,
	MATERIAL
}

public enum Weapon_Classification {

	MELEE = 0,
	RANGED_PROJECTILE,
	RANGED_HITSCAN
}

public enum Weapon_Handiness {

	ONE_HANDED = 0,
	TWO_HANDED
}

public enum Armor_Class {

	HEAD = 0,
	SHOULDERS,
	TORSO,
	HANDS,
	LEGS,
	FEET,
	NECK,
}

public enum Item_Owner {

	NPC = 0,
	PLAYER,
	NONE
}

public enum Damage_Type {

	PHYSICAL = 0,
	MAGIC,
	PURE
}

public enum Char_Stat {

	ARMOR = 0, // Resistance to physical
	RESI, // Resistance to magic
	HP, // Total health modifier
	CRIT, // Chance of getting a crit hit. Percentage
	TEMPO, // Speed multiplier for attacks. Percentage
	REG // HP Regen per second
}

public enum Stat_Modifier_Type {

	PRODUCT = 0,
	SUM
}

public enum Inventory_Category {

	WEAPON = 0,
	ARMOR,
	MATERIAL,
	CONSUMABLE,
	PET_ITEM
}

public enum Model_Section {

	VERTS = 0,
	COLS,
	TRIS
}

public enum Weapon_Slot {

	RIGHT_HAND = 0,
	LEFT_HAND,
}

public enum Ability_Slot {

	SLOT_1 = 0,
	SLOT_2,
	SLOT_3,
	SLOT_4
}

public enum Image_Type {

	PNG = 0,
	JPG
}

public enum Scd_Flag {

	CHANGES_POSITION = 0x0,
	CHANGES_ROTATION = 0x1,
	CHANGES_SCALE = 0x2,
}

public enum Interp_Type {

	ASSIGN = 0, // Assign
	LINEAR, // Lerp
	SPHERICAL // Slerp
}

public enum Debug_Warning_Level {
	NORMAL = 0,
	WARNING,
	ERROR
}