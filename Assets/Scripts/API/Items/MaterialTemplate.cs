using UnityEngine;
using System.Collections;

public abstract class Material_Template : Item_Template {
	
	private Material m_material_object;
	public Material material_object {

		get {

			return m_material_object;
		}
		internal set {

			m_material_object = value;
		}
	}
}
