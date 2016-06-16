﻿/////////////////////////////////////////////////////////////////////////////////
//
//	vp_MathUtility.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	miscellaneous math utility functions
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Diagnostics;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

public static class vp_MathUtility
{


	/// <summary>
	/// Cleans non numerical values (NaN) from a float by
	/// retaining a previous property value. If 'prevValue' is
	/// omitted, the NaN will be replaced by '0.0f'.
	/// </summary>
	public static float NaNSafeFloat(float value, float prevValue = default(float))
	{

		value = double.IsNaN(value) ? prevValue : value;
		return value;

	}


	/// <summary>
	/// Cleans non numerical values (NaN) from a Vector2 by
	/// retaining a previous property value. If 'prevVector' is
	/// omitted, the NaN will be replaced by '0.0f'.
	/// </summary>
	public static Vector2 NaNSafeVector2(Vector2 vector, Vector2 prevVector = default(Vector2))
	{

		vector.x = double.IsNaN(vector.x) ? prevVector.x : vector.x;
		vector.y = double.IsNaN(vector.y) ? prevVector.y : vector.y;

		return vector;

	}


	/// <summary>
	/// Cleans non numerical values (NaN) from a Vector3 by
	/// retaining a previous property value. If 'prevVector' is
	/// omitted, the NaN will be replaced by '0.0f'.
	/// </summary>
	public static Vector3 NaNSafeVector3(Vector3 vector, Vector3 prevVector = default(Vector3))
	{

		vector.x = double.IsNaN(vector.x) ? prevVector.x : vector.x;
		vector.y = double.IsNaN(vector.y) ? prevVector.y : vector.y;
		vector.z = double.IsNaN(vector.z) ? prevVector.z : vector.z;

		return vector;

	}


	/// <summary>
	/// Cleans non numerical values (NaN) from a Quaternion by
	/// retaining a previous property value. If 'prevQuaternion'
	/// is omitted, the NaN will be replaced by '0.0f'.
	/// </summary>
	public static Quaternion NaNSafeQuaternion(Quaternion quaternion, Quaternion prevQuaternion = default(Quaternion))
	{

		quaternion.x = double.IsNaN(quaternion.x) ? prevQuaternion.x : quaternion.x;
		quaternion.y = double.IsNaN(quaternion.y) ? prevQuaternion.y : quaternion.y;
		quaternion.z = double.IsNaN(quaternion.z) ? prevQuaternion.z : quaternion.z;
		quaternion.w = double.IsNaN(quaternion.w) ? prevQuaternion.w : quaternion.w;

		return quaternion;

	}


	/// <summary>
	/// This can be used to snap individual super-small property
	/// values to zero, for avoiding some floating point issues.
	/// </summary>
	public static Vector3 SnapToZero(Vector3 value, float epsilon = 0.0001f)
	{

		value.x = (Mathf.Abs(value.x) < epsilon) ? 0.0f : value.x;
		value.y = (Mathf.Abs(value.y) < epsilon) ? 0.0f : value.y;
		value.z = (Mathf.Abs(value.z) < epsilon) ? 0.0f : value.z;
		return value;

	}


	/// <summary>
	/// This can be used to snap individual super-small property
	/// values to zero, for avoiding some floating point issues.
	/// </summary>
	public static float SnapToZero(float value, float epsilon = 0.0001f)
	{

		value = (Mathf.Abs(value) < epsilon) ? 0.0f : value;
		return value;

	}


	/// <summary>
	/// Reduces the number of decimals of a floating point number.
	/// This can be used to solve floating point imprecision cases.
	/// 'factor' determines the amount of decimals. Default is 1000
	/// which results in 3 decimals.
	/// </summary>
	public static float ReduceDecimals(float value, float factor = 1000)
	{

		return Mathf.Round(value * factor) / factor;

	}



	/// <summary>
	/// returns true if the supplied integer is an odd value and false
	/// if it's an even value. this can be used to perform logic every
	/// other time something happens, or for every other iteration in
	/// a loop
	/// </summary>
	public static bool IsOdd(int val)
	{
		return (val % 2 != 0);
	}


	/// <summary>
	/// Returns the state of a bob wave motion with the specified
	/// parameters at the current 'Time.time'.
	/// </summary>
	public static float Sinus(float rate, float amp, float offset = 0.0f)
	{
		return (Mathf.Cos((Time.time + offset) * rate) * amp);
	}


}

