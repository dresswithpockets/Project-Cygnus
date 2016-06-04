using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public MeshFilter filter;
	public ModelController model_controller;
	internal Vox_Data data = new Vox_Data();

	internal ItemTemplate m_template = null;
	public ItemTemplate template {
		get {

			return m_template;
		}
		internal set {

			m_template = value;
			m_template.game_object = gameObject;
		}
	}

	#region Ownership

	internal NPCController m_NPC_owner = null;
	internal PlayerController m_player_owner = null;

	public NPCController NPC_owner {

		get {

			return m_NPC_owner;
		}
		internal set {

			m_NPC_owner = value;
		}
	}

	public PlayerController player_owner {

		get {

			return m_player_owner;
		}
		internal set {

			m_player_owner = value;
		}
	}

	public ItemOwner ownership {

		get {

			return (m_NPC_owner == null ? (m_player_owner == null ? ItemOwner.NONE : ItemOwner.PLAYER) : ItemOwner.NPC);
		}
	}

	internal void set_owner(PlayerController player) {

		m_NPC_owner = null;
		m_player_owner = player;
	}

	internal void set_owner(NPCController npc) {
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
		model_controller = GetComponent<ModelController>();
		model_controller.load_vox(data);

		if (template != null) template.spawned();

		Physics.IgnoreCollision(GetComponent<BoxCollider>(), PlayerController.instance.body_collider);
	}

	public virtual void Update() {
		if (template != null) template.exists_update();

		switch (ownership) {
			case ItemOwner.NPC:

				if (template != null) template.passive_update(m_NPC_owner);

				break;
			case ItemOwner.PLAYER:

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

			case ItemOwner.NONE:

				Debug.LogError("Cannot pick up item because no owner was assigned before the event was completed.", this);

				break;
			case ItemOwner.NPC:

				if (template != null) template.picked_up(m_NPC_owner);

				break;
			case ItemOwner.PLAYER:

				if (template != null) template.picked_up(m_player_owner);

				break;
		}
	}

	public virtual void drop() {

		switch (ownership) {

			case ItemOwner.NONE:

				Debug.LogError("Cannot drop item as no NPC or Player owns this item.", this);

				break;
			case ItemOwner.NPC:

				if (template != null) template.dropped(m_NPC_owner);

				break;
			case ItemOwner.PLAYER:

				if (template != null) template.dropped(m_player_owner);

				break;
		}
		drop_owner();
	}

	public void assign_template(ItemTemplate template, Vox_Data model) {

		this.template = template;
		data = model;
	}
}
