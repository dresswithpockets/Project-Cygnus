using UnityEngine;
using System.Collections;

public abstract class Ability {

	public Pawn Owner { get; internal set; }

	public bool DoUpdate { get; private set; }

	public string FullName {
		get {
			return "[" +
				(GetType().BaseType.Equals(typeof(MonoBehaviour)) ? GetType().Name : GetType().BaseType.Name) +
				":" + Name + "]";
		}
	}

	public abstract string Name { get; }

	public virtual AbilityType AbilityType {
		get {
			return AbilityType.ACTIVE;
		}
	}

	/// <summary>
	/// If AbilityType is AbilityType.PASSIVE then Trigger is called when a player equips this ability.
	/// Otherwise, Trigger is called when a player triggers the ability.
	/// For non-casted abilities, it is expected that the entire ability is handled here.
	/// </summary>
	public virtual void Trigger() { GameController.InvokeTriggeredAbility(this, Owner, this); }

	/// <summary>
	/// TriggerUpdate is called once a frame when DoUpdate is true or if AbilityType is AbilityType.PASSIVE.
	/// For casted abilities, it is expected that the cast-time and the cast is handled here.
	/// </summary>
	public virtual void TriggerUpdate() { }

	/// <summary>
	/// Called when an event occurs on the Owner that invokes an ability Interrupt.
	/// For casted abilities, it is expected that this stops any ability from casting/being used.
	/// </summary>
	public virtual void Interrupt() { }
}
