using UnityEngine;
using System.Collections;

public sealed class Consumable : Item {

	public Consumable_Template consumable_template {

		get {

			return (Consumable_Template)m_template;
		}
		internal set {

			m_template.game_object = gameObject;
			((Consumable_Template)m_template).consumable_object = this;
			m_template = value;
		}
	}
}
