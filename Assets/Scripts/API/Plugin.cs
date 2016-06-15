using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PicaVoxel;

public enum PivotCase {

	/// <summary>
	/// Explicit clause for if the Pivot in a RenderData is not to be used.
	/// </summary>
	NONE = 0,

	/// <summary>
	/// Use the Pivot provided by a RenderData object.
	/// </summary>
    USE_PIVOT = 1
}

public struct RenderData {
	public readonly string ModelID;
	public readonly PivotCase Case;
	public readonly Vector3 Pivot;

	public RenderData(string modelID, PivotCase pCase, Vector3 pivot) {
		ModelID = modelID;
		Case = pCase;
		Pivot = pivot;
	}
}

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

	public static Dictionary<int, Recipe> RecipeRegister { get; private set; }

	internal void InternalInit() {

		ModelRegister = new Dictionary<string, byte[]>();
		RecipeRegister = new Dictionary<int, Recipe>();

		insideModInit = true;

		// Register defaults
		LoadModel("Default", "default");

		Initialize();
		insideModInit = false;
	}

	public static void RegisterPlugin<T>() where T : Plugin, new() {
		T plugin = new T();
		plugin.InternalInit();
		GameController.Instance.PluginInstances.Add(plugin);
	}

	public Item SpawnItem(Type itemType) {
		return SpawnItem(itemType, Vector3.zero, Quaternion.identity);
	}

	public Item SpawnItem(Type itemType, string model) {
		return SpawnItem(itemType, Vector3.zero, Quaternion.identity, model);
	}

	public Item SpawnItem(Type itemType, Vector3 position, Quaternion rotation) {
		if (typeof(Item).IsAssignableFrom(itemType)) { // Is an item, spawn that shit
			GameObject go = (GameObject)GameObject.Instantiate(GameController.Instance.VoxPrefab, position, rotation);

			Item item = (Item)go.AddComponent(itemType);

			item.gameObject.name = item.Name;

			string useModel = (ModelRegister.ContainsKey(item.DefaultModel) ? item.DefaultModel : "Default");
			using (BinaryReader reader = new BinaryReader(new MemoryStream(ModelRegister[useModel]))) {
				VoxelUtil.FromMagica(reader, go.GetComponentInChildren<Volume>().gameObject, 0.075f, false);
			}

			Volume vol = item.GetComponentInChildren<Volume>();
			vol.Pivot = (new Vector3(vol.XSize, vol.YSize, vol.ZSize) * vol.VoxelSize) / 2f;
			vol.UpdatePivot();

			GameController.InvokeSpawned(this, item);

			return item;
		}

		return null;
	}

	public Item SpawnItem(Type itemType, Vector3 position, Quaternion rotation, string model) {
		if (typeof(Item).IsAssignableFrom(itemType)) { // Is an item, spawn that shit
			GameObject go = (GameObject)GameObject.Instantiate(GameController.Instance.VoxPrefab, position, rotation);

			Item item = (Item)go.AddComponent(itemType);

			item.gameObject.name = item.Name;

			string useModel = (ModelRegister.ContainsKey(model) ? model : (ModelRegister.ContainsKey(item.DefaultModel) ? item.DefaultModel : "Default"));
			using (BinaryReader reader = new BinaryReader(new MemoryStream(ModelRegister[useModel]))) {
				VoxelUtil.FromMagica(reader, go.GetComponentInChildren<Volume>().gameObject, 0.075f, false);
			}

			Volume vol = item.GetComponentInChildren<Volume>();
			vol.Pivot = (new Vector3(vol.XSize, vol.YSize, vol.ZSize) * vol.VoxelSize) / 2f;
			vol.UpdatePivot();

			GameController.InvokeSpawned(this, item);

			return item;
		}

		return null;
	}

	public Pawn SpawnPawn(Type pawnType) {
		return SpawnPawn(pawnType, Vector3.zero, Quaternion.identity);
	}

	public Pawn SpawnPawn(Type pawnType, string model) {
		return SpawnPawn(pawnType, Vector3.zero, Quaternion.identity, model);
	}

	public Pawn SpawnPawn(Type pawnType, Vector3 position, Quaternion rotation) {
		if (typeof(PlayerController).IsAssignableFrom(pawnType)) {
			DebugConsole.Log(FullName + " cannot spawn PlayerController via SpawnPawn; use SpawnPlayer instead.", DebugWarningLevel.ERROR);
		}
		else if (typeof(Pawn).IsAssignableFrom(pawnType)) { // Is a non-player pawn, spawn that shit
			GameObject go = (GameObject)GameObject.Instantiate(GameController.Instance.VoxPrefab, position, rotation);
			Pawn pawn = (Pawn)go.AddComponent(pawnType);

			pawn.gameObject.name = pawn.Name;

			string useModel = (ModelRegister.ContainsKey(pawn.DefaultModel) ? pawn.DefaultModel : "Default");
			using (BinaryReader reader = new BinaryReader(new MemoryStream(ModelRegister[pawn.DefaultModel]))) {
				VoxelUtil.FromMagica(reader, go.GetComponentInChildren<Volume>().gameObject, 0.075f, false);
			}

			Volume vol = pawn.GetComponentInChildren<Volume>();
			vol.Pivot = (new Vector3(vol.XSize, vol.YSize, vol.ZSize) * vol.VoxelSize) / 2f;
			vol.UpdatePivot();

			GameController.InvokeSpawned(this, pawn);

			return pawn;
		}

		return null;
	}

	public Pawn SpawnPawn(Type pawnType, Vector3 position, Quaternion rotation, string model) {
		if (typeof(PlayerController).IsAssignableFrom(pawnType)) {
			Log("cannot spawn PlayerController via SpawnPawn; use SpawnPlayer instead.", DebugWarningLevel.ERROR);
		}
		else if (typeof(Pawn).IsAssignableFrom(pawnType)) { // Is a non-player pawn, spawn that shit
			GameObject go = (GameObject)GameObject.Instantiate(GameController.Instance.VoxPrefab, position, rotation);
			Pawn pawn = (Pawn)go.AddComponent(pawnType);

			pawn.gameObject.name = pawn.Name;

			string useModel = (ModelRegister.ContainsKey(model) ? model : (ModelRegister.ContainsKey(pawn.DefaultModel) ? pawn.DefaultModel : "Default"));
			using (BinaryReader reader = new BinaryReader(new MemoryStream(ModelRegister[useModel]))) {
				VoxelUtil.FromMagica(reader, go.GetComponentInChildren<Volume>().gameObject, 0.075f, false);
			}

			Volume vol = pawn.GetComponentInChildren<Volume>();
			vol.Pivot = (new Vector3(vol.XSize, vol.YSize, vol.ZSize) * vol.VoxelSize) / 2f;
			vol.UpdatePivot();

			GameController.InvokeSpawned(this, pawn);

			return pawn;
		}

		return null;
	}

	public Pawn SpawnPlayer() {
		return SpawnPlayer(GameController.Instance.SpawnPoint, Quaternion.identity);
	}

	public Pawn SpawnPlayer(string model) {
		return SpawnPlayer(GameController.Instance.SpawnPoint, Quaternion.identity, model);
	}

	public Pawn SpawnPlayer(Vector3 position, Quaternion rotation) {
		PlayerController pc = GameController.FindObjectOfType<PlayerController>();
		
		if (pc == null) {

			// Instantiate the player

			pc = (GameObject.Instantiate(GameController.Instance.PlayerPrefab, position, rotation) as GameObject).GetComponent<PlayerController>();

			pc.gameObject.name = pc.Name;
		}

		// Setup player model
		string useModel = (ModelRegister.ContainsKey(pc.DefaultModel) ? pc.DefaultModel : "Default");
		using (BinaryReader reader = new BinaryReader(new MemoryStream(ModelRegister[useModel]))) {
			VoxelUtil.FromMagica(reader, pc.GetComponentInChildren<Volume>().gameObject, 0.075f, false);

			Volume vol = pc.GetComponentInChildren<Volume>();
			vol.Pivot = (new Vector3(vol.XSize, vol.YSize, vol.ZSize) * vol.VoxelSize) / 2f;
			vol.UpdatePivot();
		}

		if (pc.HasStarted) pc.Spawn(position);
		else {
			pc.QueuedSpawn = position;
			pc.SpawnAtQueue = true;
		}

		return pc;
	}

	public Pawn SpawnPlayer(Vector3 position, Quaternion rotation, string model) {

		PlayerController pc = GameController.FindObjectOfType<PlayerController>();


		if (pc == null) {

			// Instantiate the player

			GameObject go = (GameObject)GameObject.Instantiate(GameController.Instance.PlayerPrefab, position, rotation);
			pc = go.GetComponent<PlayerController>();

			pc.gameObject.name = pc.Name;
        }

		// Setup player model
		string useModel = (ModelRegister.ContainsKey(model) ? model : (ModelRegister.ContainsKey(pc.DefaultModel) ? pc.DefaultModel : "Default"));
		using (BinaryReader reader = new BinaryReader(new MemoryStream(ModelRegister[useModel]))) {
			VoxelUtil.FromMagica(reader, pc.GetComponentInChildren<Volume>().gameObject, 0.075f, false);

			Volume vol = pc.GetComponentInChildren<Volume>();
			vol.Pivot = (new Vector3(vol.XSize, vol.YSize, vol.ZSize) * vol.VoxelSize) / 2f;
			vol.UpdatePivot();
		}

		if (pc.HasStarted) pc.Spawn(position);
		else {
			pc.QueuedSpawn = position;
			pc.SpawnAtQueue = true;
		}

		return pc;
	}

	public void LoadModel(string id, string fileName) {
		
		if (!insideModInit) {

			Log("models can only be loaded inside Initialize().", DebugWarningLevel.ERROR);
			return;
		}
		if (string.IsNullOrEmpty(id)) {
			Log("a model ID cannot be blank!", DebugWarningLevel.ERROR);
			return;
		}

		ModelRegister.Add(id, File.ReadAllBytes(Path + "/models/" + fileName + ".vox"));

		Log("loaded model " + id + " from " + fileName + ".vox");
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

		RecipeRegister.Add(recipe.ID, recipe);
	}

	public void Log(string message, DebugWarningLevel level = DebugWarningLevel.NORMAL) {
		DebugConsole.Log(FullName + " " + message, level);
	}
}
