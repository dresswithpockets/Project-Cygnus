using UnityEngine;
using System.Collections;

public abstract class Weapon : Equippable {
	
	public bool m_Using;
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
