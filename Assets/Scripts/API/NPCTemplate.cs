using UnityEngine;
using System.Collections;

public abstract class NPCTemplate {

	internal GameObject m_Owner;
	public GameObject Owner
	{
		get
		{
			return m_Owner;
		}
	}

	internal NPC m_NPC;
	public NPC NPC
	{
		get
		{
			return m_NPC;
		}
	}

	public abstract string NPCName
	{
		get;
	}

	public abstract string NPCDescription
	{
		get;
	}

	public abstract int CurrentHealth
	{
		get;
	}

	public abstract int CurrentMaxHealth
	{
		get;
	}

	public abstract bool DisplayDefaultHealthbar
	{
		get;
	}

	public abstract CharacterController Character
	{
		get;
	}

	public virtual void Spawned()
	{

	}

	public virtual void Died(GameObject attacker)
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

	public virtual void DoDamage(Damage damage)
	{

	}

	public virtual void Goto(Vector3 location, bool pathfind = true)
	{
		//TODO: Implement default pathfind and goto functions.
	}
}
