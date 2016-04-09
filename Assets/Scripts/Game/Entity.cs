using UnityEngine;
using System.Collections;

public class Entity
{
	internal GameObject m_Object = null;
	public GameObject Object
	{
		get
		{
			return m_Object;
		}
		internal set
		{
			m_Object = value;
		}
	}

	public Vector3 Position
	{
		get
		{
			return m_Object.transform.position;
		}
		set
		{
			m_Object.transform.position = value;
		}
	}

	public Vector3 Rotation
	{
		get
		{
			return m_Object.transform.eulerAngles;
		}
		set
		{
			m_Object.transform.eulerAngles = value;
		}
	}

	public bool IsPlayer
	{
		get
		{
			return (m_Object.GetComponent<PlayerController>() != null);
		}
	}

	public bool IsNPC
	{
		get
		{
			return (m_Object.GetComponent<NPC>() != null);
		}
	}

	public PlayerController GetPlayer()
	{
		return m_Object.GetComponent<PlayerController>();
	}

	public NPC GetNPC()
	{
		return m_Object.GetComponent<NPC>();
	}

	public void DoDamage(Damage damage)
	{
		m_Object.SendMessage("DoDamage", damage);
	}

	public static explicit operator Entity(GameObject go)
	{
		return new Entity()
		{
			m_Object = go
		};
	}

	public static explicit operator GameObject(Entity ent)
	{
		return ent.m_Object;
	}
}

public class GameEntity : Entity
{

	public virtual void Awake()
	{

	}

	public virtual void FixedUpdate()
	{

	}

	public virtual void LateUpdate()
	{

	}

	public virtual void OnCollisionEnter(Collision other)
	{

	}

	public virtual void OnCollisionExit(Collision other)
	{

	}

	public virtual void OnCollisionStay(Collision other)
	{

	}

	public virtual void OnDestroy()
	{

	}

	public virtual void OnGUI()
	{

	}

	public virtual void OnTriggerEnter(Collider other)
	{

	}

	public virtual void OnTriggerExit(Collider other)
	{

	}

	public virtual void OnTriggerStay(Collider other)
	{

	}

	public virtual void Start()
	{

	}

	public virtual void Update()
	{

	}

	public void Instantiate(Vector3 position, Vector3 rotation)
	{
		if (m_Object == null)
		{
			m_Object = (GameObject)GameObject.Instantiate(GameController.Instance.EntityPrefab, position, Quaternion.Euler(rotation));
			return;
		}

		Debug.LogError("Attempted to instantiate an entity as a Unity GameObject that has already been instantiated in the scene.", (GameObject)this);
	}
}