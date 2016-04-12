using UnityEngine;
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

public enum Equipment_Classification {

	HEAD = 0,
	TORSO,
	LEGS,
	SHOULDERS,
	HANDS,
	FEET,
	NECK,
	PET,
	BOAT,
	FLIGHT
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
	EQUIPMENT,
	MATERIAL,
	CONSUMABLE,
	PET_ITEM
}

public enum Model_Section {

	VERTS = 0,
	COLS,
	TRIS
}