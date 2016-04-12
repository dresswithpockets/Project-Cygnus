using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour {

	internal Equipment_Template m_Template = null;
	public Equipment_Template Template
	{
		get
		{
			return m_Template;
		}
		internal set
		{
			m_Template.owner = gameObject;
			m_Template.equipment_object = this;
		}
	}

	internal MeshFilter Filter;

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

	public Item_Owner Ownership
	{
		get
		{
			return (m_NPCOwner == null ? (m_PlayerOwner == null ? Item_Owner.NONE : Item_Owner.PLAYER) : Item_Owner.NPC);
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
		Template.spawned();
	}

	void Update()
	{
		Template.exists_update();

		switch (Ownership)
		{
			case Item_Owner.NPC:

				Template.passive_update(m_NPCOwner);

				break;
			case Item_Owner.PLAYER:

				Template.passive_update(m_PlayerOwner);

				break;
		}
	}

	void FixedUpdate()
	{
		Template.fixed_update();
	}

	void LateUpdate()
	{
		Template.late_update();
	}

	public void PickUp()
	{
		switch (Ownership)
		{
			case Item_Owner.NONE:
				Debug.LogError("Cannot pick up weapon because no owner was assigned before the event was completed.", this);
				break;
			case Item_Owner.NPC:
				Template.picked_up(m_NPCOwner);
				break;
			case Item_Owner.PLAYER:
				Template.picked_up(m_PlayerOwner);
				break;
		}
	}

	public void Drop()
	{
		switch (Ownership)
		{
			case Item_Owner.NONE:
				Debug.LogError("Cannot drop weapon as no NPC or Player owns this item.", this);
				break;
			case Item_Owner.NPC:
				Template.dropped(m_NPCOwner);
				break;
			case Item_Owner.PLAYER:
				Template.dropped(m_PlayerOwner);
				break;
		}
	}

	public void Equip(int slot)
	{
		switch (Ownership)
		{
			case Item_Owner.NONE:
				Debug.LogError("Cannot equip weapon as no NPC or Player owns this item.", this);
				break;
			case Item_Owner.NPC:
				Template.equipped(m_NPCOwner, slot);
				break;
			case Item_Owner.PLAYER:
				Template.equipped(m_PlayerOwner, slot);
				break;
		}
	}

	public void AssignTemplate(Equipment_Template equipmentTemplate, MeshFilter model)
	{
		Template = equipmentTemplate;
		Filter.mesh = model.sharedMesh;
	}
}
