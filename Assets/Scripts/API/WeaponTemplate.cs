using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class Weapon_Template {

	internal GameObject m_Owner;
	public GameObject Owner
	{
		get
		{
			return m_Owner;
		}
	}
	
	internal Weapon m_Weapon;
	public Weapon Weapon
	{
		get
		{
			return m_Weapon;
		}
	}

	public abstract string WeaponName
	{
		get;
	}

	public abstract string WeaponDescription
	{
		get;
	}

	public abstract bool UsedByAI
	{
		get;
	}

	public abstract Weapon_Classification Classification
	{
		get;
	}

	public abstract Weapon_Handiness Handiness
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

	public virtual void Used(PlayerController player)
	{

	}

	public virtual void Used(NPC npc)
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

	//Used for AI, only called if the AI in question has a target and the item can be used by AI
	//See: UsedByAI
	public virtual bool AICanUseOnTarget(NPC npc, Entity target)
	{
		return false;
	}
}