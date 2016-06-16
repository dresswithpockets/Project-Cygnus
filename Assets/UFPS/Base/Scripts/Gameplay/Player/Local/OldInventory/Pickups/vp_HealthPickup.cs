/////////////////////////////////////////////////////////////////////////////////
//
//	vp_HealthPickup.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	a simple script for adding health to the player
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class vp_HealthPickup : vp_Pickup
{

	public float Health = 1.0f;

	/// <summary>
	/// tries to add 'Health' to the player
	/// </summary>
	protected override bool TryGive(vp_FPPlayerEventHandler player)
	{

		if (player.Health.Get() < 0.0f)
			return false;

		if (player.Health.Get() >= player.MaxHealth.Get())
			return false;

		player.Health.Set(Mathf.Min(player.MaxHealth.Get(), (player.Health.Get() + Health)));

		return true;

	}

}
