using UnityEngine;
using System.Collections;

public class PlayerController : Pawn {

	#region Stats

	private bool m_IsAlive = false;
	private float m_HP = 0;
	private float m_Magic = 100;

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

	#endregion

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

	private UltimateOrbitCamera m_OrbitCamera;
	private vp_FPInput m_FPInput;
	private vp_FPController m_FPController;
	private Collider m_BodyCollider;

	void Start() {
		m_OrbitCamera = GetComponentInChildren<UltimateOrbitCamera>();
		m_FPInput = GetComponent<vp_FPInput>();
		m_FPController = GetComponent<vp_FPController>();
		m_BodyCollider = GetComponent<Collider>();

		MaxHealth = MaxHealthDefault;

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
		if (vp_Input.GetButtonDown("Attack1")) {
			// TODO: Test sword swinging
		}
	}

	void UpdateMovement() {
		if (IsMoving) {
			GameController.InvokeMoved(this, this, transform.position, PlayerVelocity);
		}
	}

	void UpdateRotation() {
		if (IsMoving) { // TODO: or casting ability or using weapon
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

	public void Spawn(Vector3 pos) {
		transform.position = pos;
		HP = MaxHealth;
		GameController.InvokeSpawned(this, this);
	}
}
