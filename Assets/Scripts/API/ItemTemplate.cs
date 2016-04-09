using UnityEngine;
using System.Collections;

public abstract class ItemTemplate {

	internal GameObject m_Owner;
	public GameObject Owner
	{
		get
		{
			return m_Owner;
		}
	}

	internal Item m_Item;
	public Item Item
	{
		get
		{
			return m_Item;
		}
	}

	public abstract string ItemName
	{
		get;
	}

	public abstract string ItemDescription
	{
		get;
	}

	public abstract bool UsedByAI
	{
		get;
	}

	public abstract ItemClassification Classification
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
