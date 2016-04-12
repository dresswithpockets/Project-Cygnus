using UnityEngine;
using System.Collections;

public abstract class PlayerEventTemplate {

	internal GameObject m_Owner;
	public GameObject Owner
	{
		get
		{
			return m_Owner;
		}
	}

	internal PlayerController m_Player;
	public PlayerController Player
	{
		get
		{
			return m_Player;
		}
	}

	internal InventoryController m_PlayerInventory;
	public InventoryController PlayerInventory
	{
		get
		{
			return m_PlayerInventory;
		}
	}

	public virtual void Spawned()
	{

	}

	public virtual void Died(Entity attacker)
	{

	}

	public virtual void Damaged(Damage damage)
	{

	}

	public virtual void Moved(Vector3 position, Vector3 delta)
	{

	}

	public virtual void Jumped()
	{

	}

	public virtual void Landed()
	{

	}

	public virtual void Update()
	{

	}

	public virtual void FixedUpdate()
	{

	}

	public virtual void LateUpdate()
	{

	}

	public virtual void UsedItem(Item item)
	{

	}

	/* Items can no longer be added to hotbar.
	public virtual void AddedItemToHotBar(Item item, int slot)
	{

	}
	*/

	public virtual void DroppedItem(Item item)
	{

	}

	public virtual void PickedUpItem(Item item)
	{

	}

	public virtual void CraftedItem(Item item)
	{

	}

	public virtual void UsedWeapon(Weapon item)
	{

	}

	public virtual void EquippedWeapon(Weapon weapon, int slot)
	{

	}

	public virtual void UnequippedWeapon(Weapon weapon)
	{

	}

	public virtual void PickedUpWeapon(Weapon weapon)
	{

	}

	public virtual void DroppedWeapon(Weapon weapon)
	{

	}

	public virtual void CraftedWeapon(Weapon weapon)
	{

	}

	public virtual void EquippedEquipment(Equipment equipment, int slot)
	{

	}

	public virtual void UnequippedEquipment(Equipment equipment)
	{

	}

	public virtual void PickedUpEquipment(Equipment equipment)
	{

	}

	public virtual void DroppedEquipment(Equipment equipment)
	{

	}

	public virtual void CraftedEquipment(Equipment equipment)
	{

	}

	public virtual void UsedAbility(Ability_Template ability)
	{

	}

	public virtual void LearnedAbility(Ability_Template ability, int level)
	{

	}

	public virtual void DefeatedOther(Entity other)
	{

	}
}
