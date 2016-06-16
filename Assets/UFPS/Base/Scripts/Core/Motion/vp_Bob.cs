/////////////////////////////////////////////////////////////////////////////////
//
//	vp_Bob.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	this script will move its gameobject in a wavy (sinus / bob)
//					motion. NOTE: this script currently can not be used on items
//					that spawn from vp_SpawnPoints
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class vp_Bob : MonoBehaviour
{

	public Vector3 BobAmp = new Vector3(0.0f, 0.1f, 0.0f);		// wave motion strength
	public Vector3 BobRate = new Vector3(0.0f, 4.0f, 0.0f);		// wave motion speed
	public float BobOffset = 0.0f;								// bob offset (can be used to make pickups in a row move independently)
	public float GroundOffset = 0.0f;							// TIP: increase this to avoid ground intersection
	public bool RandomizeBobOffset = false;
	public bool LocalMotion = false;

	protected Transform m_Transform;
	protected Vector3 m_InitialPosition;
	protected Vector3 m_Offset;


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Awake()
	{
		m_Transform = transform;
		m_InitialPosition = m_Transform.position;
	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnEnable()
	{

		m_Transform.position = m_InitialPosition;
		if (RandomizeBobOffset)
			BobOffset = Random.value;

	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Update()
	{

		if ((BobRate.x != 0.0f) && (BobAmp.x != 0.0f))
			m_Offset.x = vp_MathUtility.Sinus(BobRate.x, BobAmp.x, BobOffset);

		if ((BobRate.y != 0.0f) && (BobAmp.y != 0.0f))
			m_Offset.y = vp_MathUtility.Sinus(BobRate.y, BobAmp.y, BobOffset);

		if ((BobRate.z != 0.0f) && (BobAmp.z != 0.0f))
			m_Offset.z = vp_MathUtility.Sinus(BobRate.z, BobAmp.z, BobOffset);

		if (!LocalMotion)
			m_Transform.position = (m_InitialPosition + m_Offset) + (Vector3.up * GroundOffset);
		else
		{
			m_Transform.position = m_InitialPosition + (Vector3.up * GroundOffset);
			m_Transform.localPosition += m_Transform.TransformDirection(m_Offset);
		}

	}



}