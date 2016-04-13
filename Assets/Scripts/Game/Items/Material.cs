using UnityEngine;
using System;
using System.Collections;

public sealed class Material : MonoBehaviour {

	public MeshFilter filter;

	private Material_Template m_template = null;

	public Material_Template template {

		get {

			return m_template;
		}
		internal set {

			m_template = value;
			m_template.game_object = gameObject;
			m_template.material_object = this;
		}
	}

	#region Ownership

	private NPC m_NPC_owner = null;
	private Player_Controller m_player_owner = null;


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

			return (NPC_owner == null ? (player_owner == null ? Item_Owner.NONE : Item_Owner.PLAYER) : Item_Owner.NPC);
		}
	}

	internal void set_owner(Player_Controller player) {

		NPC_owner = null;
		player_owner = player;
	}

	internal void set_owner(NPC npc) {

		player_owner = null;
		NPC_owner = npc;
	}

	internal void drop_owner() {

		player_owner = null;
		NPC_owner = null;
	}

	#endregion

	void Start() {

		filter = GetComponent<MeshFilter>();
		template.spawned();
	}

	void Update() {

		template.exists_update();

		switch (ownership) {
			case Item_Owner.NPC:

				template.passive_update(NPC_owner);

				break;
			case Item_Owner.PLAYER:

				template.passive_update(player_owner);

				break;
		}
	}

	void FixedUpdate() {

		template.fixed_update();
	}

	void LateUpdate() {

		template.late_update();
	}

	public void pick_up() {

		switch (ownership) {

			case Item_Owner.NONE:

				Debug.LogError("Cannot pick up item because no owner was assigned before the event was completed.", this);

				break;
			case Item_Owner.NPC:

				template.picked_up(NPC_owner);

				break;
			case Item_Owner.PLAYER:
				template.picked_up(player_owner);
				break;
		}
	}

	public void drop() {

		switch (ownership) {

			case Item_Owner.NONE:

				Debug.LogError("Cannot drop item as no NPC or Player owns this item.", this);

				break;
			case Item_Owner.NPC:

				template.dropped(NPC_owner);

				break;
			case Item_Owner.PLAYER:

				template.dropped(player_owner);

				break;
		}
	}

	public void assign_template(Material_Template template, MeshFilter model) {

		this.template = template;
		if (model != null) {
			filter.mesh = model.sharedMesh;
		}
	}
}
