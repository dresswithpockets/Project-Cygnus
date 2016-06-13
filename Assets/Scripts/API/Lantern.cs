using UnityEngine;
using System.Collections;
using System;

public abstract class Lantern : Equippable {
	public override EquippableSlot Slot {
		get {
			return EquippableSlot.LANTERN;
		}
	}
}