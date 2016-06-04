using UnityEngine;
using System.Collections;

public sealed class Consumable : Item {

	public ConsumableTemplate consumable_template {

		get {

			return (ConsumableTemplate)m_template;
		}
		internal set {

			m_template.game_object = gameObject;
			((ConsumableTemplate)m_template).consumable_object = this;
			m_template = value;
		}
	}
}
