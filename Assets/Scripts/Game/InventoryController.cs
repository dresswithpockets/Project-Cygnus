using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryController : MonoBehaviour
{
	internal List<Item> m_ItemInventory = new List<Item>();
	internal List<Equipment> m_EquipmentInventory = new List<Equipment>();
	internal List<Weapon> m_WeaponInventory = new List<Weapon>();

	internal bool m_CastingAbility = false;
	internal Dictionary<string, Ability_Template> m_LearnedAbilities = new Dictionary<string, Ability_Template>();
	internal Ability_Template m_Ability1;
	internal Ability_Template m_Ability2;
	internal Ability_Template m_Ability3;
	internal Ability_Template m_Ability4;

	internal bool m_PlayerInventory = false;

	#region Properties

	public bool PlayerInventory
	{
		get
		{
			return m_PlayerInventory;
		}
		internal set
		{
			m_PlayerInventory = value;
		}
	}

	public List<Item> ItemInventory
	{
		get
		{
			return m_ItemInventory;
		}
		internal set
		{
			m_ItemInventory = value;
		}
	}

	public List<Equipment> EquipmentInventory
	{
		get
		{
			return m_EquipmentInventory;
		}
		internal set
		{
			m_EquipmentInventory = value;
		}
	}

	public List<Weapon> WeaponInventory
	{
		get
		{
			return m_WeaponInventory;
		}
		internal set
		{
			m_WeaponInventory = value;
		}
	}

	public bool CastingAbility
	{
		get
		{
			return m_CastingAbility;
		}
	}

	#endregion

	public void Pickup(Item item)
	{
		ItemInventory.Add(item);

		MeshRenderer[] renderers = item.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers)
		{
			r.enabled = true;
		}

		if (PlayerInventory)
		{
			GameController.InvokePlayerPickedUpItem(item);
		}
	}

	public void Pickup(Equipment equipment)
	{
		EquipmentInventory.Add(equipment);

		MeshRenderer[] renderers = equipment.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers)
		{
			r.enabled = true;
		}

		if (PlayerInventory)
		{
			GameController.InvokePlayerPickedUpEquipment(equipment);
		}
	}

	public void Pickup(Weapon weapon)
	{
		WeaponInventory.Add(weapon);

		MeshRenderer[] renderers = weapon.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers)
		{
			r.enabled = true;
		}

		if (PlayerInventory)
		{
			GameController.InvokePlayerPickedUpWeapon(weapon);
		}
    }

	public void Drop(Inventory_Category tab, int slot)
	{
		GameObject droppedItem = null;

		switch (tab)
		{
			case Inventory_Category.EQUIPMENT:
				droppedItem = EquipmentInventory[slot].gameObject;

				if (PlayerInventory)
				{
					GameController.InvokePlayerDroppedEquipment(EquipmentInventory[slot]);
		        }
				break;
			case Inventory_Category.ITEM:
				droppedItem = ItemInventory[slot].gameObject;

				if (PlayerInventory)
				{
					GameController.InvokePlayerDroppedItem(ItemInventory[slot]);
				}
				break;
			case Inventory_Category.WEAPON:
				droppedItem = WeaponInventory[slot].gameObject;

				if (PlayerInventory)
				{
					GameController.InvokePlayerDroppedWeapon(WeaponInventory[slot]);
				}
				break;
		}

		MeshRenderer[] renderers = droppedItem.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderers)
		{
			r.enabled = true;
		}
	}

	public bool HasLearned(string abilityID)
	{
		return m_LearnedAbilities.ContainsKey(abilityID);
	}

	internal void LearnAbility(string abilityID, Ability_Type ability)
	{
		if (!HasLearned(abilityID))
		{
			m_LearnedAbilities.Add(abilityID, (Ability_Template)Activator.CreateInstance(ability.type));
		}
	}
}
