using UnityEngine;
using System;
using System.Collections;

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
	public readonly AItem Item;

	public PawnItemEventArgs(Pawn pawn, AItem item) {
		Pawn = pawn;
	}
}

public class GameController : MonoBehaviour {
	
	public bool Paused = false;
	public bool Debug = false;

	public static bool GamePaused {
		get {
			return Instance.Paused;
		}
	}

	#region Events

	public event EventHandler<PawnEventArgs> Spawned;
	public event EventHandler<PawnEventArgs> Landed;
	public event EventHandler<PawnEventArgs> Jumped;
	public event EventHandler<PawnDamagedEventArgs> Damaged;
	public event EventHandler<PawnMovedEventArgs> Moved;
	public event EventHandler<PawnEventArgs> Update;
	public event EventHandler<PawnEventArgs> FixedUpdate;
	public event EventHandler<PawnEventArgs> LateUpdate;
	public event EventHandler<PawnItemEventArgs> UsedItem;
	public event EventHandler<PawnItemEventArgs> DroppedItem;
	public event EventHandler<PawnItemEventArgs> PickedUpItem;
	public event EventHandler<PawnItemEventArgs> CraftedItem;
	public event Action<Pawn, AWeapon> EquippedWeapon;
	public event Action<Pawn, AArmor> EquippedArmor;
	public event Action<Pawn, ALantern> EquippedLantern;
	public event Action<Pawn, AWeapon> UnequippedWeapon;
	public event Action<Pawn, AArmor> UnequippedArmor;
	public event Action<Pawn, ALantern> UnequippedLantern;
	public event Action<Pawn, AAbility> UsedAbility;
	public event Action<Pawn, ASkill> TriggeredSkill;
	public event Action<Pawn, Pawn> DefeatedOther;

	public static void InvokeSpawned(object sender, Pawn pawn) {
		if (Instance.Spawned != null) Instance.Spawned.Invoke(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeLanded(object sender, Pawn pawn) {
		if (Instance.Landed != null) Instance.Landed.Invoke(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeJumped(object sender, Pawn pawn) {
		if (Instance.Jumped != null) Instance.Jumped.Invoke(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeDamaged(object sender, Pawn inflicter, Pawn inflicted, DamageInfo damage) {
		if (Instance.Damaged != null) Instance.Damaged.Invoke(sender, new PawnDamagedEventArgs(inflicter, inflicted, damage));
	}

	public static void InvokeMoved(object sender, Pawn pawn, Vector3 newPos, Vector3 delta) {
		if (Instance.Moved != null) Instance.Moved.Invoke(sender, new PawnMovedEventArgs(pawn, newPos, delta));
	}

	public static void InvokeUpdate(object sender, Pawn pawn) {
		if (Instance.Update != null) Instance.Update.Invoke(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeFixedUpdate(object sender, Pawn pawn) {
		if (Instance.FixedUpdate != null) Instance.FixedUpdate.Invoke(sender, new PawnEventArgs(pawn));
	}

	public static void InvokeLateUpdate(object sender, Pawn pawn) {
		if (Instance.LateUpdate != null) Instance.LateUpdate.Invoke(sender, new PawnEventArgs(pawn));
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
			Spawned += (sender, args) => {
				DebugConsole.Log("Pawn." + args.Pawn.Name + " was spawned at: " + args.Pawn.transform.position.ToString());
			};

			UsedItem += (sender, args) => {
				DebugConsole.Log("Pawn." + args.Pawn.Name + " used " + args.Item.GetType().Name + "." + args.Item.Name);
			};
		}
	}

	#endregion
}
