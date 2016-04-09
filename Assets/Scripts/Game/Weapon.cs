using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{

	internal WeaponTemplate m_Template = null;
	public WeaponTemplate Template
	{
		get
		{
			return m_Template;
		}
		internal set
		{
			m_Template.m_Owner = gameObject;
			m_Template.m_Weapon = this;
		}
	}

	internal MeshFilter Filter;

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

				break;
			case ItemOwner.Player:

				Template.PassiveUpdate(m_PlayerOwner);

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

	void Use()
	{
		switch (Ownership)
		{
			case ItemOwner.None:
				Debug.LogError("Cannot use weapon because no owner was assigned before the event was completed.", this);
				break;
			case ItemOwner.NPC:
				Template.Used(m_NPCOwner);
				break;
			case ItemOwner.Player:
				Template.Used(m_PlayerOwner);
				break;
		}
	}

	public void PickUp()
	{
		switch (Ownership)
		{
			case ItemOwner.None:
				Debug.LogError("Cannot pick up weapon because no owner was assigned before the event was completed.", this);
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
				Debug.LogError("Cannot drop weapon as no NPC or Player owns this item.", this);
				break;
			case ItemOwner.NPC:
				Template.Dropped(m_NPCOwner);
				break;
			case ItemOwner.Player:
				Template.Dropped(m_PlayerOwner);
				break;
		}
	}

	public void Equip(int slot)
	{
		switch (Ownership)
		{
			case ItemOwner.None:
				Debug.LogError("Cannot equip weapon as no NPC or Player owns this item.", this);
				break;
			case ItemOwner.NPC:
				Template.Equipped(m_NPCOwner, slot);
				break;
			case ItemOwner.Player:
				Template.Equipped(m_PlayerOwner, slot);
				break;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (Template.MeleeInUse())
		{
			switch (other.tag)
			{
				case "Player":
					Template.MeleeAttackHit(m_PlayerOwner, (Entity)other.gameObject);
					break;
				case "NPC":
					Template.MeleeAttackHit(m_NPCOwner, (Entity)other.gameObject);
					break;
			}
		}
	}

	public void AssignTemplate(WeaponTemplate weaponTemplate, MeshFilter model)
	{
		Template = weaponTemplate;
		Filter.mesh = model.sharedMesh;
	}
}
