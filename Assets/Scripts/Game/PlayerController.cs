using UnityEngine;
using System.Collections.Generic;

public class PlayerController : Pawn {

	#region Stats
	
	private float m_HP = 0;
	private float m_MP = 0;
	private float m_Stamina = 0;
	private float m_Armor = 0;
	private float m_MagicResistance = 0;
	private float m_StaminaRegenRate = 30; // TODO: default?

	private float m_MaxHealth = 0;
	private float m_MaxMagic = 0;
	private float m_MaxStamina = 0;

	private float m_MaxHealthAdditive = 0f;
	private float m_MaxMagicAdditive = 0f;
	private float m_MaxStaminaAdditive = 0f;

	private int m_MaxHealthLevel = 1;
	private int m_MaxMagicLevel = 1;
	private int m_MaxStaminaLevel = 1;

	private bool m_StaminaLocked = false;
	private float m_TimeSinceStaminaUsed = 0f;

	public float PickupDistance = 7.5f;

	public float HealthLevelMod = 10;
	public float StaminaLevelMod = 2;

	public float MaxHealthDefault = 100;
	public float MaxMagicDefault = 100;
	public float MaxStaminaDefault = 100;

	public float ArmorModConstant = 100f;
	public float ResiModConstant = 100f;
	
	public float StaminaRegenDelay = 1.5f;

	public float HP {
		get {
			return m_HP;
		}
		set {
			m_HP = value;
			if (m_HP < 0f || Mathf.Approximately(m_HP, 0f)) m_HP = 0f;
			else if (m_HP > m_MaxHealth) m_HP = m_MaxHealth;
		}
	}

	public float MP {
		get {
			return m_MP;
		}
		set {
			m_MP = value;
			if (m_MP < 0f || Mathf.Approximately(m_MP, 0f)) m_MP = 0f;
			else if (m_MP > m_MaxMagic) m_MP = m_MaxMagic;
		}
	}

	public float Stamina {
		get {
			return m_Stamina;
		}
		set {
			m_Stamina = value;
			if (m_Stamina > m_MaxStamina) m_Stamina = m_MaxStamina;
			//if (m_Stamina < 0f || Mathf.Approximately(m_Stamina, 0f)) m_Stamina = 0f;
			//else if (m_Stamina > m_MaxStamina) m_Stamina = m_MaxStamina;
		}
	}

	public float Armor {
		get {
			return m_Armor;
		}
	}

	public float MagicResistance {
		get {
			return m_MagicResistance;
		}
	}

	public float StaminaRegenRate {
		get {
			return m_StaminaRegenRate;
		}
	}

	public float MaxHealth {
		get {
			return m_MaxHealth;
		}
	}

	public float MaxMagic {
		get {
			return m_MaxMagic;
		}
	}

	public float MaxStamina {
		get {
			return m_MaxStamina;
		}
	}

	public float SqrPickupDistance {
		get {
			return PickupDistance * PickupDistance;
		}
	}

	#endregion

	public override string Name {
		get {
			return "LocalPlayer";
		}
	}

	private bool m_AllowMovement = true;

	/// <summary>
	/// If set to true, the user cannot motivate the player to move with the standard movement keys (WASD by default).
	/// Other source of movement not caused by the user may actually move the player, however.
	/// </summary>
	public bool AllowMovement {
		get {
			return m_AllowMovement;
		}
		set {
			if (value) FPController.SetState("NoMove", false);
			else FPController.SetState("NoMove", true);
			m_AllowMovement = value;
		}
	}

	public Vector3 PlayerVelocity {
		get {
			return FPController.m_Velocity;
		}
	}

	public bool IsMoving {
		get {
			return PlayerVelocity != Vector3.zero;
		}
	}

	public bool MovementKeysDown {
		get {
			return !(Mathf.Approximately(vp_Input.GetAxisRaw("Vertical"), 0f) && Mathf.Approximately(vp_Input.GetAxisRaw("Horizontal"), 0f));
		}
	}

	public UltimateOrbitCamera OrbitCamera { get; private set; }
	public vp_FPInput FPInput { get; private set; }
	public vp_FPController FPController { get; private set; }
	public Collider BodyCollider { get; private set; }
	public InventoryController Inventory { get; private set; }
	public PlayerUIController PlayerUI { get; private set; }

	void Start() {
		OrbitCamera = GetComponentInChildren<UltimateOrbitCamera>();
		FPInput = GetComponent<vp_FPInput>();
		FPController = GetComponent<vp_FPController>();
		BodyCollider = GetComponent<Collider>();
		Inventory = GetComponent<InventoryController>();
		PlayerUI = GetComponent<PlayerUIController>();

		// TODO: Update stats from save file

		SetupMaxHealth();
		SetupMaxMagic();
		SetupMaxStamina();

		m_HP = m_MaxHealth;
		m_MP = m_MaxMagic;
		m_Stamina = m_MaxStamina;

		m_TimeSinceStaminaUsed = StaminaRegenDelay;

		Alive = true; // Call the property's set routine to update the player's state.

		OrbitCamera.cameraCollision = true;

		PlayerUI.ToggleInteractText(false);

		Spawn(transform.position);
	}

	void SetupMaxHealth() {
		m_MaxHealth = MaxHealthDefault;
		for (int i = 0; i < m_MaxHealthLevel; i++) {
			m_MaxHealth += HealthLevelMod * i;
		}
		m_MaxHealth += m_MaxHealthAdditive;
	}

	void SetupMaxMagic() {
		m_MaxMagic = MaxMagicDefault;

		m_MaxMagic += m_MaxMagicAdditive;
	}

	void SetupMaxStamina() {
		m_MaxStamina = MaxStaminaDefault;
		for (int i = 1; i < m_MaxStaminaLevel; i++) {
			m_MaxStamina += StaminaLevelMod * i;
		}
		m_MaxStamina += m_MaxStaminaAdditive;
	}

	#region Update

	public override void Update() {
		if (Alive) {
			UpdateAbilities();

			UpdateMovement();

			UpdateRotation();

			UpdateInteract();
		}
		else if (vp_Input.GetButtonDown("Use")) { // Player wants to respawn
			Spawn(true);
		}

		UpdateHealth();

		UpdateStamina();

		base.Update();

		GameController.InvokeUpdate(this, this);
	}

	void UpdateAbilities() {
		Ability[] abilities = Inventory.Abilities;
		foreach (Ability ability in abilities) {
			if (ability != null && (ability.AbilityType == AbilityType.PASSIVE || ability.DoUpdate)) {
				ability.TriggerUpdate();
			}
		}

		// Because I'm too lazy to write four if statements. Plus, this looks cooler ;)
		for (int i = 1; i <= 4; i++) if (vp_Input.GetButtonDown("Ability" + i.ToString())) Inventory.Abilities[i].Trigger();
	}

	void UpdateMovement() {
		if (IsMoving) {
			GameController.InvokeMoved(this, this, transform.position, PlayerVelocity);
		}
	}

	void UpdateRotation() {
		if (MovementKeysDown) { // TODO: or casting ability or using weapon
			Vector3 oldCamPos = OrbitCamera.transform.position;
			Quaternion oldCamRot = OrbitCamera.transform.rotation;

			Vector3 newEulerRot = Vector3.zero;
			newEulerRot.y = OrbitCamera.transform.eulerAngles.y;
			transform.eulerAngles = newEulerRot;

			OrbitCamera.transform.position = oldCamPos;
			OrbitCamera.transform.rotation = oldCamRot;
		}
	}

	void UpdateHealth() {

		if (Alive) {
			if (m_HP < 0f || Mathf.Approximately(m_HP, 0f)) { // The player just died!
				Alive = false;

				PlayerUI.SetInteractText("Revive", true);

				GameController.InvokeDied(this, this);
			}
		}
    }

	void UpdateStamina() {

		if (!Alive && AliveLastFrame) Stamina = m_MaxStamina;
		else if (Alive && !m_StaminaLocked) {
			if (m_TimeSinceStaminaUsed < StaminaRegenDelay) m_TimeSinceStaminaUsed += Time.deltaTime;
			else {
				Stamina += StaminaRegenRate * Time.deltaTime;
			}
		}
	}

	void UpdateInteract() {
		bool showInteract = false;
		Item[] nearItems = FindObjectsOfType<Item>();
		foreach (Item item in nearItems) {
			if (item.Owner != this && Util.SqrDistance(item.transform.position, transform.position) < SqrPickupDistance) {
				if (vp_Input.GetButtonDown("Use")) {
					PickupItem(item);
                    continue;
				}
				showInteract = true;
				break;
			}
		}
		PlayerUI.SetInteractText("Pick Up", showInteract);
	}

	void FixedUpdate() {
		GameController.InvokeFixedUpdate(this, this);
	}

	void LateUpdate() {
		GameController.InvokeLateUpdate(this, this);
	}

	#endregion

	public void PickupItem(Item item) {
		if (item) {
			Inventory.Pickup(item);
			if (typeof(Weapon).IsAssignableFrom(item.GetType())) {
				Inventory.Equip((Weapon)item, WeaponSlot.RIGHT_HAND);
			}
		}
	}

	public void Spawn(bool atStatue = false) {
		GameObject[] statues;
		if (atStatue && (statues = GameObject.FindGameObjectsWithTag("Statue")).Length > 0) {
			int closestIndex = 0;
			float sqrDistance = 0f;
			for (int i = 0; i < statues.Length; i++) {
				float newDistance = Util.SqrDistance(transform.position, statues[i].transform.position);
				if (newDistance < sqrDistance) closestIndex = i;
			}
			Spawn(statues[closestIndex].transform.position + new Vector3(0f, 0f, 2f));
		}
		else Spawn(GameController.Instance.SpawnPoint);
	}

	public void Spawn(Vector3 pos) {
		transform.position = pos;

		// Reset stats
		HP = m_MaxHealth;
		MP = m_MaxMagic;
		Stamina = m_MaxStamina;

		Alive = true;

		// TODO: Roll Abilities and Skills that aren't locked in.

		GameController.InvokeSpawned(this, this);
	}

	public bool UseStamina(float stamina) {
		if (Stamina > 0f) { // Can use the stamina

			//m_TimeSinceStaminaUsed = 0f; // reset the timer for stamina usage
			m_TimeSinceStaminaUsed = 0f;
			Stamina -= stamina;

			return true; // Stamina was used, so return true
		}

		return false; // Stamina wasn't used, so return false
	}

	/// <summary>
	/// Stamina will not regenerate unless it is unlocked.
	/// </summary>
	/// <param name="lockStamina">Whether to lock or unlock stamina</param>
	public void LockStaminaRegen(bool lockStamina) {
		m_StaminaLocked = lockStamina;
	}

	public bool UseMagic(float magic) {
		if (MP > magic || Mathf.Approximately(MP, magic)) {

			MP -= magic;
			return true;
		}

		return false;
	}
	
	public override void Damage(DamageInfo damage) {
		
		if (!damage.Passed) {
			switch (damage.Element) {
				case DamageElement.FIRE:

					// TODO: Manage damage over time

					break;
				case DamageElement.ICE:

					// TOOD: Manage slowing down

					break;
				case DamageElement.NONE:
					float dmgMod = 1f;

					switch (damage.Type) {
						case DamageType.MAGIC:

							dmgMod = (m_MagicResistance > 0 || Mathf.Approximately(m_MagicResistance, 0f)) ?
								ResiModConstant / (ResiModConstant + m_MagicResistance) : // if Resi >= 0
								2f - (ResiModConstant / (ResiModConstant - m_MagicResistance)); // otherwise

							break;
						case DamageType.PHYSICAL:

							dmgMod = (m_Armor > 0 || Mathf.Approximately(m_Armor, 0f)) ?
								ArmorModConstant / (ArmorModConstant + m_Armor) : // if Armor >= 0
								2f - (ArmorModConstant / (ArmorModConstant - m_Armor)); // otherwise

							break;
					}

					HP -= dmgMod * damage.Damage;

					base.Damage(damage);

					DebugConsole.Log(FullName + ".HP = " + HP.ToString());

					break;
			}
		}
		else {
			// TODO: Damage over time
		}
	}
}
