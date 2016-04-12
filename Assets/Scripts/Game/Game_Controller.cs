using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Game_Controller : MonoBehaviour {

	#region Prefabs

	public GameObject item_prefab;
	public GameObject weapon_prefab;
	public GameObject equipment_prefab;
	public GameObject NPC_prefab;
	public GameObject player_prefab;
	public GameObject ent_prefab;

	#endregion
	
	internal static List<Mod_Template> mod_list = new List<Mod_Template>();
	internal static List<Player_Event_Template> player_event_list = new List<Player_Event_Template>();

	public static Player_Controller player {

		get {

			return Player_Controller.instance;
		}
	}

	public static NPC[] NPC_list {

		get {

			return FindObjectsOfType<NPC>();
		}
	}

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

	internal static void invoke_player_dropped(Item item) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.dropped(item);
		}
	}

	internal static void invoke_player_picked_up(Item item) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.picked_up(item);
		}
	}

	internal static void invoke_player_crafted(Item item) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.crafted(item);
		}
	}

	internal static void invoke_player_used(Weapon weapon) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.used(weapon);
		}
	}

	internal static void invoke_player_equipped(Weapon weapon, int slot) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.equipped(weapon, slot);
		}
	}

	internal static void invoke_player_unequipped(Weapon weapon) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.unequipped(weapon);
		}
	}

	internal static void invoke_player_picked_up(Weapon weapon) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.picked_up(weapon);
		}
	}

	internal static void invoke_player_dropped(Weapon weapon) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.dropped(weapon);
		}
	}

	internal static void invoke_player_crafted(Weapon weapon) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.crafted(weapon);
		}
	}

	internal static void invoke_player_equipped(Equipment equipment, int slot) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.equipped(equipment, slot);
		}
	}

	internal static void invoke_player_unequipped(Equipment equipment) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.unequipped(equipment);
		}
	}

	internal static void invoke_player_picked_up(Equipment equipment) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.picked_up(equipment);
		}
	}

	internal static void invoke_player_dropped(Equipment equipment) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.dropped(equipment);
		}
	}

	internal static void invoke_player_crafted(Equipment equipment) {
		foreach (Player_Event_Template pet in player_event_list) {
			pet.crafted(equipment);
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
			Debug.Log("No plugins to load. Finished.");
			return;
		}

		DirectoryInfo di = new DirectoryInfo(@"plugins\");

		FileInfo[] files = di.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

		if (files.Length == 0) {

			Debug.Log("No plugins to load. Finished.");
			return;
		}

		Debug.Log("Loading plugins found in directory: " + di.FullName);

		foreach (FileInfo file in files) {

			Debug.Log("Processing possible plugin file: " + file.Name);
			if (m_dll_ignore_list.Contains(file.Name)) {

				Debug.Log("File name found within the assembly ignore list. Continuing to next assembly.");
				continue;
			}

			Assembly plugin = Assembly.LoadFrom(file.FullName);

			Debug.Log("Successfuly loaded plugin's assembly, looking for inherited type for player events.");

			Type[] modTypes = plugin.get_all_types_with_inheriting_type(typeof(Mod_Template));

			if (modTypes.Length != 1) {

				Debug.LogError("Could not load mod because there is either no class that inherits from ModTemplate or there are more than one classes that inherit from ModTemplate.");
			}

			mod_list.Add((Mod_Template)Activator.CreateInstance(modTypes[0]));

			Type[] eventTypes = plugin.get_all_types_with_inheriting_type(typeof(Player_Event_Template));

			foreach (Type t in eventTypes) {

				player_event_list.Add((Player_Event_Template)Activator.CreateInstance(t));
			}

			Debug.Log("Successfuly loaded Player Event types.");
		}
	}


	#endregion
}
