using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vp_Gameplay
{

	public static bool isMultiplayer = false;
	protected static bool m_IsMaster = true;
	public static bool isMaster
	{
		get
		{
			if (!isMultiplayer)	// always true in singleplayer
				return true;
			return m_IsMaster;
		}
		set
		{
			if (!isMultiplayer)	// can't be set in singleplayer
				return;
			m_IsMaster = value;
		}
	}

}