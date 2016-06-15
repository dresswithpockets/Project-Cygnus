using UnityEngine;
using System;
using System.Collections;

public abstract class Item : MonoBehaviour {

	public Pawn Owner { get; internal set; }

	public bool IsEquippable {
		get {
			return typeof(Equippable).IsAssignableFrom(GetType());
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
				(!string.IsNullOrEmpty(InternalTypeName) ? InternalTypeName : (GetType().BaseType.Equals(typeof(MonoBehaviour)) ? GetType().Name : GetType().BaseType.Name)) +
				":" + Name + "]";
		}
	}

	internal virtual string InternalTypeName {
		get {
			return "";
		}
	}

	public virtual string Name {
		get {
			return "Default" + GetType().Name;
		}
	}

	public virtual string DefaultModel {
		get {
			return "Default";
		}
	}

	public virtual void OnStart() {

	}

	public virtual void OnUpdate() {

	}

	public void Start() {
		OnStart();
	}

	public void Update() {
		OnUpdate();

		PlayerController local = GameController.Instance.LocalPlayer;
		if (local && Util.SqrDistance(local.transform.position, transform.position) < local.SqrPickupDistance && !local.NearbyItems.Contains(this)) local.NearbyItems.Add(this);
	}
}
