/////////////////////////////////////////////////////////////////////////////////
//
//	vp_Billboard.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	this script will make its gameobject always face the camera
//
///////////////////////////////////////////////////////////////////////////////// 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vp_Billboard : MonoBehaviour
{

	public Transform m_CameraTransform = null;
	Transform m_Transform = null;

	/// <summary>
	/// 
	/// </summary>
	protected virtual void Start()
	{

		m_Transform = transform;
		if (m_CameraTransform == null)
			m_CameraTransform = Camera.main.transform;

	}

	/// <summary>
	/// 
	/// </summary>
	protected virtual void Update()
	{

		if (m_CameraTransform != null)
			m_Transform.localEulerAngles = m_CameraTransform.eulerAngles;

		m_Transform.localEulerAngles = (Vector2)m_Transform.localEulerAngles;

	}

}