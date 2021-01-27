using System;
using UnityEngine;

public sealed class BasePlugin : Plugin {
	public override string Name {
		get {
			return "Base";
		}
	}

	public override string Author {
		get {
			return "Tristen Horton";
		}
	}

	public override Version Version {
		get {
			return Util.AssemblyVersion;
		}
	}

	public override void Initialize() {
		LoadModel("ShortSword", "shortsword");
		LoadModel("Recipe", "recipe");

		LoadModel("Projectile", "projectile");

		LoadModel("PlayerFox", "player_fox");
		LoadModel("PlayerRain", "player_rain");

		SpawnPlayer("PlayerRain");

		SpawnItem(typeof(ShortSword), new Vector3(0f, 2f, 0f), Quaternion.identity);

		Projectile proj = (Projectile)SpawnPawn(typeof(Projectile), new Vector3(-16f, 1f, 17f), Quaternion.identity);
		proj.SetTrajectory(new Vector3(1f, 0f, 0f), 10f);
	}

	public override void Update() {
		
	}
}
