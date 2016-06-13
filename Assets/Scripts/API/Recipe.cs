using UnityEngine;
using System.Collections;

public struct RecipePart {
	public readonly Item Material;
	public readonly int Quantity;

	public RecipePart(Item material, int quantity) {
		Material = material;
		Quantity = quantity;
	}
}

public struct Recipe {
	internal static int nextID = 1;
	private int m_ID;
	public int ID {
		get {
			if (m_ID == 0) {
				m_ID = nextID;
				nextID++;
			}
			return m_ID;
		}
	}

	public Item CraftedItem;
	public readonly RecipePart[] Parts;

	public readonly bool RequireTable;

	public Recipe(Item craftedItem, bool requireTable = false, params RecipePart[] parts) {
		m_ID = 0;
		CraftedItem = craftedItem;
		RequireTable = requireTable;
		Parts = parts;
	}
}
