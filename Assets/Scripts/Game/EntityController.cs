using UnityEngine;
using System.Collections;

public class EntityController : MonoBehaviour {

	internal GameEntity m_Entity = null;

	void Awake()
	{
		m_Entity.Awake();
	}

	void FixedUpdate()
	{
		m_Entity.FixedUpdate();
    }

	void LateUpdate()
	{
		m_Entity.LateUpdate();
    }

	void OnCollisionEnter(Collision other)
	{
		m_Entity.OnCollisionEnter(other);
    }

	void OnCollisionExit(Collision other)
	{
		m_Entity.OnCollisionExit(other);
	}

	void OnCollisionStay(Collision other)
	{
		m_Entity.OnCollisionStay(other);
    }

	void OnDestroy()
	{
		m_Entity.OnDestroy();
    }

	void OnGUI()
	{
		m_Entity.OnGUI();
    }

	void OnTriggerEnter(Collider other)
	{
		m_Entity.OnTriggerEnter(other);
    }

	void OnTriggerExit(Collider other)
	{
		m_Entity.OnTriggerExit(other);
    }

	void OnTriggerStay(Collider other)
	{
		m_Entity.OnTriggerStay(other);
    }

	void Start()
	{
		m_Entity.Start();
    }

	void Update()
	{
		m_Entity.Update();
    }
}
