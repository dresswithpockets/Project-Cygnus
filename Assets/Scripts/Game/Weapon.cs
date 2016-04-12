using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{

	internal Weapon_Template m_template = null;
	public Weapon_Template template
	{
		get
		{
			return m_template;
		}
		internal set
		{
			m_template.m_Owner = gameObject;
			m_template.m_Weapon = this;
		}
	}

	internal MeshFilter mesh_fliter;

	internal bool BeingUsed = false;
	internal bool UsageQueued = false;

	#region Ownership

	internal GameObject m_NPCTarget = null;
	internal NPC m_NPCOwner = null;
	internal PlayerController m_PlayerOwner = null;

	public GameObject NPCTarget
	{
		get
		{
			return m_NPCTarget;
		}
	}

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

	public void SetNPCTarget(GameObject target)
	{
		if (m_NPCOwner == null)
		{
			Debug.LogError("Cannot set item target because this item is not owned by an NPC.");
			return;
		}

		m_NPCTarget = target;
	}

	#endregion
	
	void Start()
	{
		mesh_fliter = GetComponent<MeshFilter>();
		template.Spawned();
	}

	void Update()
	{
		template.ExistsUpdate();

		switch (Ownership)
		{
			case Item_Owner.NPC:

				template.PassiveUpdate(m_NPCOwner);

				break;
			case Item_Owner.PLAYER:

				template.PassiveUpdate(m_PlayerOwner);

				break;
		}
	}

	void FixedUpdate()
	{
		template.FixedUpdate();
	}

	void LateUpdate()
	{
		template.LateUpdate();
	}

	void Use()
	{
		switch (Ownership)
		{
			case Item_Owner.NONE:
				Debug.LogError("Cannot use weapon because no owner was assigned before the event was completed.", this);
				break;
			case Item_Owner.NPC:
				template.Used(m_NPCOwner);
				break;
			case Item_Owner.PLAYER:
				template.Used(m_PlayerOwner);
				break;
		}
	}

	public void PickUp()
	{
		switch (Ownership)
		{
			case Item_Owner.NONE:
				Debug.LogError("Cannot pick up weapon because no owner was assigned before the event was completed.", this);
				break;
			case Item_Owner.NPC:
				template.PickedUp(m_NPCOwner);
				break;
			case Item_Owner.PLAYER:
				template.PickedUp(m_PlayerOwner);
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
				template.Dropped(m_NPCOwner);
				break;
			case Item_Owner.PLAYER:
				template.Dropped(m_PlayerOwner);
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
				template.Equipped(m_NPCOwner, slot);
				break;
			case Item_Owner.PLAYER:
				template.Equipped(m_PlayerOwner, slot);
				break;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (template.MeleeInUse())
		{
			switch (other.tag)
			{
				case "Player":
					template.MeleeAttackHit(m_PlayerOwner, (Entity)other.gameObject);
					break;
				case "NPC":
					template.MeleeAttackHit(m_NPCOwner, (Entity)other.gameObject);
					break;
			}
		}
	}

	public void AssignTemplate(Weapon_Template weaponTemplate, MeshFilter model)
	{
		template = weaponTemplate;
		mesh_fliter.mesh = model.sharedMesh;
	}
}
