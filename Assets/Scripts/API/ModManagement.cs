using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public abstract class Plugin {

	#region Template Code

	public abstract string Name { get; }

	public abstract string Author { get; }

	public abstract Version Version { get; }

	public virtual void Initialize() { }

	public virtual void Update() { }

	#endregion

	internal string Path {

		get {

			return "plugins/" + Name;
		}
	}

	internal Dictionary<string, Type> itemTemplateDictionary = new Dictionary<string, Type>();
	internal Dictionary<string, Type> weaponTemplateDictionary = new Dictionary<string, Type>();

	private bool insideModInit = true;

	internal void InternalInit() {
		
	}

	public void RegisterItem(string ID, Type item_type) {

		if (!insideModInit) {

			Debug.LogError("Types can only be registered inside Initialize().");
			return;
		}
		else Debug.LogError("Cannot import template of type " + item_type.Name + " because it does not inherit one of the abstract item templates in CygnusAPI.");
	}
}
