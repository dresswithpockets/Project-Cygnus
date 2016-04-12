using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public abstract class ModTemplate {

	#region Template Code

	public abstract string ModName
	{
		get;
	}

	public abstract string ModAuthor
	{
		get;
	}

	public abstract Version ModVersion
	{
		get;
	}

	public virtual void Initialize()
	{
	}

	public virtual void Update()
	{
	}

	#endregion

	internal string ModPath
	{
		get
		{
			return "plugins/" + ModName;
		}
	}

	internal Dictionary<string, AudioClip> LoadedAudoClips = new Dictionary<string, AudioClip>();

	internal Dictionary<string, Type> ItemTemplates = new Dictionary<string, Type>();
	internal Dictionary<string, Type> WeaponTemplates = new Dictionary<string, Type>();
	internal Dictionary<string, Type> EquipmentTemplates = new Dictionary<string, Type>();
	internal SortedDictionary<string, Ability_Type> BasicAbilities = new SortedDictionary<string, Ability_Type>();
	internal SortedDictionary<string, Ability_Type> IntermediateAbilities = new SortedDictionary<string, Ability_Type>();
	internal SortedDictionary<string, Ability_Type> AdvancedAbilities = new SortedDictionary<string, Ability_Type>();
	internal Dictionary<string, Type> NPCTemplates = new Dictionary<string, Type>();

	internal Dictionary<string, MeshFilter> LoadedModels = new Dictionary<string, MeshFilter>();

	internal bool m_InModInit = true;

	internal void InternalInit()
	{
		BasicAbilities.OrderBy(x => x.Value.min_player_level).ToDictionary(pair => pair.Key, pair => pair.Value);
		IntermediateAbilities.OrderBy(x => x.Value.min_player_level).ToDictionary(pair => pair.Key, pair => pair.Value);
		AdvancedAbilities.OrderBy(x => x.Value.min_player_level).ToDictionary(pair => pair.Key, pair => pair.Value);
	}

	public void SpawnEntity(string entity, Vector3 position)
	{
		SpawnEntity(entity, position, Vector3.zero);
	}

	public void SpawnEntity(string entity, Vector3 position, Vector3 rotation)
	{
		string entityPrefix = entity.Split('.')[0].ToLower();
		string entityID = entity.Split('.')[1];
		string modelID = "";
		switch (entityPrefix)
		{
			case "item":
				GameObject item = (GameObject)GameObject.Instantiate(GameController.Instance.ItemPrefab, position, Quaternion.Euler(rotation));

				Item_Template templateItem = (Item_Template)Activator.CreateInstance(ItemTemplates[entityID]);
				modelID = (templateItem.model_ID == null ? "Cygnus.default" : templateItem.model_ID);

				item.GetComponent<Item>().AssignTemplate(templateItem, LoadedModels[modelID]);

				break;
			case "weapon":
				GameObject weapon = (GameObject)GameObject.Instantiate(GameController.Instance.WeaponPrefab, position, Quaternion.Euler(rotation));

				Weapon_Template templateWeapon = (Weapon_Template)Activator.CreateInstance(WeaponTemplates[entityID]);
				modelID = (templateWeapon.ModelID == null ? "Cygnus.default" : templateWeapon.ModelID);

				weapon.GetComponent<Weapon>().AssignTemplate(templateWeapon, LoadedModels[modelID]);

				break;
			case "equipment":
				GameObject equipment = (GameObject)GameObject.Instantiate(GameController.Instance.EquipmentPrefab, position, Quaternion.Euler(rotation));

				Equipment_Template templateEquipment = (Equipment_Template)Activator.CreateInstance(EquipmentTemplates[entityID]);
				modelID = (templateEquipment.model_ID == null ? "Cygnus.default" : templateEquipment.model_ID);

				equipment.GetComponent<Equipment>().AssignTemplate(templateEquipment, LoadedModels[modelID]);

				break;
			case "npc":
				Debug.Log("NPC Spawning has not been implemented.");
				//TODO: Spawn NPC
				break;
		}
	}

	public void LoadModel(string ID, string name)
	{
		if (!m_InModInit)
		{
			Debug.LogError("Models can only be loaded inside Initialize().");
			return;
		}
		MeshFilter m = new MeshFilter();

		string[] vox = File.ReadAllLines(ModPath + "/Models/" + name + ".vox");

		List<Vector3> verts = new List<Vector3>();
		List<Color> cols = new List<Color>();
		List<int> tris = new List<int>();
		bool inVerts = false;
		bool inCols = false;
		bool inTris = false;
		for (int i = 0; i < vox.Length; i++)
		{
			string line = vox[i].Trim();
			if (line == "[verts]")
			{
				inVerts = true;
				inCols = false;
				inTris = false;
			}
			else if (line == "[cols]")
			{
				inVerts = false;
				inCols = true;
				inTris = false;
			}
			else if (line == "[tris]")
			{
				inVerts = false;
				inCols = false;
				inTris = true;
			}
			else
			{
				if (inVerts)
				{
					string[] axes = line.Split(' ');
					verts.Add(new Vector3(float.Parse(axes[0]), float.Parse(axes[1]), float.Parse(axes[3])));
				}
				else if (inCols)
				{
					cols.Add(HexToColor(line));
				}
				else if (inTris)
				{
					tris.Add(int.Parse(line));
				}
			}
		}
		m.mesh.vertices = verts.ToArray();
		m.mesh.colors = cols.ToArray();
		m.mesh.triangles = tris.ToArray();

		m.mesh.RecalculateNormals();

		LoadedModels.Add(ID, m);
	}

	public void LoadSound(string ID, string name)
	{
		if (!m_InModInit)
		{
			Debug.LogError("Sounds can only be loaded inside Initialize().");
			return;
		}

		if (LoadedAudoClips.ContainsKey(ID))
		{
			Debug.LogError("Cannot import audio file " + name +
				" because the ID provided \"" + ID +
				"\" has already been assigned to another audio file.");
			return;
		}

		string path = ModPath + "/Sounds/" + name + ".wav";

		Debug.Log("Importing WAV: " + path + " with ID: " + ID);

		WAV wav = new WAV(path);

		Debug.Log("Imported WAV: " + wav.ToString());

		AudioClip clip = AudioClip.Create(name, wav.SampleCount, wav.ChannelCount, wav.Frequency, false);

		float[] wavData = new float[wav.LeftChannel.Length + wav.RightChannel.Length];
		for (int i = 0; i < wavData.Length; i++)
		{
			if (i % 2 == 0)
			{
				wavData[i] = wav.LeftChannel[i / 2];
				continue;
			}

			wavData[i] = wav.RightChannel[Mathf.CeilToInt(i / 2.0f)];
		}

		clip.SetData(wavData, 0);

		LoadedAudoClips.Add(ID, clip);
	}

	public void RegisterTemplate(string ID, Type template)
	{
		if (!m_InModInit)
		{
			Debug.LogError("Templates can only be registered inside Initialize().");
			return;
		}
		if (typeof(Item_Template).IsAssignableFrom(template))
		{
			if (ItemTemplates.ContainsKey(ID))
			{
				Debug.LogError("Cannot register item with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			ItemTemplates.Add(ID, template);
		}
		else if (typeof(Weapon_Template).IsAssignableFrom(template))
		{
			if (WeaponTemplates.ContainsKey(ID))
			{
				Debug.LogError("Cannot register weapon with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			WeaponTemplates.Add(ID, template);
		}
		else if (typeof(Equipment_Template).IsAssignableFrom(template))
		{
			if (EquipmentTemplates.ContainsKey(ID))
			{
				Debug.LogError("Cannot register equipment with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			EquipmentTemplates.Add(ID, template);
		}
		else if (typeof(Ability_Template).IsAssignableFrom(template))
		{
			Debug.LogError("Abilities are no longer registered using RegisterTemplate. Register using RegisterAbility.");
		}
		else if (typeof(NPCTemplate).IsAssignableFrom(template))
		{
			if (NPCTemplates.ContainsKey(ID))
			{
				Debug.LogError("Cannot register npc with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			NPCTemplates.Add(ID, template);
		}
		else
		{
			Debug.LogError("Cannot import template of type " + template.Name +
				" because it does not inherit one of the abstract templates in CygnusAPI.");
			return;
		}
	}

	public void RegisterAbility(string ID, Type ability, Ability_Tier tier, int minimumPlayerLevel)
	{
		switch (tier)
		{
			case Ability_Tier.ADVANCED:
				if (AdvancedAbilities.ContainsKey(ID))
				{
					Debug.LogError("Cannot register ability with ID: " + ID +
						" because there is already another advanced ability registered with that ID.");
					return;
				}

				AdvancedAbilities.Add(ID, new Ability_Type(ability, tier, minimumPlayerLevel));
				break;
			case Ability_Tier.BASIC:
				if (BasicAbilities.ContainsKey(ID))
				{
					Debug.LogError("Cannot register ability with ID: " + ID +
						" because there is already another basic ability registered with that ID.");
					return;
				}

				BasicAbilities.Add(ID, new Ability_Type(ability, tier, minimumPlayerLevel));
				break;
			case Ability_Tier.INTERMEDIATE:
				if (IntermediateAbilities.ContainsKey(ID))
				{
					Debug.LogError("Cannot register ability with ID: " + ID +
						" because there is already another intermediate ability registered with that ID.");
					return;
				}

				IntermediateAbilities.Add(ID, new Ability_Type(ability, tier, minimumPlayerLevel));
				break;
		}
	}

	public PlayerController GetPlayer()
	{
		return GameController.Player;
	}

	public NPC[] GetNPCs()
	{
		return GameController.NPCs;
	}

	public GameController GetGame()
	{
		return GameController.Instance;
	}

	public static string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
		return hex;
	}

	public static Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		if (hex.Length > 6)
		{
			return new Color32(r, g, b, byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber));
		}
		return new Color32(r, g, b, 255);
	}
}
