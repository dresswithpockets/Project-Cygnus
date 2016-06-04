using UnityEngine;
using System.Collections;


// TODO: Implement Pet Items and Pets

public sealed class PetItem : Item {
	
	private bool m_is_item = true;

	public PetItemTemplate pet_item_template {
		get {

			return (PetItemTemplate)m_template;
		}
		internal set {

			m_template.game_object = gameObject;
			((PetItemTemplate)m_template).pet_item_object = this;
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
