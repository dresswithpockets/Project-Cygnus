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
		LoadModel("ShortSword", "ShortSword");

		SpawnItem(typeof(ShortSword), new Vector3(0f, 2f, 0f), Quaternion.identity, "ShortSword");
	}

	public override void Update() {
		
	}
}
