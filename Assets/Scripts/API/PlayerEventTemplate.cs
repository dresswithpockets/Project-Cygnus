using UnityEngine;
using System.Collections;

public abstract class Player_Event_Template {

	private GameObject m_owner;
	public GameObject owner {

		get {

			return m_owner;
		}
		internal set {

			m_owner = value;
		}
	}

	private PlayerController m_player;
	public PlayerController Player {

		get {

			return m_player;
		}
		internal set {

			m_player = value;
		}
	}

	private InventoryController m_player_inv;
	public InventoryController player_inv {

		get {

			return m_player_inv;
		}
		internal set {

			m_player_inv = value;
		}
	}

	public virtual void spawned() { }

	public virtual void died(Entity attacker) { }

	public virtual void damaged(Damage damage) { }

	public virtual void moved(Vector3 position, Vector3 delta) { }

	public virtual void jumped() { }

	public virtual void landed() { }

	public virtual void update() { }

	public virtual void fixed_update() { }

	public virtual void late_update() { }

	public virtual void used(ItemTemplate item) { }

	public virtual void dropped(ItemTemplate item) { }

	public virtual void picked_up(ItemTemplate item) { }

	public virtual void crafted(ItemTemplate item) { }

	public virtual void equipped(ItemTemplate weapon, int slot) { }

	public virtual void unequipped(ItemTemplate weapon) { }

	public virtual void used(Ability_Template ability) { }

	public virtual void learned(Ability_Template ability, int level) { }

	public virtual void defeated_other(Entity other) { }
}
