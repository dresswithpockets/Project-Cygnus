/////////////////////////////////////////////////////////////////////////////////
//
//	vp_FPWeaponHandler.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	weapon handler logic that is specific to a local first person
//					player should be added to this script
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;


public class vp_FPWeaponHandler : vp_WeaponHandler
{
	

	/// <summary>
	/// 
	/// </summary>
	protected virtual bool OnAttempt_AutoReload()
	{

		if (!ReloadAutomatically)
			return false;

		if (CurrentWeapon.AnimationType == (int)vp_Weapon.Type.Melee)
			return false;

		return m_Player.Reload.TryStart();

	}


}


