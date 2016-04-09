using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class GameController : MonoBehaviour {

	#region Prefabs

	public GameObject ItemPrefab;
	public GameObject WeaponPrefab;
	public GameObject EquipmentPrefab;
	public GameObject NPCPrefab;
	public GameObject PlayerPrefab;
	public GameObject EntityPrefab;

	#endregion

	public static PlayerController Player
	{
		get
		{
			return PlayerController.Instance;
		}
	}

	public static NPC[] NPCs
	{
		get
		{
			return FindObjectsOfType<NPC>();
		}
	}

	void Update()
	{
		foreach (ModTemplate mod in Mods)
		{
			mod.Update();
		}
	}

	#region Construction and Singleton

	private static GameController m_Instance = null;
	public static GameController Instance
	{
		get
		{
			return m_Instance;
		}
	}

	void Awake()
	{
		if (m_Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		m_Instance = this;
		DontDestroyOnLoad(this);

		LoadPlugins();

		foreach (ModTemplate mod in Mods)
		{
			mod.m_InModInit = true;
			mod.Initialize();
			mod.m_InModInit = false;
			mod.InternalInit();
		}
	}

	#endregion

	#region Plugin Loading

	internal static List<string> m_AssemblyIgnoreList = new List<string>(new string[]
	{
		"Assembly-CSharp.dll",
		"UnityEngine.dll",
		"UnityEngine.UI.dll"
	});

	internal static List<ModTemplate> Mods = new List<ModTemplate>();
	internal static List<PlayerEventTemplate> PlayerEvents = new List<PlayerEventTemplate>();

	internal static void InvokePlayerSpawned()
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.Spawned();
		}
	}

	internal static void InvokePlayerDied(Entity attacker)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.Died(attacker);
		}
	}

	internal static void InvokePlayerDamaged(Damage damage)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.Damaged(damage);
		}
	}

	internal static void InvokePlayerMoved(Vector3 position, Vector3 delta)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.Moved(position, delta);
		}
	}

	internal static void InvokePlayerJumped()
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.Jumped();
		}
	}

	internal static void InvokePlayerLanded()
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.Landed();
		}
	}

	internal static void InvokePlayerUpdate()
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.Update();
		}
	}

	internal static void InvokePlayerFixedUpdate()
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.FixedUpdate();
		}
	}

	internal static void InvokePlayerLateUpdate()
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.LateUpdate();
		}
	}

	/* Items cannot be used anymore.
	internal static void InvokePlayerUsedItem(Item item)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.UsedItem(item);
		}
	}
	*/
	/* Items cannot be added to hotbar.
	internal static void InvokePlayerAddedItemToHotBar(Item item, int slot)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.AddedItemToHotBar(item, slot);
		}
	}
	*/

	internal static void InvokePlayerDroppedItem(Item item)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.DroppedItem(item);
		}
	}

	internal static void InvokePlayerPickedUpItem(Item item)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.PickedUpItem(item);
		}
	}

	internal static void InvokePlayerCraftedItem(Item item)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.CraftedItem(item);
		}
	}

	internal static void InvokePlayerUsedWeapon(Weapon weapon)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.UsedWeapon(weapon);
		}
	}

	internal static void InvokePlayerEquippedWeapon(Weapon weapon, int slot)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.EquippedWeapon(weapon, slot);
		}
	}

	internal static void InvokePlayerUnequippedWeapon(Weapon weapon)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.UnequippedWeapon(weapon);
		}
	}

	internal static void InvokePlayerPickedUpWeapon(Weapon weapon)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.PickedUpWeapon(weapon);
		}
	}

	internal static void InvokePlayerDroppedWeapon(Weapon weapon)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.DroppedWeapon(weapon);
		}
	}

	internal static void InvokePlayerCraftedWeapon(Weapon weapon)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.CraftedWeapon(weapon);
		}
	}

	internal static void InvokePlayerEquippedEquipment(Equipment equipment, int slot)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.EquippedEquipment(equipment, slot);
		}
	}

	internal static void InvokePlayerUnequippedEquipment(Equipment equipment)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.UnequippedEquipment(equipment);
		}
	}

	internal static void InvokePlayerPickedUpEquipment(Equipment equipment)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.PickedUpEquipment(equipment);
		}
	}

	internal static void InvokePlayerDroppedEquipment(Equipment equipment)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.DroppedEquipment(equipment);
		}
	}

	internal static void InvokePlayerCraftedEquipment(Equipment equipment)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.CraftedEquipment(equipment);
		}
	}

	internal static void InvokePlayerUsedAbility(AbilityTemplate ability)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.UsedAbility(ability);
		}
	}

	internal static void InvokePlayerLearnedAbility(AbilityTemplate ability, int level)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.LearnedAbility(ability, level);
		}
	}

	internal static void InvokePlayerDefeatedOther(Entity other)
	{
		foreach (PlayerEventTemplate pet in PlayerEvents)
		{
			pet.DefeatedOther(other);
		}
	}

	internal static void LoadPlugins()
	{
		if (!Directory.Exists(@"plugins\"))
		{
			Directory.CreateDirectory(@"plugins\");
			Debug.Log("No plugins to load. Finished.");
			return;
		}

		DirectoryInfo di = new DirectoryInfo(@"plugins\");

		FileInfo[] files = di.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

		if (files.Length == 0)
		{
			Debug.Log("No plugins to load. Finished.");
			return;
		}

		Debug.Log("Loading plugins found in directory: " + di.FullName);

		foreach (FileInfo file in files)
		{
			Debug.Log("Processing possible plugin file: " + file.Name);
			if (m_AssemblyIgnoreList.Contains(file.Name))
			{
				Debug.Log("File name found within the assembly ignore list. Continuing to next assembly.");
				continue;
			}

			Assembly plugin = Assembly.LoadFrom(file.FullName);

			Debug.Log("Successfuly loaded plugin's assembly, looking for inherited type for player events.");

			Type[] modTypes = GetAllTypesWithInheritingType(plugin, typeof(ModTemplate));
			if (modTypes.Length != 1)
			{
				Debug.LogError("Could not load mod because there is either no class that inherits from ModTemplate or there are more than one classes that inherit from ModTemplate.");
			}
			Mods.Add((ModTemplate)Activator.CreateInstance(modTypes[0]));

			Type[] eventTypes = GetAllTypesWithInheritingType(plugin, typeof(PlayerEventTemplate));
			foreach (Type t in eventTypes)
			{
				PlayerEvents.Add((PlayerEventTemplate)Activator.CreateInstance(t));
			}

			Debug.Log("Successfuly loaded Player Event types.");
		}
	}

	internal static Type[] GetAllTypesWithInheritingType(Assembly assembly, Type inheritingType)
	{
		List<Type> res = new List<Type>();

		foreach (Type t in assembly.GetTypes())
		{
			if (t.BaseType == inheritingType)
			{
				res.Add(t);
			}
		}

		return res.ToArray();
	}

	#endregion
}
