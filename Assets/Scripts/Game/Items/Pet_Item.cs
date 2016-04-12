using UnityEngine;
using System.Collections;


// TODO: Implement Pet Items and Pets

public class Pet_Item : MonoBehaviour {

	private MeshFilter m_filter;
	private bool m_is_item = true;
	private Pet_Item_Template m_template = null;

	public Pet_Item_Template template {
		get {

			return m_template;
		}
		internal set {

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

	public MeshFilter filter {
		get {
			return m_filter;
		}
		set {
			m_filter = value;
		}
	}

	public void assign_template(Pet_Item_Template template, MeshFilter model) {

		this.template = template;
		filter.mesh = model.sharedMesh;
	}
}
