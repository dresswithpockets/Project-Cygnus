using UnityEngine;
using System.Collections;

public abstract class AWeapon : AItem, IEquippable {

	public EquippableSlot Slot { get; set; }
}
