using UnityEngine;
using System.Collections.Generic;

public abstract class EquipmentTemplate  {

	internal GameObject m_Owner;
	public GameObject Owner
	{
		get
		{
			return m_Owner;
		}
	}

	internal Equipment m_Equipment;
	public Equipment Equipment
	{
		get
		{
			return m_Equipment;
		}
	}

	public abstract string EquipmentName
	{
		get;
	}

	public abstract string EquipmentDescription
	{
		get;
	}

	public abstract bool UsedByAI
	{
		get;
	}

	public abstract EquipmentClassification Classification
	{
		get;
	}

	public abstract List<StatModifier> EquippedStatModifiers
	{
		get;
	}

	public abstract string ModelID
	{
		get;
	}

	public virtual void Spawned()
	{

	}

	public virtual void ExistsUpdate()
	{

	}

	public virtual void PassiveUpdate(PlayerController player)
	{

	}

	public virtual void PassiveUpdate(NPC npc)
	{

	}

	public virtual void FixedUpdate()
	{

	}

	public virtual void LateUpdate()
	{

	}

	public virtual void PickedUp(PlayerController player)
	{

	}

	public virtual void PickedUp(NPC npc)
	{

	}

	public virtual void Dropped(PlayerController player)
	{

	}

	public virtual void Dropped(NPC npc)
	{

	}

	public virtual void Equipped(PlayerController player, int slot)
	{

	}

	public virtual void Equipped(NPC npc, int slot)
	{

	}
}
