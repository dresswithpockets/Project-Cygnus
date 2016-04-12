using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public abstract class Mod_Template {

	#region Template Code

	public abstract string mod_name { get; }

	public abstract string mod_author { get; }

	public abstract Version mod_version { get; }

	public virtual void initialize() { }

	public virtual void update() { }

	#endregion

	internal string mod_path {

		get {

			return "plugins/" + mod_name;
		}
	}

	internal Dictionary<string, AudioClip> audio_clips = new Dictionary<string, AudioClip>();

	internal Dictionary<string, Type> item_templates = new Dictionary<string, Type>();
	internal Dictionary<string, Type> weapon_templates = new Dictionary<string, Type>();
	internal Dictionary<string, Type> equipment_templates = new Dictionary<string, Type>();
	internal Dictionary<string, Type> consumable_templates = new Dictionary<string, Type>();
	internal Dictionary<string, Type> pet_item_templates = new Dictionary<string, Type>();
	internal SortedDictionary<string, Ability_Type> basic_abilities = new SortedDictionary<string, Ability_Type>();
	internal SortedDictionary<string, Ability_Type> inter_abilities = new SortedDictionary<string, Ability_Type>();
	internal SortedDictionary<string, Ability_Type> advanced_abilities = new SortedDictionary<string, Ability_Type>();
	internal Dictionary<string, Type> NPC_templates = new Dictionary<string, Type>();

	internal Dictionary<string, MeshFilter> models = new Dictionary<string, MeshFilter>();

	internal bool inside_mod_initer = true;

	internal void internal_init() {
		
		basic_abilities.OrderBy(x => x.Value.min_player_level).ToDictionary(pair => pair.Key, pair => pair.Value);
		inter_abilities.OrderBy(x => x.Value.min_player_level).ToDictionary(pair => pair.Key, pair => pair.Value);
		advanced_abilities.OrderBy(x => x.Value.min_player_level).ToDictionary(pair => pair.Key, pair => pair.Value);
	}

	public void spawn_ent(string entity, Vector3 position) {

		spawn_ent(entity, position, Vector3.zero);
	}

	public void spawn_ent(string ent, Vector3 pos, Vector3 rot) {

		string ent_prefix = ent.Split('.')[0].ToLower();
		string ent_ID = ent.Split('.')[1];
		string model_ID = "";
		switch (ent_prefix) {

			case "material":

				GameObject item = (GameObject)GameObject.Instantiate(Game_Controller.instance.material_prefab, pos, Quaternion.Euler(rot));

				Material_Template template_item = (Material_Template)Activator.CreateInstance(item_templates[ent_ID]);

				model_ID = (template_item.model_ID == null ? "default" : template_item.model_ID);

				item.GetComponent<Material>().assign_template(template_item, models[model_ID]);

				break;
			case "weapon":

				GameObject weapon = (GameObject)GameObject.Instantiate(Game_Controller.instance.weapon_prefab, pos, Quaternion.Euler(rot));

				Weapon_Template template_weapon = (Weapon_Template)Activator.CreateInstance(weapon_templates[ent_ID]);

				model_ID = (template_weapon.model_ID == null ? "default" : template_weapon.model_ID);

				weapon.GetComponent<Weapon>().AssignTemplate(template_weapon, models[model_ID]);

				break;
			case "equipment":

				GameObject equipment = (GameObject)GameObject.Instantiate(Game_Controller.instance.equipment_prefab, pos, Quaternion.Euler(rot));

				Equipment_Template template_equipment = (Equipment_Template)Activator.CreateInstance(equipment_templates[ent_ID]);

				model_ID = (template_equipment.model_ID == null ? "default" : template_equipment.model_ID);

				equipment.GetComponent<Equipment>().assign_template(template_equipment, models[model_ID]);

				break;
			case "consumable":

				GameObject consumable = (GameObject)GameObject.Instantiate(Game_Controller.instance.consumable_prefab, pos, Quaternion.Euler(rot));

				Consumable_Template template_consumable = (Consumable_Template)Activator.CreateInstance(consumable_templates[ent_ID]);

				model_ID = (template_consumable.model_ID == null ? "default" : template_consumable.model_ID);

				consumable.GetComponent<Consumable>().assign_template(template_consumable, models[model_ID]);

				break;
			case "petitem":

				GameObject pet_item = (GameObject)GameObject.Instantiate(Game_Controller.instance.pet_item_prefab, pos, Quaternion.Euler(rot));

				Pet_Item_Template template_pet_item = (Pet_Item_Template)Activator.CreateInstance(pet_item_templates[ent_ID]);

				model_ID = (template_pet_item.model_ID == null ? "default" : template_pet_item.model_ID);

				pet_item.GetComponent<Pet_Item>().assign_template(template_pet_item, models[model_ID]);

				break;
			case "pet":

				GameObject pet = (GameObject)GameObject.Instantiate(Game_Controller.instance.pet_item_prefab, pos, Quaternion.Euler(rot));

				Pet_Item_Template template_pet = (Pet_Item_Template)Activator.CreateInstance(pet_item_templates[ent_ID]);

				model_ID = (template_pet.model_ID == null ? "default" : template_pet.model_ID);

				pet.GetComponent<Pet_Item>().assign_template(template_pet, models[model_ID]);
				pet.GetComponent<Pet_Item>().is_item = false;

				break;
			case "npc":

				Debug.Log("NPC Spawning has not been implemented.");
				// TODO: Create GameObject instantiation for NPC prefabs
				// and assign NPC_Template to that instantiated game object.

				break;
		}
	}

	public void load_model(string ID, string name) {

		if (!inside_mod_initer) {

			Debug.LogError("Models can only be loaded inside Initialize().");
			return;
		}

		MeshFilter filter = new MeshFilter();

		string[] vox = File.ReadAllLines(mod_path + "/Models/" + name + ".vox");

		List<Vector3> verts = new List<Vector3>();
		List<Color> cols = new List<Color>();
		List<int> tris = new List<int>();
		Model_Section model_section = Model_Section.VERTS;

		for (int i = 0; i < vox.Length; i++) {

			string line = vox[i].Trim();

			if (line == "[verts]") model_section = Model_Section.VERTS;
			else if (line == "[cols]") model_section = Model_Section.COLS;
			else if (line == "[tris]") model_section = Model_Section.TRIS;
			else {

				switch (model_section) {

					case Model_Section.VERTS:

						string[] axes = line.Split(' ');
						verts.Add(new Vector3(float.Parse(axes[0]), float.Parse(axes[1]), float.Parse(axes[3])));

						break;
					case Model_Section.COLS:

						cols.Add(line.hex_to_color());

						break;
					case Model_Section.TRIS:

						tris.Add(int.Parse(line));

						break;
				}
			}
		}

		filter.mesh.vertices = verts.ToArray();
		filter.mesh.colors = cols.ToArray();
		filter.mesh.triangles = tris.ToArray();

		filter.mesh.RecalculateNormals();

		models.Add(ID, filter);
	}

	public void load_sound(string ID, string name) {

		if (!inside_mod_initer) {

			Debug.LogError("Sounds can only be loaded inside Initialize().");
			return;
		}

		if (audio_clips.ContainsKey(ID)) {

			Debug.LogError("Cannot import audio file " + name +
				" because the ID provided \"" + ID +
				"\" has already been assigned to another audio file.");
			return;
		}

		string path = mod_path + "/Sounds/" + name + ".wav";

		Debug.Log("Importing WAV: " + path + " with ID: " + ID);

		WAV wav = new WAV(path);

		Debug.Log("Imported WAV: " + wav.ToString());

		AudioClip clip = AudioClip.Create(name, wav.SampleCount, wav.ChannelCount, wav.Frequency, false);

		float[] wav_data = new float[wav.LeftChannel.Length + wav.RightChannel.Length];
		for (int i = 0; i < wav_data.Length; i++) {

			if (i % 2 == 0) {
				wav_data[i] = wav.LeftChannel[i / 2];
				continue;
			}

			wav_data[i] = wav.RightChannel[Mathf.CeilToInt(i / 2.0f)];
		}

		clip.SetData(wav_data, 0);

		audio_clips.Add(ID, clip);
	}

	public void register_template(string ID, Type template) {

		if (!inside_mod_initer) {

			Debug.LogError("Templates can only be registered inside Initialize().");
			return;
		}
		if (typeof(Material_Template).IsAssignableFrom(template)) {

			if (item_templates.ContainsKey(ID)) {

				Debug.LogError("Cannot register item with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			item_templates.Add(ID, template);
		}
		else if (typeof(Weapon_Template).IsAssignableFrom(template)) {

			if (weapon_templates.ContainsKey(ID)) {

				Debug.LogError("Cannot register weapon with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			weapon_templates.Add(ID, template);
		}
		else if (typeof(Equipment_Template).IsAssignableFrom(template)) {

			if (equipment_templates.ContainsKey(ID)) {

				Debug.LogError("Cannot register equipment with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			equipment_templates.Add(ID, template);
		}
		else if (typeof(Consumable_Template).IsAssignableFrom(template)) {

			if (consumable_templates.ContainsKey(ID)) {

				Debug.LogError("Cannot register consumable with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			consumable_templates.Add(ID, template);
		}
		else if (typeof(Pet_Item_Template).IsAssignableFrom(template)) {

			if (pet_item_templates.ContainsKey(ID)) {

				Debug.LogError("Cannot register pet item with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			pet_item_templates.Add(ID, template);
		}
		else if (typeof(Ability_Template).IsAssignableFrom(template)) Debug.LogError("Abilities are no longer registered using RegisterTemplate. Register using RegisterAbility.");
		else if (typeof(NPC_Template).IsAssignableFrom(template)) {

			if (NPC_templates.ContainsKey(ID)) {

				Debug.LogError("Cannot register npc with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			NPC_templates.Add(ID, template);
		}
		else Debug.LogError("Cannot import template of type " + template.Name + " because it does not inherit one of the abstract templates in CygnusAPI.");
	}

	public void register_ability(string ID, Type ability, Ability_Tier tier, int min_player_level) {

		switch (tier) {

			case Ability_Tier.ADVANCED:
				if (advanced_abilities.ContainsKey(ID)) {

					Debug.LogError("Cannot register ability with ID: " + ID +
						" because there is already another advanced ability registered with that ID.");
					return;
				}

				advanced_abilities.Add(ID, new Ability_Type(ability, tier, min_player_level));
				break;
			case Ability_Tier.BASIC:
				if (basic_abilities.ContainsKey(ID)) {

					Debug.LogError("Cannot register ability with ID: " + ID +
						" because there is already another basic ability registered with that ID.");
					return;
				}

				basic_abilities.Add(ID, new Ability_Type(ability, tier, min_player_level));
				break;
			case Ability_Tier.INTERMEDIATE:
				if (inter_abilities.ContainsKey(ID)) {

					Debug.LogError("Cannot register ability with ID: " + ID +
						" because there is already another intermediate ability registered with that ID.");
					return;
				}

				inter_abilities.Add(ID, new Ability_Type(ability, tier, min_player_level));
				break;
		}
	}

	public Player_Controller get_player() { return Game_Controller.player; }

	public NPC[] get_NPC_list() { return Game_Controller.NPC_list; }

	public Game_Controller get_game() { return Game_Controller.instance; }
}
