using UnityEngine;
using System.Collections;


// TODO: Implement Pet Items and Pets

public sealed class Pet_Item : MonoBehaviour {

	private MeshFilter m_filter;
	private bool m_is_item = true;
	private Pet_Item_Template m_template = null;

	public Pet_Item_Template template {
		get {

			return m_template;
		}
		internal set {

			m_template = value;
			m_template.game_object = gameObject;
			m_template.pet_item_object = this;
		}
	}

	public bool is_item {

		get {

			return m_is_item;
		}
		internal set {

			m_is_item = value;
		}
	}

	public MeshFilter filter {
		get {
			return m_filter;
		}
		set {
			m_filter = value;
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

	public void assign_template(Pet_Item_Template template, MeshFilter model) {

		this.template = template;
		if (model != null) {
			filter.mesh = model.sharedMesh;
		}
	}
}
