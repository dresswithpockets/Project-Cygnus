using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public sealed class RecipeController : Item {

	public Recipe Recipe { get; set; }

	public override string Name {
		get {
			return Recipe.ID.ToString();
		}
	}

	internal override string InternalTypeName {
		get {
			return "Recipe";
		}
	}

	public override string DefaultModel {
		get {
			return "Recipe";
		}
	}
}
