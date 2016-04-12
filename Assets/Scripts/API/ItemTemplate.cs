using UnityEngine;
using System.Collections;

public abstract class Item_Template {

	private GameObject m_owner;
	public GameObject owner
	{
		get
		{
			return m_owner;
		}
		internal set
		{
			m_owner = value;
		}
	}

	private Item m_item;
	public Item item
	{
		get
		{
			return m_item;
		}
		internal set
		{
			m_item = value;
		}
	}

	public abstract string name
	{
		get;
	}

	public abstract string description
	{
		get;
	}

	public abstract bool used_by_AI
	{
		get;
	}

	public abstract Item_Classification classification
	{
		get;
	}

	public abstract string model_ID
	{
		get;
	}

	public virtual void spanwed()
	{

	}

	public virtual void exists_update()
	{

	}

	public virtual void passive_update(PlayerController player)
	{

	}

	public virtual void passive_update(NPC npc)
	{

	}

	/* Items can no longer be used.
	public virtual void Start(PlayerController player)
	{

	}

	public virtual void Start(NPC npc)
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

	public virtual bool AICanUseOnTarget(NPC other, Entity target)
	{
		return false;
	}

	public virtual void Inturrupt()
	{
		
	}
	*/

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

	/* Items can no longer be added to hotbar.
	public virtual void AddedToHotBar(int slot)
	{

	}
	*/
}
