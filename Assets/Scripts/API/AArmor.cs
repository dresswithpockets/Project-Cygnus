using UnityEngine;
using System.Collections;

public abstract class AArmor : AItem, IEquippable {

	public EquippableSlot Slot { get; set; }
}
