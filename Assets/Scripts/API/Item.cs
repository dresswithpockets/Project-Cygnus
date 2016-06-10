using UnityEngine;
using System;
using System.Collections;

public abstract class Item : MonoBehaviour {

	public Pawn Owner { get; internal set; }

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

	/// <summary>
	/// Returns a BaseType-lead name of the item. Example: [AItem:Water Bottle]
	/// </summary>
	public string FullName {
		get {
			return "[" +
				(GetType().BaseType.Equals(typeof(MonoBehaviour)) ? GetType().Name : GetType().BaseType.Name) +
				":" + Name + "]";
		}
	}

	public virtual string Name {
		get {
			return "Default" + GetType().Name;
		}
	}

	public virtual void Start() {

	}

	public virtual void Update() {

	}
}
