/////////////////////////////////////////////////////////////////////////////////
//
//	vp_RigidbodyImpulse.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	this script will apply a rigidbody impulse to its gameobject
//					in the moment it awakes (spawns)
//
///////////////////////////////////////////////////////////////////////////////// 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]

public class vp_RigidbodyImpulse : MonoBehaviour
{

	protected Rigidbody m_Rigidbody = null;
	public Vector3 RigidbodyForce = new Vector3(0.0f, 5.0f, 0.0f);	// this force will be applied to the rigidbody when spawned
	public float RigidbodySpin = 0.2f;								// this much random torque will be applied to rigidbody when spawned


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnEnable()
	{
		if (m_Rigidbody == null)
			return;

		if (RigidbodyForce != Vector3.zero)
			m_Rigidbody.AddForce(RigidbodyForce, ForceMode.Impulse);
		if (RigidbodySpin != 0.0f)
			m_Rigidbody.AddTorque(Random.rotation.eulerAngles * RigidbodySpin);

	}


}