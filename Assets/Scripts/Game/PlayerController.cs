using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
	#region Stats

	internal bool m_Alive = false;
	internal float m_Health = 0f;

	internal float m_Armor = 0f;
	internal float m_MagicResistance = 0f; // Resi stat
	internal float m_CritChance = 0f; // Crit stat
	internal float m_MaxHealth = 100f; // HP Stat
	internal float m_MaxHealthDefault = 100f;
	internal float m_HealthRegen = 0f; // Reg stat
	internal float m_AttackTempo = 1.0f; // Tempo stat

	public float Health
	{
		get
		{
			return m_Health;
		}
		internal set
		{
			m_Health = value;
			if (m_Health < 0f || Mathf.Approximately(m_Health, 0f))
			{
				m_Alive = false;
				m_Health = 0f;
			}
			else
			{
				m_Alive = true;
			}
		}
	}

	public bool Alive
	{
		get
		{
			return m_Alive;
		}
		internal set
		{
			m_Alive = value;
		}
	}

	public float Armor
	{
		get
		{
			return m_Armor;
		}
		internal set
		{
			m_Armor = value;
		}
	}

	public float MagicResistance
	{
		get
		{
			return m_MagicResistance;
		}
		internal set
		{
			m_MagicResistance = value;
		}
	}

	public float CritChance
	{
		get
		{
			return m_CritChance;
		}
		internal set
		{
			m_CritChance = value;
		}
	}

	public float MaxHealth
	{
		get
		{
			return m_MaxHealth;
		}
		internal set
		{
			m_MaxHealth = value;
		}
	}

	public float HealthRegen
	{
		get
		{
			return m_HealthRegen;
		}
		internal set
		{
			m_HealthRegen = value;
		}
	}

	public float AttackTempo
	{
		get
		{
			return m_AttackTempo;
		}
		internal set
		{
			m_AttackTempo = value;
		}
	}

	#endregion
	
	public GameObject m_OrbitalCamera = null;
	internal vp_FPController m_FPController = null;
	internal InventoryController m_Inventory;

	internal Vector3 m_Home = new Vector3(0f, 0f, 0f);

	public GameObject OrbitalCamera
	{
		get
		{
			return m_OrbitalCamera;
		}
		internal set
		{
			m_OrbitalCamera = value;
		}
	}

	public Vector3 Home
	{
		get
		{
			return m_Home;
		}
		internal set
		{
			m_Home = value;
		}
	}

	public vp_FPController FPController
	{
		get
		{
			return m_FPController;
		}
		internal set
		{
			m_FPController = value;
		}
	}

	public InventoryController Inventory
	{
		get
		{
			return m_Inventory;
		}
		internal set
		{
			m_Inventory = value;
		}
	}

	public Vector3 PlayerVelocity
	{
		get
		{
			return FPController.m_Velocity;
		}
	}

	public bool Moving
	{
		get
		{
			return PlayerVelocity != Vector3.zero;
		}
	}

	void Start()
	{
		OrbitalCamera = GetComponentInChildren<UltimateOrbitCamera>().gameObject;
		FPController = GetComponent<vp_FPController>();
		Inventory = GetComponent<InventoryController>();
		Spawn(transform.position);
	}

	#region Update

	void Update()
	{
		if (Alive)
		{
			UpdateMovement();

			UpdateRotation();
		}

		GameController.InvokePlayerUpdate();
	}

	void UpdateMovement()
	{
		if (Moving)
		{
			GameController.InvokePlayerMoved(transform.position, PlayerVelocity * Time.deltaTime);
		}
	}

	void UpdateRotation()
	{
		if (Moving || Inventory.CastingAbility) //TODO: Or attacking w/ weapon (mouse 1 or mouse 2 by default)
		{
			Vector3 oldCamPos = OrbitalCamera.transform.position;
			Quaternion oldCamRot = OrbitalCamera.transform.rotation;

			Vector3 newRot = new Vector3(0f, 0f, 0f);
			newRot.y = OrbitalCamera.transform.eulerAngles.y;
			transform.eulerAngles = newRot;

			//Fix rotation of camera as it is a child.
			m_OrbitalCamera.transform.position = oldCamPos;
			m_OrbitalCamera.transform.rotation = oldCamRot;
		}
	}

	#endregion

	void FixedUpdate()
	{
		GameController.InvokePlayerFixedUpdate();
	}

	void LateUpdate()
	{
		GameController.InvokePlayerLateUpdate();
	}

	public void Spawn(Vector3 position)
	{
		transform.position = position;
		Health = MaxHealth;
		GameController.InvokePlayerSpawned();
	}

	public void Spawn(bool atStatue = false)
	{
		if (atStatue)
		{
			Vector3 closestStatue = Home;
			float closestDistance = Mathf.Infinity;

			Statue[] statues = FindObjectsOfType<Statue>();
			foreach (Statue statue in statues)
			{
				Vector3 directionToStatue = statue.transform.position - transform.position;
				float sqrDistance = directionToStatue.sqrMagnitude;
				if (sqrDistance < closestDistance)
				{
					closestDistance = sqrDistance;
					closestStatue = statue.transform.position;
				}
			}

			Spawn(closestStatue);
			return;
		}

		Spawn(Home);
	}

	public void SetHome(Vector3 position)
	{
		// TODO: Do checks to make sure position is a valid position in world space.
		Home = position;
	}

	public void DoDamage(Damage damage)
	{
		float dmgMult = 1f;
		switch (damage.damageType)
		{
			case Damage_Type.MAGIC:
				if (MagicResistance > 0f || Mathf.Approximately(MagicResistance, 0f))
				{
					dmgMult = 100 / (100 + MagicResistance);
				}
				else
				{
					dmgMult = 2f - (100 / (100 - MagicResistance));
				}
				break;
			case Damage_Type.PHYSICAL:
				if (Armor > 0f || Mathf.Approximately(Armor, 0f))
				{
					dmgMult = 100 / (100 + Armor);
				}
				else
				{
					dmgMult = 2f - (100 / (100 - Armor));
				}
				break;
		}

		Health -= damage.damage * dmgMult;
		GameController.InvokePlayerDamaged(damage);
		if (!Alive)
		{
			GameController.InvokePlayerDied(damage.attacker);
		}
	}

	public void Kill(Entity killer)
	{
		DoDamage(new Damage(Health + 1f, killer, Damage_Type.PURE));// +1 ensures that the damage done is more than the health the player has.
	}

	public float GetStat(Char_Stat stat)
	{
		switch (stat)
		{
			case Char_Stat.ARMOR:
				return Armor;
			case Char_Stat.CRIT:
				return CritChance;
			case Char_Stat.HP:
				return MaxHealth;
			case Char_Stat.REG:
				return HealthRegen;
			case Char_Stat.RESI:
				return MagicResistance;
			case Char_Stat.TEMPO:
				return AttackTempo;
		}

		return 0f;
	}

	public void UpdateStats()
	{
		//TODO: Update stats based on weapons and inventory.
	}

	#region Construction and Singleton

	private static PlayerController m_Instance = null;
	public static PlayerController Instance
	{
		get
		{
			return m_Instance;
		}
	}

	void Awake()
	{
		if (m_Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		m_Instance = this;
	}

	#endregion
}
