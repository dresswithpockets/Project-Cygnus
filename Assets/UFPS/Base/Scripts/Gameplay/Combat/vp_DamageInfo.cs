/////////////////////////////////////////////////////////////////////////////////
//
//	vp_DamageInfo.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	carries information about a single instance of damage done,
//					typically to a vp_DamageHandler-derived component. this class
//					is a long term work in progress
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class vp_DamageInfo
{
	public float Damage;				// how much damage was done?
	public Transform Source;			// from what object did it come (directly)? common use: HUD / GUI
	public Transform OriginalSource;	// what object initially caused this to happen? common use: game logic, score
	public DamageType Type;				// what type of damage is this?

	public enum DamageType
	{
		Unknown,
		KillZone,
		Fall,
		Impact,
		Bullet,
		Explosion,
		// the above are the types represented in the UFPS demo but can be easily
		// extended: e.g. blunt, electrical, cutting, piercing, freezing, crushing
		// drowning, gas, acid, freezing, burning, scolding, magical, plasma etc.
	}

	/// <summary>
	/// 
	/// </summary>
	public vp_DamageInfo(float damage, Transform source, DamageType type = DamageType.Unknown)
	{
		Damage = damage;
		Source = source;
		OriginalSource = source;
		Type = type;
	}


	/// <summary>
	/// 
	/// </summary>
	public vp_DamageInfo(float damage, Transform source, Transform originalSource, DamageType type = DamageType.Unknown)
	{
		Damage = damage;
		Source = source;
		OriginalSource = originalSource;
		Type = type;
	}


}

