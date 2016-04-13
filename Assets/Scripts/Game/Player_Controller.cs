using UnityEngine;
using System.Collections.Generic;

// TODO: Implement the player's ability to craft items, armor, etc.
public sealed class Player_Controller : MonoBehaviour {

	#region Stats

	private bool m_is_alive = false;
	private float m_hp = 0f;

	private float m_armor = 0f;
	private float m_magic_resi = 0f; // Resi stat
	private float m_crit_chance = 0f; // Crit stat
	private float m_max_hp = 100f; // HP Stat
	private float m_max_hp_default = 100f; // hp stat by default (at lowest hp level)
	private float m_hp_regen = 0f; // Reg stat
	private float m_attack_tempo = 1.0f; // Tempo stat
	
	private float m_hp_level = 0;

	public float hp {

		get {

			return m_hp;
		}
		internal set {

			m_hp = value;
			if (m_hp < 0f || Mathf.Approximately(m_hp, 0f)) {

				m_is_alive = false;
				m_hp = 0f;
			}
			else {

				m_is_alive = true;
			}
		}
	}

	public bool is_alive {

		get {

			return m_is_alive;
		}
		internal set {

			m_is_alive = value;
		}
	}

	public float armor {

		get {

			return m_armor;
		}
		internal set {

			m_armor = value;
		}
	}

	public float magic_resi {

		get {

			return m_magic_resi;
		}
		internal set {

			m_magic_resi = value;
		}
	}

	public float crit_chance {

		get {

			return m_crit_chance;
		}
		internal set {

			m_crit_chance = value;
		}
	}

	public float max_hp {

		get {

			return m_max_hp;
		}
		internal set {

			m_max_hp = value;
		}
	}

	public float default_max_hp {

		get {
			return m_max_hp_default;
		}
		internal set {
			m_max_hp_default = value;
		}
	}

	public float hp_level {
		get {
			return m_hp_level;
		}
		internal set {
			m_hp_level = value;
		}
	}

	public float hp_regen {

		get {

			return m_hp_regen;
		}
		internal set {

			m_hp_regen = value;
		}
	}

	public float attack_tempo {

		get {

			return m_attack_tempo;
		}
		internal set {

			m_attack_tempo = value;
		}
	}

	public int player_level {

		get {

			return inventory.learned_ability_list.Count + 1;
		}
	}

	#endregion

	public GameObject m_orbit_camera = null;
	private vp_FPController m_fp_controller = null;
	private Inventory_Controller m_inv;

	internal Vector3 m_home_pos = new Vector3(0f, 0f, 0f);

	public GameObject orbit_camera {

		get {

			return m_orbit_camera;
		}
		internal set {

			m_orbit_camera = value;
		}
	}

	public Vector3 home_pos {

		get {

			return m_home_pos;
		}
		internal set {

			m_home_pos = value;
		}
	}

	public vp_FPController fp_controller {

		get {

			return m_fp_controller;
		}
		internal set {

			m_fp_controller = value;
		}
	}

	public Inventory_Controller inventory {

		get {

			return m_inv;
		}
		internal set {

			m_inv = value;
		}
	}

	public Vector3 player_velocity {

		get {

			return fp_controller.m_Velocity;
		}
	}

	public bool is_moving {

		get {

			return player_velocity != Vector3.zero;
		}
	}

	void Start() {

		orbit_camera = GetComponentInChildren<UltimateOrbitCamera>().gameObject;
		fp_controller = GetComponent<vp_FPController>();
		inventory = GetComponent<Inventory_Controller>();

		inventory.is_player_inventory = true;
		inventory.player = this;

		spawn(transform.position);
	}

	#region Update

	void Update() {
		if (is_alive) {
			update_input();

			update_movement();

			update_rotation();
		}

		Game_Controller.invoke_player_update();
	}

	void update_input() {

		if (vp_Input.GetButtonDown("Attack1")) {
			if (inventory.active_weapon_list[0] != null) {
				inventory.active_weapon_list[0].template.primary_used(this);
			}

		}
		else if (vp_Input.GetButtonDown("Attack2")) {
			if (inventory.active_weapon_list[0] != null) {
				inventory.active_weapon_list[0].template.alternate_used(this);
			}

		}
		else if (vp_Input.GetButtonDown("Ability1")) {
			if (inventory.active_ability_list[0] != null) {
				inventory.active_ability_list[0].active_update(this);
			}

		}
		else if (vp_Input.GetButtonDown("Ability2")) {
			if (inventory.active_ability_list[1] != null) {
				inventory.active_ability_list[1].active_update(this);
			}

		}
		else if (vp_Input.GetButtonDown("Ability3")) {
			if (inventory.active_ability_list[2] != null) {
				inventory.active_ability_list[2].active_update(this);
			}

		}
		else if (vp_Input.GetButtonDown("Ability4")) {
			if (inventory.active_ability_list[3] != null) {
				inventory.active_ability_list[3].active_update(this);
			}

		}
	}

	void update_movement() {

		if (is_moving) {

			Game_Controller.invoke_player_moved(transform.position, player_velocity * Time.deltaTime);
		}
	}

	void update_rotation() {

		if (is_moving || inventory.is_casting_ability) // TODO: Or attacking w/ weapon (mouse 1 or mouse 2 by default)
		{
			Vector3 old_cam_pos = orbit_camera.transform.position;
			Quaternion old_cam_rot = orbit_camera.transform.rotation;

			Vector3 new_euler_rot = new Vector3(0f, 0f, 0f);
			new_euler_rot.y = orbit_camera.transform.eulerAngles.y;
			transform.eulerAngles = new_euler_rot;

			//Fix rotation of camera as it is a child.
			m_orbit_camera.transform.position = old_cam_pos;
			m_orbit_camera.transform.rotation = old_cam_rot;
		}
	}

	#endregion

	void FixedUpdate() {

		Game_Controller.invoke_player_fixed_update();
	}

	void LateUpdate() {

		Game_Controller.invoke_player_late_update();
	}

	public void spawn(Vector3 pos) {

		transform.position = pos;
		hp = max_hp;
		Game_Controller.invoke_player_spawned();
	}

	public void spawn(bool atStatue = false) {

		if (atStatue) {
			Vector3 closestStatue = home_pos;
			float closestDistance = Mathf.Infinity;

			Statue[] statues = FindObjectsOfType<Statue>();
			foreach (Statue statue in statues) {
				Vector3 directionToStatue = statue.transform.position - transform.position;
				float sqrDistance = directionToStatue.sqrMagnitude;
				if (sqrDistance < closestDistance) {
					closestDistance = sqrDistance;
					closestStatue = statue.transform.position;
				}
			}

			spawn(closestStatue);
			return;
		}

		spawn(home_pos);
	}

	public void set_home(Vector3 position) {

		// TODO: Do checks to make sure position is a valid position in world space.
		home_pos = position;
	}

	public void do_damage(Damage damage) {

		float dmgMult = 1f;

		switch (damage.type) {

			case Damage_Type.MAGIC:

				if (magic_resi > 0f || Mathf.Approximately(magic_resi, 0f)) {
					dmgMult = 100 / (100 + magic_resi);
				}
				else {
					dmgMult = 2f - (100 / (100 - magic_resi));
				}

				break;
			case Damage_Type.PHYSICAL:

				if (armor > 0f || Mathf.Approximately(armor, 0f)) {
					dmgMult = 100 / (100 + armor);
				}
				else {
					dmgMult = 2f - (100 / (100 - armor));
				}

				break;
		}

		hp -= damage.damage * dmgMult;
		Game_Controller.invoke_player_damaged(damage);
		if (!is_alive) {

			Game_Controller.invoke_player_died(damage.attacker);
		}
	}

	public void kill(Entity killer) {
		do_damage(new Damage(hp + 1f, killer, Damage_Type.PURE));// +1 ensures that the damage done is more than the health the player has.
	}

	public float get_stat(Char_Stat stat) {

		switch (stat) {
			case Char_Stat.ARMOR: return armor;
			case Char_Stat.CRIT: return crit_chance;
			case Char_Stat.HP: return max_hp;
			case Char_Stat.REG: return hp_regen;
			case Char_Stat.RESI: return magic_resi;
			case Char_Stat.TEMPO: return attack_tempo;
		}

		return 0f;
	}

	public void update_stats() {

		// TODO: determine a more realistic interval for increasing hp.
		hp = (max_hp = (hp_level * 20) + default_max_hp);
		
		foreach (Armor armor in inventory.active_armor_list) {
			if (armor != null) {
				foreach (Stat_Modifier stat_mod in armor.template.stat_mods) {
					process_stat_mod(stat_mod);
				}
			}
		}
		foreach (Weapon weapon in inventory.active_weapon_list) {
			if (weapon != null ) {
				foreach (Stat_Modifier stat_mod in weapon.template.stat_mods) {
					process_stat_mod(stat_mod);
				}
			}
		}
	}

	internal void process_stat_mod(Stat_Modifier stat_mod) {
		switch (stat_mod.stat) {
			case Char_Stat.ARMOR:
				armor += stat_mod.amount;
				break;
			case Char_Stat.CRIT:
				crit_chance += stat_mod.amount;
				break;
			case Char_Stat.HP:
				hp += stat_mod.amount;
				break;
			case Char_Stat.REG:
				hp_regen += stat_mod.amount;
				break;
			case Char_Stat.RESI:
				magic_resi += stat_mod.amount;
				break;
			case Char_Stat.TEMPO:
				attack_tempo += stat_mod.amount;
				break;
		}
	}

	public void give_ability(string ability_ID, Mod_Template mod) {

		Debug.Log("$$Checking inventory nullity...");

		if (inventory == null) {
			Debug.LogError("Uh oh, inventory is null?!");
			return;
		}

		Debug.Log("$$Checking get_ability...");

		if (mod.get_ability(ability_ID).min_player_level == -1) {
			Debug.LogError("Uh oh, get_ability is returning an empty ability type?!");
		}

		Debug.Log("$$Learning the ability...");

		inventory.learn_ability(ability_ID, mod.get_ability(ability_ID));
	}

	public void give_weapon(string weapon_ID, Mod_Template mod) {
		Weapon_Template weapon = (Weapon_Template)mod.spawn_ent("weapon." + weapon_ID, transform.position);
		inventory.pickup(weapon.game_object.GetComponent<Weapon>());
	}

	public void give_armor(string armor_ID, Mod_Template mod) {
		Armor_Template armor = (Armor_Template)mod.spawn_ent("armor." + armor_ID, transform.position);
		inventory.pickup(armor.game_object.GetComponent<Armor>());
	}

	public void give_material(string material_ID, Mod_Template mod) {
		Material_Template material = (Material_Template)mod.spawn_ent("material." + material_ID, transform.position);
		inventory.pickup(material.game_object.GetComponent<Material>());
	}

	public void give_consumable(string consumable_ID, Mod_Template mod) {
		Consumable_Template consumable = (Consumable_Template)mod.spawn_ent("consumable." + consumable_ID, transform.position);
		inventory.pickup(consumable.game_object.GetComponent<Consumable>());
	}

	public void give_pet_item(string pet_item_ID, Mod_Template mod) {
		Pet_Item_Template pet_item = (Pet_Item_Template)mod.spawn_ent("pet_item." + pet_item_ID, transform.position);
		inventory.pickup(pet_item.game_object.GetComponent<Pet_Item>());
	}

	#region Construction and Singleton

	private static Player_Controller m_instance = null;
	public static Player_Controller instance {

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
	}

	#endregion
}
