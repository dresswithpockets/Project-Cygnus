using UnityEngine;
using System;
using System.Collections.Generic;

public sealed class Material : Item {

	public MaterialTemplate material_template {

		get {

			return (MaterialTemplate)m_template;
		}
		internal set {

			m_template.game_object = gameObject;
			((MaterialTemplate)m_template).material_object = this;
			m_template = value;
		}
	}
}
