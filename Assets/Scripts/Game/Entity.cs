using UnityEngine;
using System.Collections;

public class Entity {
	private GameObject m_object = null;
	public GameObject game_object {

		get {

			return m_object;
		}
		internal set {

			m_object = value;
		}
	}

	public Vector3 position {

		get {

			return m_object.transform.position;
		}
		set {

			m_object.transform.position = value;
		}
	}

	public Vector3 rotation {

		get {

			return m_object.transform.eulerAngles;
		}
		set {

			m_object.transform.eulerAngles = value;
		}
	}

	public bool is_player {

		get {

			return (m_object.GetComponent<Player_Controller>() != null);
		}
	}

	public bool is_NPC {

		get {

			return (m_object.GetComponent<NPC>() != null);
		}
	}

	// Returns null if no PlayerController component is attached to the owner object
	public Player_Controller try_get_player() {

		return m_object.GetComponent<Player_Controller>();
	}

	// Returns null if no NPC component is attached to the owner object
	public NPC try_get_npc() {

		return m_object.GetComponent<NPC>();
	}

	public void do_damage(Damage damage) {

		m_object.SendMessage("DoDamage", damage);
	}

	public static explicit operator Entity(GameObject go) {

		return new Entity() {

			m_object = go
		};
	}

	public static explicit operator GameObject(Entity ent) {

		return ent.m_object;
	}
}

public class Game_Entity : Entity {

	public virtual void awake() { }

	public virtual void fixed_update() { }

	public virtual void late_update() { }

	public virtual void collision_enter(Collision other) { }

	public virtual void collision_exit(Collision other) { }

	public virtual void collision_stay(Collision other) { }

	public virtual void destroyed() { }

	public virtual void GUI() { }

	public virtual void trigger_enter(Collider other) { }

	public virtual void trigger_exit(Collider other) { }

	public virtual void trigger_stay(Collider other) { }

	public virtual void start() { }

	public virtual void update() { }

	public void create_instance(Vector3 pos, Vector3 rot) {
		if (game_object == null) {
			game_object = (GameObject)GameObject.Instantiate(Game_Controller.instance.ent_prefab, pos, Quaternion.Euler(rot));
			return;
		}

		Debug.LogError("Attempted to instantiate an entity as a Unity GameObject that has already been instantiated in the scene.", (GameObject)this);
	}
}