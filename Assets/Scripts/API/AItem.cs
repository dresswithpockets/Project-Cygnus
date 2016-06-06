using UnityEngine;
using System;
using System.Collections;

public abstract class AItem : MonoBehaviour {

	private Pawn m_Owner = null;
	public Pawn Owner {
		get {
			return m_Owner;
		}
		internal set {
			m_Owner = value;
		}
	}

	public bool IsEquippable {
		get {
			foreach (Type t in GameController.EquippableTypes)
				if (t.IsAssignableFrom(GetType())) return true;

			return false;
		}
	}

	public void ToggleItem(bool toggle) {

		Renderer[] itemRenderers = GetComponentsInChildren<Renderer>();
		Collider[] itemColliders = GetComponentsInChildren<Collider>();

		foreach (Renderer r in itemRenderers) {
			r.enabled = toggle;
		}

		foreach (Collider c in itemColliders) {
			c.enabled = toggle;
		}
	}

	public virtual string Name {
		get {
			return "Default" + GetType().Name;
		}
	}

	public virtual string FullName {
		get {
			return "[AItem:" + Name + "]";
		}
	}

	public virtual void Start() {

	}

	public virtual void Update() {

	}
}
