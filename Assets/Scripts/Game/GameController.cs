using UnityEngine;
using System;
using System.Collections.Generic;

public class ItemEventArgs : EventArgs {
	public readonly Item Item;

	public ItemEventArgs(Item item) {
		Item = item;
	}
}

public class PawnEventArgs : EventArgs {
	public readonly Pawn Pawn;

	public PawnEventArgs(Pawn pawn) {
		Pawn = pawn;
	}
}

public class PawnDamagedEventArgs : EventArgs {
	public readonly Pawn Inflicter;
	public readonly Pawn Inflicted;
	public readonly DamageInfo Damage;

	public PawnDamagedEventArgs(Pawn inflicter, Pawn inflicted, DamageInfo damage) {
		Inflicter = inflicter;
		Inflicted = inflicted;
		Damage = damage;
	}
}

public class PawnMovedEventArgs : EventArgs {
	public readonly Pawn Pawn;
	public readonly Vector3 NewPosition;
	public readonly Vector3 DeltaPosition;

	public PawnMovedEventArgs(Pawn pawn, Vector3 newPos, Vector3 delta) {
		Pawn = pawn;
		NewPosition = newPos;
		DeltaPosition = delta;
	}
}

public class PawnItemEventArgs : EventArgs {
	public readonly Pawn Pawn;
	public readonly Item Item;

	public PawnItemEventArgs(Pawn pawn, Item item) {
		Pawn = pawn;
		Item = item;
	}
}

public class PawnWeaponEventArgs : EventArgs {
	public readonly Pawn Pawn;
	public readonly Weapon Weapon;

	public PawnWeaponEventArgs(Pawn pawn, Weapon weapon) {
		Pawn = pawn;
		Weapon = weapon;
	}
}

public class PawnArmorEventArgs : EventArgs {
	public readonly Pawn Pawn;
	public readonly Armor Armor;

	public PawnArmorEventArgs(Pawn pawn, Armor armor) {
		Pawn = pawn;
		Armor = armor;
	}
}

public class PawnLanternEventArgs : EventArgs {
	public readonly Pawn Pawn;
	public readonly Lantern Lantern;

	public PawnLanternEventArgs(Pawn pawn, Lantern lantern) {
		Pawn = pawn;
		Lantern = lantern;
	}
}

public class PawnAbilityEventArgs : EventArgs {
	public readonly Pawn Pawn;
	public readonly Ability Ability;

	public PawnAbilityEventArgs(Pawn pawn, Ability ability) {
		Pawn = pawn;
		Ability = ability;
	}
}

public class PawnPawnEventArgs : EventArgs {
	public readonly Pawn Pawn;
	public readonly Pawn Other;

	public PawnPawnEventArgs(Pawn pawn, Pawn other) {
		Pawn = pawn;
		Other = other;
	}
}

public class GameController : MonoBehaviour {
	
	public bool Paused = false;
	public bool Debug = false;

	public Vector3 SpawnPoint = new Vector3(0f, 0f ,0f);

	public static bool GamePaused {
		get {
			return Instance.Paused;
		}
	}

	public static Type[] EquippableTypes {
		get {
			return new[] { typeof(Armor), typeof(Weapon), typeof(Lantern) };
		}
	}

	#region Events

	public event EventHandler<PawnEventArgs> SpawnedPawn;
	public event EventHandler<ItemEventArgs> SpawnedItem;
	public event EventHandler<PawnEventArgs> Landed;
	public event EventHandler<PawnEventArgs> Jumped;
	public event EventHandler<PawnEventArgs> Died;
	public event EventHandler<PawnDamagedEventArgs> Damaged;
	public event EventHandler<PawnMovedEventArgs> Moved;
	public event EventHandler<PawnEventArgs> Update;
	public event EventHandler<PawnEventArgs> FixedUpdate;
	public event EventHandler<PawnEventArgs> LateUpdate;
	public event EventHandler<PawnItemEventArgs> UsedItem;
	public event EventHandler<PawnItemEventArgs> DroppedItem;
	public event EventHandler<PawnItemEventArgs> PickedUpItem;
	public event EventHandler<PawnItemEventArgs> CraftedItem;
	public event EventHandler<PawnWeaponEventArgs> EquippedWeapon;
	public event EventHandler<PawnArmorEventArgs> EquippedArmor;
	public event EventHandler<PawnLanternEventArgs> EquippedLantern;
	public event EventHandler<PawnWeaponEventArgs> UnequippedWeapon;
	public event EventHandler<PawnArmorEventArgs> UnequippedArmor;
	public event EventHandler<PawnLanternEventArgs> UnequippedLantern;
	public event EventHandler<PawnAbilityEventArgs> LearnedAbility;
	public event EventHandler<PawnAbilityEventArgs> EquippedAbility;
	public event EventHandler<PawnAbilityEventArgs> UnequippedAbility;
	public event EventHandler<PawnAbilityEventArgs> TriggeredAbility;
	public event EventHandler<PawnPawnEventArgs> DefeatedOther;

	public static void InvokeSpawned(object sender, Pawn pawn) {
		if (Instance.SpawnedPawn != null) Instance.SpawnedPawn(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeSpawned(object sender, Item item) {
		if (Instance.SpawnedItem != null) Instance.SpawnedItem(sender, new ItemEventArgs(item));
	}

	public static void InvokeLanded(object sender, Pawn pawn) {
		if (Instance.Landed != null) Instance.Landed(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeJumped(object sender, Pawn pawn) {
		if (Instance.Jumped != null) Instance.Jumped(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeDied(object sender, Pawn pawn) {
		if (Instance.Died != null) Instance.Died(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeDamaged(object sender, Pawn inflicter, Pawn inflicted, DamageInfo damage) {
		if (Instance.Damaged != null) Instance.Damaged(sender, new PawnDamagedEventArgs(inflicter, inflicted, damage));
	}

	public static void InvokeMoved(object sender, Pawn pawn, Vector3 newPos, Vector3 delta) {
		if (Instance.Moved != null) Instance.Moved(sender, new PawnMovedEventArgs(pawn, newPos, delta));
	}

	public static void InvokeUpdate(object sender, Pawn pawn) {
		if (Instance.Update != null) Instance.Update(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeFixedUpdate(object sender, Pawn pawn) {
		if (Instance.FixedUpdate != null) Instance.FixedUpdate(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeLateUpdate(object sender, Pawn pawn) {
		if (Instance.LateUpdate != null) Instance.LateUpdate(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeUsedItem(object sender, Pawn pawn, Item item) {
		if (Instance.UsedItem != null) Instance.UsedItem(sender, new PawnItemEventArgs(pawn, item));
	}

	public static void InvokePickedUpItem(object sender, Pawn pawn, Item item) {
		if (Instance.PickedUpItem != null) Instance.PickedUpItem(sender, new PawnItemEventArgs(pawn, item));
	}

	public static void InvokeDroppedItem(object sender, Pawn pawn, Item item) {
		if (Instance.DroppedItem != null) Instance.DroppedItem(sender, new PawnItemEventArgs(pawn, item));
	}

	public static void InvokeCraftedItem(object sender, Pawn pawn, Item item) {
		if (Instance.CraftedItem != null) Instance.CraftedItem(sender, new PawnItemEventArgs(pawn, item));
	}

	public static void InvokeEquippedWeapon(object sender, Pawn pawn, Weapon weapon) {
		if (Instance.EquippedWeapon != null) Instance.EquippedWeapon(sender, new PawnWeaponEventArgs(pawn, weapon));
	}

	public static void InvokeEquippedArmor(object sender, Pawn pawn, Armor armor) {
		if (Instance.EquippedArmor != null) Instance.EquippedArmor(sender, new PawnArmorEventArgs(pawn, armor));
	}

	public static void InvokeEquippedLantern(object sender, Pawn pawn, Lantern lantern) {
		if (Instance.EquippedLantern != null) Instance.EquippedLantern(sender, new PawnLanternEventArgs(pawn, lantern));
	}

	public static void InvokeEquippedAbility(object sender, Pawn pawn, Ability ability) {
		if (Instance.EquippedAbility != null) Instance.EquippedAbility(sender, new PawnAbilityEventArgs(pawn, ability));
	}

	public static void InvokeUnequippedWeapon(object sender, Pawn pawn, Weapon weapon) {
		if (Instance.UnequippedWeapon != null) Instance.UnequippedWeapon(sender, new PawnWeaponEventArgs(pawn, weapon));
	}

	public static void InvokeUnequippedArmor(object sender, Pawn pawn, Armor armor) {
		if (Instance.UnequippedArmor != null) Instance.UnequippedArmor(sender, new PawnArmorEventArgs(pawn, armor));
	}

	public static void InvokeUnequippedLantern(object sender, Pawn pawn, Lantern lantern) {
		if (Instance.UnequippedLantern != null) Instance.UnequippedLantern(sender, new PawnLanternEventArgs(pawn, lantern));
	}

	public static void InvokeUnequippedAbility(object sender, Pawn pawn, Ability ability) {
		if (Instance.UnequippedAbility != null) Instance.UnequippedAbility(sender, new PawnAbilityEventArgs(pawn, ability));
	}

	public static void InvokeTriggeredAbility(object sender, Pawn pawn, Ability ability) {
		if (Instance.TriggeredAbility != null) Instance.TriggeredAbility(sender, new PawnAbilityEventArgs(pawn, ability));
	}

	public static void InvokeLearnedAbility(object sender, Pawn pawn, Ability ability) {
		if (Instance.LearnedAbility != null) Instance.LearnedAbility(sender, new PawnAbilityEventArgs(pawn, ability));
	}

	#endregion

	#region Construction & Singleton

	private static GameController m_Instance = null;
	public static GameController Instance {
		get {
			if (m_Instance == null) m_Instance = new GameObject("GameController", new Type[] { typeof(GameController) }).GetComponent<GameController>();
			return m_Instance;
		}
	}

	void Awake() {
		if (m_Instance != null) {
			Destroy(gameObject);
			return;
		}

		m_Instance = this;

		if (Debug) {
			SpawnedPawn += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " was spawned at: " + args.Pawn.transform.position.ToString());
			};

			SpawnedItem += (sender, args) => {
				DebugConsole.Log(args.Item.FullName + " was spawned at: " + args.Item.transform.position.ToString());
			};

			Died += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " was killed");
			};

			Damaged += (sender, args) => {
				if (args.Inflicter != null) DebugConsole.Log(args.Inflicted.FullName + " received " + args.Damage.Damage + " " + (args.Damage.Element == DamageElement.NONE ? "" : args.Damage.Element.ToString() + "-") + args.Damage.Type.ToString() + " damage from " + args.Inflicter.FullName);
				else DebugConsole.Log(args.Inflicted.FullName + " received " + args.Damage.Damage + " " + (args.Damage.Element == DamageElement.NONE ? "" : args.Damage.Element.ToString() + "-") + args.Damage.Type.ToString() + " damage.");
			};
			
			UsedItem += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " used " + args.Item.FullName);
			};

			PickedUpItem += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " picked up " + args.Item.FullName);
            };

			DroppedItem += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " dropped " + args.Item.FullName);
			};

			CraftedItem += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " crafted " + args.Item.FullName);
			};

			EquippedWeapon += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " equipped " + args.Weapon.FullName);
			};

			EquippedArmor += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " equipped " + args.Armor.FullName);
			};

			EquippedLantern += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " equipped " + args.Lantern.FullName);
			};

			UnequippedWeapon += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " unequipped " + args.Weapon.FullName);
			};

			UnequippedArmor += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " unequipped " + args.Armor.FullName);
			};

			UnequippedLantern += (sender, args) => {
				DebugConsole.Log(args.Pawn.FullName + " unequipped " + args.Lantern.FullName);
			};
		}
	}

	#endregion
}
