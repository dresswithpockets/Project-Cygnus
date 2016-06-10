using UnityEngine;
using System.Collections;

public abstract class Weapon : Item {

	public bool m_Active = false;
	public bool Active {
		get {
			return m_Active;
		}
		internal set {
			m_Active = value;
		}
	}

	private bool m_Using = false;
	public bool Using {
		get {
			return m_Using;
		}
		set {
			if (m_Using != value && value && Owner != null) GameController.InvokeUsedItem(this, Owner, this);
			m_Using = value;
		}
	}

	public abstract WeaponHandiness Handiness { get; }
}
