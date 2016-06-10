using UnityEngine;

/// <summary>
/// The base class for any controllable entity in Cygnus.
/// Players, NetworkPlayers and NPCs all derive from Pawn.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public abstract class Pawn : MonoBehaviour {
	
	private bool m_Alive = false;

	public bool Alive {
		get {
			return m_Alive;
		}
		internal set {
			if (value != m_Alive) {
				m_Alive = value;
				if (GetType().Equals(typeof(PlayerController)))
					(this as PlayerController).AllowMovement = m_Alive;
			}
		}
	}

	public bool AliveLastFrame { get; private set; }

	/// <summary>
	/// Returns a BaseType-lead name of the pawn. Example: [Pawn:LocalPlayer] or [Pawn:BossCaveTroll]
	/// </summary>
	public string FullName {
		get {
			/*
			Old: return "[Pawn:" + GetType().Name + "]";
			*/
			return "[" +
				(GetType().BaseType.Equals(typeof(MonoBehaviour)) ? GetType().Name : GetType().BaseType.Name) +
				":" + Name + "]";
		}
	}

	public PlayerController Player {
		get {
			return GetComponent<PlayerController>();
		}
	}

	public bool IsPlayer {
		get {
			return this.GetType().Equals(typeof(PlayerController));
		}
	}
	
	public abstract string Name { get; }

	public virtual void Update() {

		AliveLastFrame = Alive;
	}

	public virtual void Damage(DamageInfo damage) {
		GameController.InvokeDamaged(this, damage.Source, this, damage);
	}
}
