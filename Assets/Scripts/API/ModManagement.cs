using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public abstract class Mod_Template {

	#region Template Code

	public abstract string mod_name { get; }

	public abstract string mod_author { get; }

	public abstract Version mod_version { get; }

	public virtual void initialize() { }

	public virtual void update() { }

	#endregion

	internal string mod_path {

		get {

			return "plugins/" + mod_name;
		}
	}

	internal Dictionary<string, Type> item_template_dict = new Dictionary<string, Type>();
	internal Dictionary<string, Type> weapon_template_dict = new Dictionary<string, Type>();

	internal bool inside_mod_initer = true;

	internal void internal_init() {
		
		
	}

	public void register_item(string ID, Type item_type) {

		if (!inside_mod_initer) {

			Debug.LogError("Templates can only be registered inside Initialize().");
			return;
		}
		else Debug.LogError("Cannot import template of type " + item_type.Name + " because it does not inherit one of the abstract item templates in CygnusAPI.");
	}
}
