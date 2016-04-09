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

	internal Vector3 m_Home = new Vector3(0f, 0f, 0f);
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

	void Start()
	{
		Spawn(transform.position); 
	}

	void Update()
	{
		if (Alive)
		{

		}

		GameController.InvokePlayerUpdate();
	}

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
			case DamageType.Magic:
				if (MagicResistance > 0f || Mathf.Approximately(MagicResistance, 0f))
				{
					dmgMult = 100 / (100 + MagicResistance);
				}
				else
				{
					dmgMult = 2f - (100 / (100 - MagicResistance));
				}
				break;
			case DamageType.Physical:
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
		DoDamage(new Damage(Health + 1f, killer, DamageType.Pure));// +1 ensures that the damage done is more than the health the player has.
	}

	public float GetStat(Stat stat)
	{
		switch (stat)
		{
			case Stat.Armor:
				return Armor;
			case Stat.Crit:
				return CritChance;
			case Stat.HP:
				return MaxHealth;
			case Stat.Reg:
				return HealthRegen;
			case Stat.Resi:
				return MagicResistance;
			case Stat.Tempo:
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
