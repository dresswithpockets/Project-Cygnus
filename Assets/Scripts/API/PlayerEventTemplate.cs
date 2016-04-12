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

	private Player_Controller m_player;
	public Player_Controller Player {

		get {

			return m_player;
		}
		internal set {

			m_player = value;
		}
	}

	private Inventory_Controller m_player_inv;
	public Inventory_Controller player_inv {

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

	public virtual void used(Item_Template item) { }

	public virtual void dropped(Item_Template item) { }

	public virtual void picked_up(Item_Template item) { }

	public virtual void crafted(Item_Template item) { }

	public virtual void equipped(Item_Template weapon, int slot) { }

	public virtual void unequipped(Item_Template weapon) { }

	public virtual void used(Ability_Template ability) { }

	public virtual void learned(Ability_Template ability, int level) { }

	public virtual void defeated_other(Entity other) { }
}
