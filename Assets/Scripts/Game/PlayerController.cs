using UnityEngine;
using System.Collections.Generic;

public class PlayerController : Pawn {

	#region Stats

	private bool m_IsAlive = false;
	private float m_HP = 0;
	private float m_Magic = 100;

	public float PickupDistance = 7.5f;

	public float MaxHealth = 100;
	public float MaxHealthDefault = 100;

	public float HP {
		get {
			return m_HP;
		}
		set {
			m_HP = value;
			if (m_HP < 0f || Mathf.Approximately(m_HP, 0f)) {
				m_HP = 0f;
				m_IsAlive = false;
			}
			else m_IsAlive = true;
		}
	}

	public float SqrPickupDistance {
		get {
			return PickupDistance * PickupDistance;
		}
	}

	#endregion

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
			if (value) m_FPController.SetState("Move");
			else m_FPController.SetState("NoMove");
			m_AllowMovement = value;
		}
	}

	public Vector3 PlayerVelocity {
		get {
			return m_FPController.m_Velocity;
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

	private UltimateOrbitCamera m_OrbitCamera;
	private vp_FPInput m_FPInput;
	private vp_FPController m_FPController;
	private Collider m_BodyCollider;
	private InventoryController m_Inventory;

	void Start() {
		m_OrbitCamera = GetComponentInChildren<UltimateOrbitCamera>();
		m_FPInput = GetComponent<vp_FPInput>();
		m_FPController = GetComponent<vp_FPController>();
		m_BodyCollider = GetComponent<Collider>();
		m_Inventory = GetComponent<InventoryController>();

		MaxHealth = MaxHealthDefault;

		AllowMovement = AllowMovement; // Call the property's set routine to update the player's state.

		m_OrbitCamera.cameraCollision = true;

		Spawn(transform.position);
	}

	#region Update

	void Update() {
		if (m_IsAlive) {
			UpdateInput();

			UpdateMovement();

			UpdateRotation();
		}

		GameController.InvokeUpdate(this, this);
	}

	void UpdateInput() {
		if (vp_Input.GetButtonDown("Use")) {
			AItem[] nearItems = FindObjectsOfType<AItem>();
			foreach (AItem item in nearItems) {
				if (Util.SqrDistance(item.transform.position, transform.position) < SqrPickupDistance) {
					PickupItem(item);
					break;
				}
			}
		}
	}

	void UpdateMovement() {
		if (IsMoving) {
			GameController.InvokeMoved(this, this, transform.position, PlayerVelocity);
		}
	}

	void UpdateRotation() {
		if (MovementKeysDown) { // TODO: or casting ability or using weapon
			Vector3 oldCamPos = m_OrbitCamera.transform.position;
			Quaternion oldCamRot = m_OrbitCamera.transform.rotation;

			Vector3 newEulerRot = Vector3.zero;
			newEulerRot.y = m_OrbitCamera.transform.eulerAngles.y;
			transform.eulerAngles = newEulerRot;

			m_OrbitCamera.transform.position = oldCamPos;
			m_OrbitCamera.transform.rotation = oldCamRot;
		}
	}

	void FixedUpdate() {
		GameController.InvokeFixedUpdate(this, this);
	}

	void LateUpdate() {
		GameController.InvokeLateUpdate(this, this);
	}

	#endregion

	public void PickupItem(AItem item) {
		if (item) {
			// Temporary picking up, specifically for weapons. Hidden inventory is nonexistent atm.
			if (typeof(AWeapon).IsAssignableFrom(item.GetType())) {
				m_Inventory.Pickup(item);
				m_Inventory.Equip((AWeapon)item, (WeaponSlot.RIGHT_HAND));
			}
		}
	}

	public void Spawn(Vector3 pos) {
		transform.position = pos;
		HP = MaxHealth;
		GameController.InvokeSpawned(this, this);
	}
}
