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

	internal Dictionary<string, AudioClip> audio_clip_dict = new Dictionary<string, AudioClip>();

	internal Dictionary<string, Type> item_template_dict = new Dictionary<string, Type>();
	internal Dictionary<string, Type> weapon_template_dict = new Dictionary<string, Type>();
	internal Dictionary<string, Type> armor_template_dict = new Dictionary<string, Type>();
	internal Dictionary<string, Type> consumable_template_dict = new Dictionary<string, Type>();
	internal Dictionary<string, Type> pet_item_template_dict = new Dictionary<string, Type>();
	internal SortedDictionary<string, Ability_Type> basic_ability_dict = new SortedDictionary<string, Ability_Type>();
	internal SortedDictionary<string, Ability_Type> inter_ability_dict = new SortedDictionary<string, Ability_Type>();
	internal SortedDictionary<string, Ability_Type> advanced_ability_dict = new SortedDictionary<string, Ability_Type>();
	internal Dictionary<string, Type> NPC_template_dict = new Dictionary<string, Type>();

	internal Dictionary<string, Vox_Data> model_dict = new Dictionary<string, Vox_Data>();

	internal Dictionary<string, Texture2D> image_dict = new Dictionary<string, Texture2D>();

	internal bool inside_mod_initer = true;

	internal void internal_init() {
		
		basic_ability_dict.OrderBy(x => x.Value.min_player_level).ToDictionary(pair => pair.Key, pair => pair.Value);
		inter_ability_dict.OrderBy(x => x.Value.min_player_level).ToDictionary(pair => pair.Key, pair => pair.Value);
		advanced_ability_dict.OrderBy(x => x.Value.min_player_level).ToDictionary(pair => pair.Key, pair => pair.Value);
	}

	public object spawn_ent(string ent, Vector3 pos) {

		return spawn_ent(ent, pos, Vector3.zero);
	}

	public object spawn_ent(string ent, Vector3 pos, Vector3 rot) {

		object spawned_ent = null;

		string ent_prefix = ent.Split('.')[0].ToLower();
		string ent_ID = ent.Split('.')[1];
		string model_ID = "";
		Vox_Data vox = new Vox_Data();
		switch (ent_prefix) {

			case "material":

				GameObject item = (GameObject)GameObject.Instantiate(Game_Controller.instance.material_prefab, pos, Quaternion.Euler(rot));

				Material_Template template_item = (Material_Template)Activator.CreateInstance(item_template_dict[ent_ID]);

				model_ID = (template_item.model_ID == null ? "default" : template_item.model_ID);

				if (model_ID == "default" || !model_dict.ContainsKey(model_ID)) {
					//filter = null;
				}
				else {
					//filter = model_dict[model_ID];
					vox = model_dict[model_ID];
				}

				item.GetComponent<Material>().assign_template(template_item, vox);

				return template_item;
			case "weapon":

				GameObject weapon = (GameObject)GameObject.Instantiate(Game_Controller.instance.weapon_prefab, pos, Quaternion.Euler(rot));

				Weapon_Template template_weapon = (Weapon_Template)Activator.CreateInstance(weapon_template_dict[ent_ID]);

				model_ID = (template_weapon.model_ID == null ? "default" : template_weapon.model_ID);

				if (model_ID == "default" || !model_dict.ContainsKey(model_ID)) {
					//vox = null;
				}
				else {
					vox = model_dict[model_ID];
				}

				weapon.GetComponent<Weapon>().assign_template(template_weapon, vox);

				return template_weapon;
			case "armor":

				GameObject armor = (GameObject)GameObject.Instantiate(Game_Controller.instance.armor_prefab, pos, Quaternion.Euler(rot));

				Armor_Template template_armor = (Armor_Template)Activator.CreateInstance(armor_template_dict[ent_ID]);

				model_ID = (template_armor.model_ID == null ? "default" : template_armor.model_ID);

				if (model_ID == "default" || !model_dict.ContainsKey(model_ID)) {
					//vox = null;
				}
				else {
					vox = model_dict[model_ID];
				}

				armor.GetComponent<Armor>().assign_template(template_armor, vox);

				return template_armor;
			case "consumable":

				GameObject consumable = (GameObject)GameObject.Instantiate(Game_Controller.instance.consumable_prefab, pos, Quaternion.Euler(rot));

				Consumable_Template template_consumable = (Consumable_Template)Activator.CreateInstance(consumable_template_dict[ent_ID]);

				model_ID = (template_consumable.model_ID == null ? "default" : template_consumable.model_ID);

				if (model_ID == "default" || !model_dict.ContainsKey(model_ID)) {
					//vox = null;
				}
				else {
					vox = model_dict[model_ID];
				}

				consumable.GetComponent<Consumable>().assign_template(template_consumable, vox);

				return template_consumable;
			case "petitem":

				GameObject pet_item = (GameObject)GameObject.Instantiate(Game_Controller.instance.pet_item_prefab, pos, Quaternion.Euler(rot));

				Pet_Item_Template template_pet_item = (Pet_Item_Template)Activator.CreateInstance(pet_item_template_dict[ent_ID]);

				model_ID = (template_pet_item.model_ID == null ? "default" : template_pet_item.model_ID);
				
				if (model_ID == "default" || !model_dict.ContainsKey(model_ID)) {
					//vox = null;
				}
				else {
					vox = model_dict[model_ID];
				}

				pet_item.GetComponent<Pet_Item>().assign_template(template_pet_item, vox);

				return template_pet_item;
			case "pet":

				GameObject pet = (GameObject)GameObject.Instantiate(Game_Controller.instance.pet_item_prefab, pos, Quaternion.Euler(rot));

				Pet_Item_Template template_pet = (Pet_Item_Template)Activator.CreateInstance(pet_item_template_dict[ent_ID]);

				model_ID = (template_pet.model_ID == null ? "default" : template_pet.model_ID);

				if (model_ID == "default" || !model_dict.ContainsKey(model_ID)) {
					//vox = null;
				}
				else {
					vox = model_dict[model_ID];
				}

				pet.GetComponent<Pet_Item>().assign_template(template_pet, vox);
				pet.GetComponent<Pet_Item>().is_item = false;

				return template_pet;
			case "npc":

				Debug.Log("NPC Spawning has not been implemented.");
				// TODO: Create GameObject instantiation for NPC prefabs
				// and assign NPC_Template to that instantiated game object.

				break;
		}

		return spawned_ent;
	}

	public void load_model(string ID, string file_name) {

		if (!inside_mod_initer) {

			Debug.LogError("Models can only be loaded inside Initialize().");
			return;
		}

		Debug.Log("Loading model with ID: " + ID + ", from filename: " + file_name);

		model_dict.Add(ID, new Vox_Data(ID, File.ReadAllLines(mod_path + "/Models/" + file_name + ".vox")));

		Debug.Log("Finished loading model.");
	}

	public void load_image(string ID, string file_name, Image_Type image_type) {

		if (image_dict.ContainsKey(ID)) {
			Debug.LogError("Cannot import image file " + file_name +
				" because the ID provided \"" + ID +
				"\" has already been assigned to another image file.");
			return;
		}

		Texture2D image = new Texture2D(2, 2);
		byte[] data = File.ReadAllBytes(mod_path + "/Images/" + file_name + "." + Enum.GetName(typeof(Image_Type), image_type));
		image.LoadImage(data);

		image_dict.Add(ID, image);
	}

	public void load_sound(string ID, string file_name) {

		if (!inside_mod_initer) {

			Debug.LogError("Sounds can only be loaded inside Initialize().");
			return;
		}

		if (audio_clip_dict.ContainsKey(ID)) {

			Debug.LogError("Cannot import audio file " + file_name +
				" because the ID provided \"" + ID +
				"\" has already been assigned to another audio file.");
			return;
		}

		string path = mod_path + "/Sounds/" + file_name + ".wav";

		Debug.Log("Importing WAV: " + path + " with ID: " + ID);

		WAV wav = new WAV(path);

		Debug.Log("Imported WAV: " + wav.ToString());

		AudioClip clip = AudioClip.Create(file_name, wav.SampleCount, wav.ChannelCount, wav.Frequency, false);

		float[] wav_data = new float[wav.LeftChannel.Length + wav.RightChannel.Length];
		for (int i = 0; i < wav_data.Length; i++) {

			if (i % 2 == 0) {
				wav_data[i] = wav.LeftChannel[i / 2];
				continue;
			}

			wav_data[i] = wav.RightChannel[Mathf.CeilToInt(i / 2.0f)];
		}

		clip.SetData(wav_data, 0);

		audio_clip_dict.Add(ID, clip);
	}

	public void register_item(string ID, Type item_type) {

		if (!inside_mod_initer) {

			Debug.LogError("Templates can only be registered inside Initialize().");
			return;
		}
		if (typeof(Material_Template).IsAssignableFrom(item_type)) {

			if (item_template_dict.ContainsKey(ID)) {

				Debug.LogError("Cannot register item with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			item_template_dict.Add(ID, item_type);
		}
		else if (typeof(Weapon_Template).IsAssignableFrom(item_type)) {

			if (weapon_template_dict.ContainsKey(ID)) {

				Debug.LogError("Cannot register weapon with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			weapon_template_dict.Add(ID, item_type);
		}
		else if (typeof(Armor_Template).IsAssignableFrom(item_type)) {

			if (armor_template_dict.ContainsKey(ID)) {

				Debug.LogError("Cannot register armor with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			armor_template_dict.Add(ID, item_type);
		}
		else if (typeof(Consumable_Template).IsAssignableFrom(item_type)) {

			if (consumable_template_dict.ContainsKey(ID)) {

				Debug.LogError("Cannot register consumable with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			consumable_template_dict.Add(ID, item_type);
		}
		else if (typeof(Pet_Item_Template).IsAssignableFrom(item_type)) {

			if (pet_item_template_dict.ContainsKey(ID)) {

				Debug.LogError("Cannot register pet item with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			pet_item_template_dict.Add(ID, item_type);
		}
		else if (typeof(Ability_Template).IsAssignableFrom(item_type)) Debug.LogError("Abilities are no longer registered using register_item. Register using register_ability.");
		else if (typeof(NPC_Template).IsAssignableFrom(item_type)) Debug.LogError("NPCs are no longer registered using register_item. REgister using register_npc.");
		else Debug.LogError("Cannot import template of type " + item_type.Name + " because it does not inherit one of the abstract item templates in CygnusAPI.");
	}

	public void register_npc(string ID, Type npc_type) {
		if (typeof(NPC_Template).IsAssignableFrom(npc_type)) {

			if (NPC_template_dict.ContainsKey(ID)) {

				Debug.LogError("Cannot register npc with ID: " + ID +
					" because there is already another item registered with that ID.");
				return;
			}

			NPC_template_dict.Add(ID, npc_type);
			return;
		}
		Debug.LogError("Cannot import template of type " + npc_type.Name + " because it does not inherit from the abstract npc template in CygnusAPI");
	}

	public void register_ability(string ID, Type ability_type, Ability_Tier tier, int min_player_level) {

		if (advanced_ability_dict.ContainsKey(ID) ||
			inter_ability_dict.ContainsKey(ID) ||
			basic_ability_dict.ContainsKey(ID)) {

			Debug.LogError("Cannot register ability with ID: " + ID +
						" because there is already another ability registered with that ID.");
			return;
		}

		Debug.Log("Registering ability with ID: " + ID + ", of tier: " + Enum.GetName(typeof(Ability_Tier), tier) + ", with mind level of: " + min_player_level.ToString());

		switch (tier) {

			case Ability_Tier.ADVANCED:

				Debug.Log("Registering ability to advanced ability dictionary...");
				advanced_ability_dict.Add(ID, new Ability_Type(ability_type, tier, min_player_level));
				Debug.Log("Registered ability to advanced ability dictionary.");

				return;
			case Ability_Tier.BASIC:

				Debug.Log("Registering ability to basic ability dictionary...");
				basic_ability_dict.Add(ID, new Ability_Type(ability_type, tier, min_player_level));
				Debug.Log("Registered ability to basic ability dictionary.");

				return;
			case Ability_Tier.INTERMEDIATE:

				Debug.Log("Registering ability to intermediate ability dictionary...");
				inter_ability_dict.Add(ID, new Ability_Type(ability_type, tier, min_player_level));
				Debug.Log("Registered ability to intermediate ability dictionary.");

				return;
		}

		Debug.LogError("Couldn't register ability for an unknown reason.");
	}

	public Ability_Type get_ability(string ability_ID) {
		if (advanced_ability_dict.ContainsKey(ability_ID)) return advanced_ability_dict[ability_ID];
		else if (inter_ability_dict.ContainsKey(ability_ID)) return inter_ability_dict[ability_ID];
		else if (basic_ability_dict.ContainsKey(ability_ID)) return basic_ability_dict[ability_ID];
		else return new Ability_Type(null, Ability_Tier.BASIC, -1);
	}

	public Player_Controller get_player() { return Player_Controller.instance; }

	public NPC_Controller[] get_NPC_list() { return Game_Controller.instance.NPC_list; }

	public Game_Controller get_game() { return Game_Controller.instance; }
}
