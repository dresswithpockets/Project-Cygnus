using UnityEngine;
using System.Collections;
using System;

public class Projectile : Pawn {
	
	public override string Name {
		get {
			return "Projectile";
		}
	}

	public override string DefaultModel {
		get {
			return "Projectile";
		}
	}

	private Rigidbody m_RBody { get; set; }
	private CharacterController m_Controller { get; set; }

	private Vector3 m_Velocity { get; set; }
	private float m_LifeTime { get; set; }

	private float m_TimeSinceAlive;

	public override void OnStart() {
		m_RBody = GetComponent<Rigidbody>();
		m_Controller = GetComponent<CharacterController>();
		GetComponentInChildren<BoxCollider>().isTrigger = true;
		m_RBody.useGravity = false;
		m_RBody.isKinematic = false;
		m_Controller.enabled = false;
	}

	public override void OnUpdate() {
		if (m_TimeSinceAlive > m_LifeTime) Destroy(gameObject);
		else {
			m_TimeSinceAlive += Time.deltaTime;
			//transform.position += m_Velocity * Time.deltaTime;
		}
	}

	public void FixedUpdate() {
		if (m_TimeSinceAlive < m_LifeTime) m_RBody.velocity = m_Velocity;
	}

	public void SetTrajectory(Vector3 velocity, float lifeTime) {
		m_Velocity = velocity;
		m_LifeTime = lifeTime;
	}

	public void OnTriggerEnter(Collider other) {
		if (other.GetComponent<Pawn>()) {
			other.SendMessage("Damage", new DamageInfo(DamageType.PHYSICAL, DamageElement.NONE, 10f, this, false), SendMessageOptions.DontRequireReceiver);
			Destroy(gameObject);
		}
	}
}
