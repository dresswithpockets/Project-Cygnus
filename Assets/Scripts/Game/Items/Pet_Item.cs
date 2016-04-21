using UnityEngine;
using System.Collections;


// TODO: Implement Pet Items and Pets

public sealed class Pet_Item : Item {
	
	private bool m_is_item = true;

	public Pet_Item_Template pet_item_template {
		get {

			return (Pet_Item_Template)m_template;
		}
		internal set {

			m_template.game_object = gameObject;
			((Pet_Item_Template)m_template).pet_item_object = this;
			m_template = value;
		}
	}

	public bool is_item {

		get {

			return m_is_item;
		}
		internal set {

			m_is_item = value;
		}
	}
}
