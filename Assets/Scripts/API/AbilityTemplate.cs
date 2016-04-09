using UnityEngine;
using System.Collections;
using System;

public abstract class AbilityTemplate  {

	// Displayed name of the ability
	public abstract string AbilityName
	{
		get;
	}

	// Displayed RichText description of the ability
	public abstract string AbilityDescription
	{
		get;
	}

	// Whether or not AI can learn and use this ability
	public abstract bool UsedByAI
	{
		get;
	}

	// The ID of the image that will be used as an avatar for the ability
	public abstract string AvatarID
	{
		get;
	}
	
	public abstract int AbilityLevel
	{
		get;
	}

	public virtual void Start(PlayerController player)
	{

	}

	public virtual void Start(NPC npc)
	{

	}

	public virtual void PassiveUpdate(PlayerController player)
	{

	}

	public virtual void PassiveUpdate(NPC npc)
	{

	}

	public virtual void ActiveUpdate(PlayerController player)
	{

	}

	public virtual void ActiveUpdate(NPC npc)
	{

	}

	public virtual void End(PlayerController player)
	{

	}

	public virtual void End(NPC npc)
	{

	}

	public virtual bool IsReady()
	{
		return true;
	}

	public virtual bool IsFinished()
	{
		return true;
	}

	public virtual void LevelUp()
	{

	}

	public virtual void Inturrupt()
	{

	}

	// Determines whether or not an AI can use this ability on a target
	public virtual bool AICanUseOnTarget(NPC npc, GameObject target)
	{
		return false;
	}
}