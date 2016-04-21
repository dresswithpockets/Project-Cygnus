using UnityEngine;
using System;
using System.Collections.Generic;

public sealed class Material : Item {

	public Material_Template material_template {

		get {

			return (Material_Template)m_template;
		}
		internal set {

			m_template.game_object = gameObject;
			((Material_Template)m_template).material_object = this;
			m_template = value;
		}
	}
}
