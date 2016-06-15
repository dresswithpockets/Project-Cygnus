using UnityEngine;
using System.Collections.Generic;

public class PlayerController : Pawn {
	
	#region Stats
	
	public override string Name {
		get {
			return "LocalPlayer";
		}
	}

	private float m_HP = 0f;
	private float m_MP = 0f;
	private float m_Stamina = 0f;
	private float m_Dexterity = 0f;
	private float m_Armor = 0f;
	private float m_CritChance = 0f;
	private float m_MagicResistance = 0f;
	private float m_StaminaRegenRate = 30; // TODO: default value?
	private float m_HealthRegenRate = 0f; // TODO: default value?
	private float m_Tempo = 1f;

	private float m_MaxHealth = 0;
	private float m_MaxMagic = 0;
	private float m_MaxStamina = 0;

	private float m_MaxHealthAdditive = 0f;
	private float m_MaxMagicAdditive = 0f;
	private float m_MaxStaminaAdditive = 0f;
	private float m_DexterityAdditive = 0f;
	private float m_ArmorAdditive = 0f;
	private float m_CritAdditive = 0f;
	private float m_MagicResistanceAdditive = 0f;
	private float m_StaminaRegenAdditive = 0f;
	private float m_HealthRegenAdditive = 0f;
	private float m_TempoAdditive = 0f;

	private float m_MaxHealthProduct = 1f;
	private float m_MaxMagicProduct = 1f;
	private float m_MaxStaminaProduct = 1f;
	private float m_DexterityProduct = 1f;
	private float m_ArmorProduct = 1f;
	private float m_CritProduct = 1f;
	private float m_MagicResistanceProduct = 1f;
	private float m_StaminaRegenProduct = 1f;
	private float m_HealthRegenProduct = 1f;
	private float m_TempoProduct = 1f;

	private int m_MaxHealthLevel = 1;
	private int m_MaxMagicLevel = 1;
	private int m_MaxStaminaLevel = 1;

	private bool m_StaminaLocked = false;
	private float m_TimeSinceStaminaUsed = 0f;
	private float m_TimeSinceDamaged = 0f;
	private float m_TimeSinceMagicUsed = 0f;

	public float PickupDistance = 7.5f;

	public float HealthLevelMod = 10;
	public float MagicLevelMod = 5;
	public float StaminaLevelMod = 2;

	public float MaxHealthDefault = 100;
	public float MaxMagicDefault = 100;
	public float MaxStaminaDefault = 100;

	public float ArmorModConstant = 100f;
	public float ResiModConstant = 100f;
	
	public float StaminaRegenDelay = 1.5f;
	public float MagicRegenDelay = 1.5f;
	public float HealthRegenDelay = 1.5f;

	public float PowerAmplitude = 101f;
	public float PowerShift = 0.025f;

	public int Level {
		get {
			return Inventory.LearnedAbilities.Length + 1;
		}
	}

	public float Power { get; private set; }

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
		}
	}

	public float Dexterity {
		get {
			return (m_Dexterity + m_DexterityAdditive) * m_DexterityProduct;
		}
	}

	public float Armor {
		get {
			return (m_Armor + m_ArmorAdditive) * m_ArmorProduct;
		}
	}

	public float Crit {
		get {
			return (m_CritChance + m_CritAdditive) * m_ArmorProduct;
		}
	}

	public float MagicResistance {
		get {
			return (m_MagicResistance + m_MagicResistanceAdditive) * m_MagicResistanceProduct;
		}
	}

	public float StaminaRegenRate {
		get {
			return (m_StaminaRegenRate + m_StaminaRegenAdditive) * m_StaminaRegenProduct;
		}
	}

	public float HealthRegenRate {
		get {
			return (m_HealthRegenRate + m_HealthRegenAdditive) * m_HealthRegenProduct;
		}
	}

	public float Tempo {
		get {
			return (m_Tempo + m_TempoAdditive) * m_TempoProduct;
		}
	}

	public float MaxHealth {
		get {
			return (m_MaxHealth + m_MaxHealthAdditive) * m_MaxHealthProduct;
		}
	}

	public float MaxMagic {
		get {
			return (m_MaxMagic + m_MaxMagicAdditive) * m_MaxMagicProduct;
		}
	}

	public float MaxStamina {
		get {
			return (m_MaxStamina + m_MaxStaminaAdditive) * m_MaxStaminaProduct;
		}
	}

	public float SqrPickupDistance {
		get {
			return PickupDistance * PickupDistance;
		}
	}

	public int MaxHealthLevel {
		get {
			return m_MaxHealthLevel;
		}
		private set {
			m_MaxHealthLevel = value;
			SetupMaxHealth();
		}
	}

	public int MaxMagicLevel {
		get {
			return m_MaxMagicLevel;
		}
		private set {
			m_MaxMagicLevel = value;
			SetupMaxMagic();
		}
	}

	public int MaxStaminaLevel {
		get {
			return m_MaxStaminaLevel;
		}
		private set {
			m_MaxStaminaLevel = value;
			SetupMaxStamina();
		}
	}

	#endregion

	#region Soul Stats
	
	private float m_HealthUpgradeConstant = 10f;
	private float m_AbilityUpgradeConstant = 25f;
	private float m_NewAbilityConstant = 75f;

	public int Souls { get; private set; }

	#endregion

	#region Interaction Properties

	public List<Item> NearbyItems { get; private set; }

	public List<GameObject> NearbyTables { get; private set; }

	#endregion

	#region Movement

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

	#endregion

	#region Setup

	public UltimateOrbitCamera OrbitCamera { get; private set; }
	public vp_FPInput FPInput { get; private set; }
	public vp_FPController FPController { get; private set; }
	public Collider BodyCollider { get; private set; }
	public InventoryController Inventory { get; private set; }
	public PlayerUIController PlayerUI { get; private set; }

	public override void OnStart() {
		OrbitCamera = GetComponentInChildren<UltimateOrbitCamera>();
		FPInput = GetComponent<vp_FPInput>();
		FPController = GetComponent<vp_FPController>();
		BodyCollider = GetComponent<Collider>();
		Inventory = GetComponent<InventoryController>();
		PlayerUI = GetComponent<PlayerUIController>();

		NearbyItems = new List<Item>();
		NearbyTables = new List<GameObject>();

		// TODO: Update stats from save file

		SetupMaxHealth();
		SetupMaxMagic();
		SetupMaxStamina();

		m_HP = MaxHealth;
		m_MP = MaxMagic;
		m_Stamina = m_MaxStamina;

		// TODO: Get player items and abilities from save file

		SetNewPower();

		m_TimeSinceStaminaUsed = StaminaRegenDelay;
		m_TimeSinceDamaged = HealthRegenDelay;
		m_TimeSinceMagicUsed = MagicRegenDelay;

		Alive = true; // Call the property's set routine to update the player's state.

		OrbitCamera.cameraCollision = true;

		PlayerUI.ToggleInteractText(false);

		ToggleGameMenu(false);

		GameController.Instance.LocalPlayer = this;
	}

	void SetupMaxHealth() {
		m_MaxHealth = MaxHealthDefault;
		for (int i = 0; i < m_MaxHealthLevel; i++) {
			m_MaxHealth += HealthLevelMod * i;
		}
	}

	void SetupMaxMagic() {
		m_MaxMagic = MaxMagicDefault;
		for (int i = 1; i < m_MaxMagicLevel; i++) {
			m_MaxMagic += MagicLevelMod * i;
		}
	}

	void SetupMaxStamina() {
		m_MaxStamina = MaxStaminaDefault;
		for (int i = 1; i < m_MaxStaminaLevel; i++) {
			m_MaxStamina += StaminaLevelMod * i;
		}
	}

	#endregion

	#region Update

	public override void OnUpdate() {
		if (GameController.ShowGameMenu) {

			if (vp_Input.GetButtonDown("Menu")) ToggleGameMenu(false);
		}
		else {
			if (Alive) {

				ClearFrameStats();

				UpdateStats();

				UpdateAbilities();

				UpdateMovement();

				UpdateRotation();

				UpdateInteract();
			}
			else if (vp_Input.GetButtonDown("Use")) { // Player wants to respawn
				Spawn(true);
			}

			if (vp_Input.GetButtonDown("Menu")) ToggleGameMenu(true);
		}

		UpdateHealth();

		UpdateMagic();

		UpdateStamina();

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

	void UpdateStats() {
		foreach (Equippable equip in Inventory.EquippedItems) {
			if (equip == null) continue;
			foreach (StatMod statMod in equip.StatModifiers) {
				AddFrameStat(statMod);
			}
		}
	}

	void UpdateHealth() {

		if (Alive) {
			if (HP < 0f || Mathf.Approximately(HP, 0f)) { // The player just died!
				Alive = false;

				PlayerUI.SetInteractText("Revive", true);

				GameController.InvokeDied(this, this);
			}
			else {
				if (m_TimeSinceDamaged > HealthRegenDelay) {
					HP += HealthRegenRate * Time.deltaTime;
				}
				else m_TimeSinceDamaged += Time.deltaTime;
			}
		}
    }

	void UpdateMagic() {
		if (Alive) {
			if (m_TimeSinceMagicUsed > MagicRegenDelay) {
				MP += MagicRegenDelay * Time.deltaTime;
			}
			else m_TimeSinceMagicUsed += Time.deltaTime;
		}
	}

	void UpdateStamina() {

		if (!Alive && AliveLastFrame) Stamina = MaxStamina;
		else if (Alive && !m_StaminaLocked) {
			if (m_TimeSinceStaminaUsed < StaminaRegenDelay) m_TimeSinceStaminaUsed += Time.deltaTime;
			else {
				Stamina += StaminaRegenRate * Time.deltaTime;
			}
		}
	}

	void UpdateInteract() {
		List<int> itemsToRemove = new List<int>();
		string interactText = "";

		for (int i = 0; i < NearbyItems.Count; i++) {
			Item item = NearbyItems[i];
			if (item.Owner != this) {
				if (Util.SqrDistance(item.transform.position, transform.position) < SqrPickupDistance) {
					if (vp_Input.GetButtonDown("Interact")) PickupItem(item);
					interactText = "Pick Up";
					break;
				}
				else {
					itemsToRemove.Add(i);
				}
			}
		}

		foreach (int i in itemsToRemove) NearbyItems.RemoveAt(i);

		PlayerUI.SetInteractText(interactText, !string.IsNullOrEmpty(interactText));
	}

	void FixedUpdate() {
		GameController.InvokeFixedUpdate(this, this);
	}

	void LateUpdate() {
		GameController.InvokeLateUpdate(this, this);
	}

	#endregion
	
	#region Frame Stats

	void ClearFrameStats() {
		m_MaxHealthAdditive = 0f;
		m_MaxMagicAdditive = 0f;
		m_MaxStaminaAdditive = 0f;
		m_DexterityAdditive = 0f;
		m_ArmorAdditive = 0f;
		m_CritAdditive = 0f;
		m_MagicResistanceAdditive = 0f;
		m_StaminaRegenAdditive = 0f;
		m_HealthRegenAdditive = 0f;
		m_TempoAdditive = 0f;

		m_MaxHealthProduct = 1f;
		m_MaxMagicProduct = 1f;
		m_MaxStaminaProduct = 1f;
		m_DexterityProduct = 1f;
		m_ArmorProduct = 1f;
		m_CritProduct = 1f;
		m_MagicResistanceProduct = 1f;
		m_StaminaRegenProduct = 1f;
		m_HealthRegenProduct = 1f;
		m_TempoProduct = 1f;
	}

	void AddFrameStat(StatMod mod) {
		switch (mod.Type) {
			case StatModifierType.PRODUCT:
				switch (mod.Stat) {
					case CharStat.ARMOR:
						m_ArmorProduct += mod.Value;
						break;
					case CharStat.DEX:
						m_DexterityProduct += mod.Value;
						break;
					case CharStat.CRIT:
						m_CritProduct += mod.Value;
						break;
					case CharStat.HP:
						m_MaxHealthProduct += mod.Value;
						break;
					case CharStat.REG:
						m_HealthRegenProduct += mod.Value;
						break;
					case CharStat.RESI:
						m_MagicResistanceProduct += mod.Value;
						break;
					case CharStat.TEMPO:
						m_TempoProduct += mod.Value;
						break;
				}
				return;
		}

		switch (mod.Stat) {
			case CharStat.ARMOR:
				m_ArmorAdditive += mod.Value;
				break;
			case CharStat.DEX:
				m_DexterityAdditive += mod.Value;
				break;
			case CharStat.CRIT:
				m_CritAdditive += mod.Value;
				break;
			case CharStat.HP:
				m_MaxHealthAdditive += mod.Value;
				break;
			case CharStat.REG:
				m_HealthRegenAdditive += mod.Value;
				break;
			case CharStat.RESI:
				m_MagicResistanceAdditive += mod.Value;
				break;
			case CharStat.TEMPO:
				m_TempoAdditive += mod.Value;
				break;
		}
	}

	#endregion

	#region Inventory

	public void PickupItem(Item item) {
		if (item) {
			Inventory.Pickup(item);
			if (typeof(Weapon).IsAssignableFrom(item.GetType())) {
				Inventory.Equip((Weapon)item, WeaponSlot.RIGHT_HAND);
			}
		}
	}

	#endregion
	
	#region Stamina, Magic and Damage Usage

	public  bool CanUseStamina() {
		return Stamina > 0f;
	}

	public bool UseStamina(float stamina) {
		if (CanUseStamina()) { // Can use the stamina

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

	public bool CanUseMagic(float magic) {
		return MP > magic || Mathf.Approximately(MP, magic);
	}

	public bool UseMagic(float magic) {
		if (CanUseMagic(magic)) {

			m_TimeSinceMagicUsed = 0f;
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

					m_TimeSinceDamaged = 0f;

					base.Damage(damage);

					DebugConsole.Log(FullName + ".HP = " + HP.ToString());

					break;
			}
		}
		else {
			// TODO: Damage over time
		}
	}

	#endregion

	#region Soul Management

	public int GetSoulsForHealthUpgrade() {
		return Mathf.RoundToInt((m_MaxHealthLevel) * m_HealthUpgradeConstant);
	}

	public int GetSoulsForAbilityUpgrade(Ability ability) {
		// TODO: Factor in tier and rarity of the ability
		return Mathf.RoundToInt((ability.Level) * m_AbilityUpgradeConstant);
	}

	public int GetSoulsForNewAbility() {
		return Mathf.RoundToInt(Level * m_NewAbilityConstant);
	}

	public bool CanUpgradeHealth() {
		return Souls >= GetSoulsForHealthUpgrade();
	}

	public bool CanUpgradeAbility(Ability ability) {
		return Souls >= GetSoulsForAbilityUpgrade(ability);
	}

	public bool CanLevelUp() {
		return Souls >= GetSoulsForNewAbility();
	}

	public bool UpgradeHealth() {
		if (CanUpgradeHealth()) {

			Souls -= GetSoulsForHealthUpgrade();
			m_MaxHealthLevel++;
			return true;
		}
		return false;
	}

	public bool UpgradeAbility(Ability ability) {
		if (CanUpgradeAbility(ability) && ability.CanLevelUp()) {

			Souls -= GetSoulsForAbilityUpgrade(ability);
			ability.LevelUp();
			return true;
		}
		return false;
	}

	public bool LevelUp() {
		if (CanLevelUp()) {

			Souls -= GetSoulsForNewAbility();
			// TODO: Roll for a new ability.
			SetNewPower();
			return true;
		}
		return false;
	}

	#endregion

	#region Interaction

	#endregion

	public override void Spawn(Vector3 pos) {
		transform.position = pos;

		// Reset stats
		HP = MaxHealth;
		MP = MaxMagic;
		Stamina = MaxStamina;

		Alive = true;

		// TODO: Roll Abilities and Skills that aren't locked in.

		GameController.InvokeSpawned(this, this);
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

	public void ToggleGameMenu(bool toggle) {
		GameController.ShowGameMenu = toggle;
		FPInput.AllowGameplayInput = !toggle;
	}

	public float SetNewPower() {
		return 2 * (PowerAmplitude / Mathf.PI) * Mathf.Atan(Level * PowerShift);
	}
}
