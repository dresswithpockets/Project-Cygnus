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

	public virtual void used(Item item) { }

	public virtual void dropped(Item item) { }

	public virtual void picked_up(Item item) { }

	public virtual void crafted(Item item) { }

	public virtual void used(Weapon item) { }

	public virtual void equipped(Weapon weapon, int slot) { }

	public virtual void unequipped(Weapon weapon) { }

	public virtual void picked_up(Weapon weapon) { }

	public virtual void dropped(Weapon weapon) { }

	public virtual void crafted(Weapon weapon) { }

	public virtual void equipped(Equipment equipment, int slot) { }

	public virtual void unequipped(Equipment equipment) { }

	public virtual void picked_up(Equipment equipment) { }

	public virtual void dropped(Equipment equipment) { }

	public virtual void crafted(Equipment equipment) { }

	public virtual void used(Ability_Template ability) { }

	public virtual void learned(Ability_Template ability, int level) { }

	public virtual void defeated_other(Entity other) { }
}
