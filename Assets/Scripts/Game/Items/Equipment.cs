using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour {

	private Equipment_Template m_template = null;
	public Equipment_Template template {

		get {

			return m_template;
		}
		internal set {

			m_template.owner = gameObject;
			m_template.equipment_object = this;
		}
	}

	internal MeshFilter filter;

	#region Ownership

	internal NPC m_NPC_owner = null;
	internal Player_Controller m_player_owner = null;

	public NPC NPC_owner {

		get {

			return m_NPC_owner;
		}
	}

	public Player_Controller player_owner {

		get {

			return m_player_owner;
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

	#endregion

	void Start() {

		filter = GetComponent<MeshFilter>();
		template.spawned();
	}

	void Update() {

		template.exists_update();

		switch (ownership) {
			case Item_Owner.NPC:

				template.passive_update(m_NPC_owner);

				break;
			case Item_Owner.PLAYER:

				template.passive_update(m_player_owner);

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

				Debug.LogError("Cannot pick up weapon because no owner was assigned before the event was completed.", this);

				break;
			case Item_Owner.NPC:

				template.picked_up(m_NPC_owner);

				break;
			case Item_Owner.PLAYER:

				template.picked_up(m_player_owner);

				break;
		}
	}

	public void drop() {

		switch (ownership) {

			case Item_Owner.NONE:
				Debug.LogError("Cannot drop weapon as no NPC or Player owns this item.", this);
				break;
			case Item_Owner.NPC:
				template.dropped(m_NPC_owner);
				break;
			case Item_Owner.PLAYER:
				template.dropped(m_player_owner);
				break;
		}
	}

	public void equip(int slot) {

		switch (ownership) {

			case Item_Owner.NONE:

				Debug.LogError("Cannot equip weapon as no NPC or Player owns this item.", this);

				break;
			case Item_Owner.NPC:

				template.equipped(m_NPC_owner, slot);

				break;
			case Item_Owner.PLAYER:

				template.equipped(m_player_owner, slot);

				break;
		}
	}

	public void assign_template(Equipment_Template template, MeshFilter model) {

		this.template = template;
		filter.mesh = model.sharedMesh;
	}
}
