/////////////////////////////////////////////////////////////////////////////////
//
//	vp_Spin.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	this component will make its gameobject spin continuously
//					around a set vector / speed
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vp_Spin : MonoBehaviour
{

	public Vector3 RotationSpeed = new Vector3(0, 90, 0);
	Transform m_Transform;

#if UNITY_EDITOR
		[vp_HelpBox("This script rotates a gameobject at a constant speed. The 'Rotation Speed' vector determines torque around each axis.", UnityEditor.MessageType.None, typeof(vp_Spin), null, true)]
		public float helpbox;
		[vp_Separator]
		public float separator;
#endif


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Start()
	{
		m_Transform = transform;
	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Update()
	{
		m_Transform.Rotate(RotationSpeed * Time.deltaTime);
	}


}