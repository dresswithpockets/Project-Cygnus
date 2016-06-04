using UnityEngine;

/// <summary>
/// The base class for any controllable entity in Cygnus.
/// Players, NetworkPlayers and NPCs all derive from Pawn.
/// </summary>
public class Pawn : MonoBehaviour {
	
	public string Name {
		get {
			if (IsPlayer) return "Player";
			return name;
		}
	}

	public PlayerController Player {
		get {
			return (IsPlayer)
				? (PlayerController)this
				: null;
		}
	}

	public bool IsPlayer {
		get {
			return this.GetType() != typeof(PlayerController);
		}
	}
}
