using UnityEngine;
using System.Collections;

public abstract class Item_Template {

	private GameObject m_owner;
	public GameObject owner {

		get {

			return m_owner;
		}
		internal set {

			m_owner = value;
		}
	}

	private Item m_item;
	public Item item {

		get {

			return m_item;
		}
		internal set {

			m_item = value;
		}
	}

	public abstract string name { get; }

	public abstract string description { get; }

	public abstract bool used_by_AI { get; }

	public abstract Item_Classification classification { get; }

	public abstract string model_ID { get; }

	public virtual void spanwed() { }

	public virtual void exists_update() { }

	public virtual void passive_update(Player_Controller player) { }

	public virtual void passive_update(NPC npc) { }

	public virtual void fixed_update() { }

	public virtual void late_update() { }

	public virtual void picked_up(Player_Controller player) { }

	public virtual void picked_up(NPC npc) { }

	public virtual void dropped(Player_Controller player) { }

	public virtual void dropped(NPC npc) { }
}
