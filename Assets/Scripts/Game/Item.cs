using UnityEngine;
using System;
using System.Collections;

public class Item : MonoBehaviour
{
	internal ItemTemplate m_Template = null;
	public ItemTemplate Template
	{
		get
		{
			return m_Template;
		}
		internal set
		{
			m_Template.m_Owner = gameObject;
			m_Template.m_Item = this;
		}
	}

	internal MeshFilter Filter;

	internal bool BeingUsed = false;
	internal bool UsageQueued = false;

	#region Ownership
	
	internal NPC m_NPCOwner = null;
	internal PlayerController m_PlayerOwner = null;
	

	public NPC NPCOwner
	{
		get
		{
			return m_NPCOwner;
		}
	}

	public PlayerController PlayerOwner
	{
		get
		{
			return m_PlayerOwner;
		}
	}

	public ItemOwner Ownership
	{
		get
		{
			return (m_NPCOwner == null ? (m_PlayerOwner == null ? ItemOwner.None : ItemOwner.Player) : ItemOwner.NPC);
		}
	}

	internal void SetOwner(PlayerController player)
	{
		m_NPCOwner = null;
		m_PlayerOwner = player;
	}

	internal void SetOwner(NPC npc)
	{
		m_PlayerOwner = null;
		m_NPCOwner = npc;
	}

	#endregion

	void Start()
	{
		Filter = GetComponent<MeshFilter>();
		Template.Spawned();
	}

	void Update()
	{
		Template.ExistsUpdate();

		switch (Ownership)
		{
			case ItemOwner.NPC:

				Template.PassiveUpdate(m_NPCOwner);

				/* Items can no longer be used.
				if (BeingUsed && Template.IsReady() && Template.AICanUseOnTarget(m_NPCOwner, (Entity)m_NPCTarget))
				{
					Template.ActiveUpdate(m_NPCOwner);

					if (Template.IsFinished())
					{
						Template.End(m_NPCOwner);
						BeingUsed = false;
					}
				}
				else if (UsageQueued && Template.AICanUseOnTarget(m_NPCOwner, (Entity)m_NPCTarget))
				{
					Template.Start(m_NPCOwner);
					BeingUsed = true;
					UsageQueued = false;
				}
				*/

				break;
			case ItemOwner.Player:

				Template.PassiveUpdate(m_PlayerOwner);

				/* Items can no longer be used.
				if (BeingUsed && Template.IsReady())
				{
					Template.ActiveUpdate(m_PlayerOwner);

					if (Template.IsFinished())
					{
						Template.End(m_PlayerOwner);
						BeingUsed = false;
					}
				}
				else if (UsageQueued)
				{
					Template.Start(m_PlayerOwner);
					BeingUsed = true;
					UsageQueued = false;
				}
				*/

				break;
		}
	}

	void FixedUpdate()
	{
		Template.FixedUpdate();
	}

	void LateUpdate()
	{
		Template.LateUpdate();
	}

	public void Use()
	{
		if (Ownership == ItemOwner.None)
		{
			Debug.LogError("Cannot use item because no owner was assigned before the event was complete.", this);
			return;
		}
		UsageQueued = true;
	}

	public void PickUp()
	{
		switch (Ownership)
		{
			case ItemOwner.None:
				Debug.LogError("Cannot pick up item because no owner was assigned before the event was completed.", this);
				break;
			case ItemOwner.NPC:
				Template.PickedUp(m_NPCOwner);
				break;
			case ItemOwner.Player:
				Template.PickedUp(m_PlayerOwner);
				break;
		}
	}

	public void Drop()
	{
		switch (Ownership)
		{
			case ItemOwner.None:
				Debug.LogError("Cannot drop item as no NPC or Player owns this item.", this);
				break;
			case ItemOwner.NPC:
				Template.Dropped(m_NPCOwner);
				break;
			case ItemOwner.Player:
				Template.Dropped(m_PlayerOwner);
				break;
		}
	}

	/* Items can no longer be used.
	public void Inturrupt()
	{
		Template.Inturrupt();
	}
	*/

	/* Items can no longer be added to hotbar.
	public void AddToHotBar(int slot)
	{
		Template.AddedToHotBar(slot);
	}
	*/

	public void AssignTemplate(ItemTemplate itemTemplate, MeshFilter model)
	{
		Template = itemTemplate;
		Filter.mesh = model.sharedMesh;
	}
}
