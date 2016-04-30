using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public sealed class Game_Controller : MonoBehaviour {

	#region Prefabs

	public GameObject material_prefab;
	public GameObject weapon_prefab;
	public GameObject armor_prefab;
	public GameObject NPC_prefab;
	public GameObject player_prefab;
	public GameObject consumable_prefab;
	public GameObject pet_item_prefab;
	public GameObject ent_prefab;
	public GameObject vox_prefab;

	#endregion
	
	internal static List<Mod_Template> mod_list = new List<Mod_Template>();
	internal static List<Player_Event_Template> player_event_list = new List<Player_Event_Template>();

	private bool m_game_paused = false;

	public Player_Controller player {

		get {

			return Player_Controller.instance;
		}
	}

	public NPC_Controller[] NPC_list {

		get {

			return FindObjectsOfType<NPC_Controller>();
		}
	}
	
	public bool game_paused {
		get {
			return m_game_paused;
		}
		internal set {
			m_game_paused = value;
			player.fp_input.AllowGameplayInput = !game_paused;
			//vp_Utility.LockCursor = !value;
		}
	}

	#region static instance properties

	public static Player_Controller player_controller {
		get {
			return instance.player;
		}
	}

	public static NPC_Controller[] NPC_controller_list {
		get {
			return instance.NPC_list;
		}
	}

	public static bool paused {
		get {
			return instance.game_paused;
		}
	}

	#endregion

	#region static instance functions

	public static void toggle_pause() {
		instance.game_paused = !instance.game_paused;
	}

	public static void pause() {
		instance.game_paused = true;
	}

	public static void unpause() {
		instance.game_paused = false;
	}

	#endregion

	void Update() {

		foreach (Mod_Template mod in mod_list) {

			mod.update();
		}
	}

	#region Construction and Singleton

	private static Game_Controller m_instance = null;
	public static Game_Controller instance {
		get {
			return m_instance;
		}
	}

	void Awake() {

		if (m_instance != null) {

			Destroy(gameObject);
			return;
		}

		m_instance = this;
		DontDestroyOnLoad(this);

		load_plugins();

		foreach (Mod_Template mod in mod_list) {

			mod.inside_mod_initer = true;
			mod.initialize();
			mod.inside_mod_initer = false;
			mod.internal_init();
		}
	}

	#endregion

	#region Plugin Loading

	internal static List<string> m_dll_ignore_list = new List<string>(new string[]
	{
		"Assembly-CSharp.dll",
		"UnityEngine.dll",
		"UnityEngine.UI.dll"
	});

	internal static void invoke_player_spawned() {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.spawned();
		}
	}

	internal static void invoke_player_died(Entity attacker) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.died(attacker);
		}
	}

	internal static void invoke_player_damaged(Damage damage) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.damaged(damage);
		}
	}

	internal static void invoke_player_moved(Vector3 position, Vector3 delta) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.moved(position, delta);
		}
	}

	internal static void invoke_player_jumped() {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.jumped();
		}
	}

	internal static void invoke_player_landed() {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.landed();
		}
	}

	internal static void invoke_player_update() {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.update();
		}
	}

	internal static void invoke_player_fixed_update() {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.fixed_update();
		}
	}

	internal static void invoke_player_late_update() {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.late_update();
		}
	}

	internal static void invoke_player_dropped(Item_Template item) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.dropped(item);
		}
	}

	internal static void invoke_player_picked_up(Item_Template item) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.picked_up(item);
		}
	}

	internal static void invoke_player_crafted(Item_Template item) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.crafted(item);
		}
	}

	internal static void invoke_player_used(Item_Template weapon) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.used(weapon);
		}
	}

	internal static void invoke_player_equipped(Item_Template weapon, int slot) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.equipped(weapon, slot);
		}
	}

	internal static void invoke_player_unequipped(Item_Template weapon) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.unequipped(weapon);
		}
	}

	internal static void invoke_player_used(Ability_Template ability) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.used(ability);
		}
	}

	internal static void invoke_player_learned(Ability_Template ability, int level) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.learned(ability, level);
		}
	}

	internal static void invoke_player_defeated_other(Entity other) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.defeated_other(other);
		}
	}

	internal static void load_plugins() {
		if (!Directory.Exists(@"plugins\")) {

			Directory.CreateDirectory(@"plugins\");
			DebugConsole.Log("No plugins to load. Finished.", true);
			return;
		}

		DirectoryInfo di = new DirectoryInfo(@"plugins\");

		FileInfo[] files = di.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

		if (files.Length == 0) {

			DebugConsole.Log("No plugins to load. Finished.", true);
			return;
		}

		DebugConsole.Log("Loading plugins found in directory: " + di.FullName, true);

		foreach (FileInfo file in files) {

			DebugConsole.Log("Processing possible plugin file: " + file.Name, true);
			if (m_dll_ignore_list.Contains(file.Name)) {

				DebugConsole.Log("File name found within the assembly ignore list. Continuing to next assembly.", true);
				continue;
			}

			Assembly plugin = Assembly.LoadFrom(file.FullName);

			DebugConsole.Log("Successfuly loaded plugin's assembly, looking for inherited type for player events.", true);

			Type[] modTypes = plugin.get_all_types_with_inheriting_type(typeof(Mod_Template));

			if (modTypes.Length != 1) {

				Debug.LogError("Could not load mod because there is either no class that inherits from ModTemplate or there are more than one classes that inherit from ModTemplate.");
			}

			mod_list.Add((Mod_Template)Activator.CreateInstance(modTypes[0]));

			Type[] eventTypes = plugin.get_all_types_with_inheriting_type(typeof(Player_Event_Template));

			foreach (Type t in eventTypes) player_event_list.Add((Player_Event_Template)Activator.CreateInstance(t));

			DebugConsole.Log("Successfuly loaded Player Event types.", true);
		}
	}

	#endregion
}
