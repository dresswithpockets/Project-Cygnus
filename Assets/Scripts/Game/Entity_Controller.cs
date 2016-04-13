using UnityEngine;
using System.Collections;

public sealed class Entity_Controller : MonoBehaviour {

	internal Game_Entity ent = null;

	void Awake() {

		ent.awake();
	}

	void FixedUpdate() {

		ent.fixed_update();
	}

	void LateUpdate() {

		ent.late_update();
	}

	void OnCollisionEnter(Collision other) {

		ent.collision_enter(other);
	}

	void OnCollisionExit(Collision other) {

		ent.collision_exit(other);
	}

	void OnCollisionStay(Collision other) {

		ent.collision_stay(other);
	}

	void OnDestroy() {

		ent.destroyed();
	}

	void OnGUI() {

		ent.GUI();
	}

	void OnTriggerEnter(Collider other) {

		ent.trigger_enter(other);
	}

	void OnTriggerExit(Collider other) {

		ent.trigger_exit(other);
	}

	void OnTriggerStay(Collider other) {

		ent.trigger_stay(other);
	}

	void Start() {

		ent.start();
	}

	void Update() {

		ent.update();
	}
}
