using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public MeshFilter filter;
	internal Vox_Data data = new Vox_Data();

	internal Item_Template m_template = null;
	public Item_Template template {
		get {

			return m_template;
		}
		internal set {

			m_template = value;
			m_template.game_object = gameObject;
		}
	}

	#region Ownership

	internal NPC m_NPC_owner = null;
	internal Player_Controller m_player_owner = null;

	public NPC NPC_owner {

		get {

			return m_NPC_owner;
		}
		internal set {

			m_NPC_owner = value;
		}
	}

	public Player_Controller player_owner {

		get {

			return m_player_owner;
		}
		internal set {

			m_player_owner = value;
		}
	}

	public Item_Owner ownership {

		get {

			return (m_NPC_owner == null ? (m_player_owner == null ? Item_Owner.NONE : Item_Owner.PLAYER) : Item_Owner.NPC);
		}
	}

	internal void set_owner(Player_Controller player) {

		m_NPC_owner = null;
		m_player_owner = player;
	}

	internal void set_owner(NPC npc) {
		m_player_owner = null;
		m_NPC_owner = npc;
	}

	internal void drop_owner() {

		player_owner = null;
		NPC_owner = null;
	}

	#endregion

	public virtual void Start() {

		filter = GetComponent<MeshFilter>();
		data.to_filter(ref filter);

		if (template != null) template.spawned();

		Physics.IgnoreCollision(GetComponent<BoxCollider>(), Player_Controller.instance.body_collider);
	}

	public virtual void Update() {
		if (template != null) template.exists_update();

		switch (ownership) {
			case Item_Owner.NPC:

				if (template != null) template.passive_update(m_NPC_owner);

				break;
			case Item_Owner.PLAYER:

				if (template != null) template.passive_update(m_player_owner);

				break;
		}
	}

	public virtual void FixedUpdate() {
		if (template != null) template.fixed_update();
	}

	public virtual void LateUpdate() {
		if (template != null) template.late_update();
	}

	public virtual void pick_up() {

		switch (ownership) {

			case Item_Owner.NONE:

				Debug.LogError("Cannot pick up item because no owner was assigned before the event was completed.", this);

				break;
			case Item_Owner.NPC:

				if (template != null) template.picked_up(m_NPC_owner);

				break;
			case Item_Owner.PLAYER:

				if (template != null) template.picked_up(m_player_owner);

				break;
		}
	}

	public virtual void drop() {

		switch (ownership) {

			case Item_Owner.NONE:

				Debug.LogError("Cannot drop item as no NPC or Player owns this item.", this);

				break;
			case Item_Owner.NPC:

				if (template != null) template.dropped(m_NPC_owner);

				break;
			case Item_Owner.PLAYER:

				if (template != null) template.dropped(m_player_owner);

				break;
		}
		drop_owner();
	}

	public void assign_template(Item_Template template, Vox_Data model) {

		this.template = template;
		data = model;
	}
}
