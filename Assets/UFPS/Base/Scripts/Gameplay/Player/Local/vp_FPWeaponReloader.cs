﻿/////////////////////////////////////////////////////////////////////////////////
//
//	vp_FPWeaponReloader.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	this component adds firearm reload logic, sound, animation and
//					reload duration to an FPWeapon. it doesn't handle ammo max caps
//					or levels. instead this should be governed by an inventory
//					system via the event handler
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;


[RequireComponent(typeof(vp_FPWeapon))]

public class vp_FPWeaponReloader : vp_WeaponReloader
{

	public AnimationClip AnimationReload = null;
	

	/// <summary>
	/// this callback is triggered right after the 'Reload' activity
	/// has been approved for activation
	/// </summary>
	protected override void OnStart_Reload()
	{

		base.OnStart_Reload();

		if (AnimationReload == null)
			return;

		// if reload duration is zero, fetch duration from the animation
		if (m_Player.Reload.AutoDuration == 0.0f)
			m_Player.Reload.AutoDuration = AnimationReload.length;

		((vp_FPWeapon)m_Weapon).WeaponModel.GetComponent<Animation>().CrossFade(AnimationReload.name);

	}
	

}

