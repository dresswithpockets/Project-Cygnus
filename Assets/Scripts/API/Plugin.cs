using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PicaVoxel;

public abstract class Plugin {

	#region Template Code

	public string FullName {
		get {
			return "[Plugin:" + Name + "]";
		}
	}

	public abstract string Name { get; }

	public abstract string Author { get; }

	public abstract Version Version { get; }

	public virtual void Initialize() { }

	public virtual void Update() { }

	#endregion

	internal string Path {

		get {

			return "plugins/" + Name;
		}
	}

	private bool insideModInit { get; set; }
	
	public Dictionary<string, byte[]> ModelRegister { get; private set; }

	public static List<Recipe> RecipeRegister { get; private set; }

	internal void InternalInit() {

		ModelRegister = new Dictionary<string, byte[]>();

		insideModInit = true;

		// Register defaults
		LoadModel("Default", "Default");

		Initialize();
		insideModInit = false;
	}

	public static void RegisterPlugin<T>() where T : Plugin, new() {
		T plugin = new T();
		plugin.InternalInit();
		GameController.Instance.PluginInstances.Add(plugin);
	}

	public Item SpawnItem(Type itemType, string model = "Default") {
		return SpawnItem(itemType, Vector3.zero, Quaternion.identity, model);
	}

	public Item SpawnItem(Type itemType, Vector3 position, Quaternion rotation, string model = "Default") {
		if (typeof(Item).IsAssignableFrom(itemType)) { // Is an item, spawn that shit
			GameObject go = (GameObject)GameObject.Instantiate(GameController.Instance.VoxPrefab, position, rotation);

			if (ModelRegister.ContainsKey(model)) {
				using (BinaryReader reader = new BinaryReader(new MemoryStream(ModelRegister[model]))) {
					VoxelUtil.FromMagica(reader, go.GetComponentInChildren<PicaVoxel.Volume>().gameObject, 0.075f, false);
				}
			}
			Item item = (Item)go.AddComponent(itemType);

			item.gameObject.name = item.Name;

			Volume vol = item.GetComponentInChildren<Volume>();
			vol.Pivot = (new Vector3(vol.XSize, vol.YSize, vol.ZSize) * vol.VoxelSize) / 2f;
			vol.UpdatePivot();

			GameController.InvokeSpawned(this, item);

			return item;
		}

		return null;
	}

	public Pawn SpawnPawn(Type pawnType, string model = "Default") {
		return SpawnPawn(pawnType, Vector3.zero, Quaternion.identity, model);
	}

	public Pawn SpawnPawn(Type pawnType, Vector3 position, Quaternion rotation, string model = "Default") {
		if (typeof(PlayerController).IsAssignableFrom(pawnType)) {
			DebugConsole.Log(FullName + " cannot spawn PlayerController via SpawnPawn; use SpawnPlayer instead.", DebugWarningLevel.ERROR);
		}
		if (typeof(Pawn).IsAssignableFrom(pawnType)) { // Is a non-player pawn, spawn that shit
			GameObject go = (GameObject)GameObject.Instantiate(GameController.Instance.VoxPrefab, position, rotation);

			if (ModelRegister.ContainsKey(model)) {
				using (BinaryReader reader = new BinaryReader(new MemoryStream(ModelRegister[model]))) {
					VoxelUtil.FromMagica(reader, go.GetComponentInChildren<PicaVoxel.Volume>().gameObject, 0.075f, false);
				}
			}
			//if (ModelRegister.ContainsKey(model)) go.GetComponent<ModelController>().Data = ModelRegister[model];
			Pawn pawn = (Pawn)go.AddComponent(pawnType);

			pawn.gameObject.name = pawn.Name;

			Volume vol = pawn.GetComponentInChildren<Volume>();
			vol.Pivot = (new Vector3(vol.XSize, vol.YSize, vol.ZSize) * vol.VoxelSize) / 2f;
			vol.UpdatePivot();

			GameController.InvokeSpawned(this, pawn);

			return pawn;
		}

		return null;
	}

	public Pawn SpawnPlayer(Vector3 position, string model = "Default") {

		PlayerController pc = GameController.FindObjectOfType<PlayerController>();

		if (pc != null) pc.Spawn(position);
		else {
			// TODO: Spawn new player
		}

		// TODO: Implement player models

		return pc;
	}

	public void LoadModel(string id, string fileName) {

		if (!insideModInit) {

			DebugConsole.Log(FullName + " models can only be loaded inside Initialize().", DebugWarningLevel.ERROR);
			return;
		}
		
		ModelRegister.Add(id, File.ReadAllBytes(Path + "/models/" + fileName + ".vox"));

		DebugConsole.Log(FullName + " loaded model " + id + " from " + fileName + ".vox");
	}

	public void LoadSound(string id, string fileName) {
		// TODO: Load sound files
	}

	public void LoadSprite(string id, string filename) {
		// TODO: Load sprites
	}

	public void AddRecipe(Recipe recipe) {

		if (!insideModInit) {

			DebugConsole.Log(FullName + " recipes can only be added inside Initialize().", DebugWarningLevel.ERROR);
			return;
		}

		RecipeRegister.Add(recipe);
	}
}
