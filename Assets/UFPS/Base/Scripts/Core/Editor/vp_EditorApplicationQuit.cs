/////////////////////////////////////////////////////////////////////////////////
//
//	vp_EditorApplicationQuit.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	this is a small convenience feature allowing the game to quit
//					itself when running in the editor by means of vp_GlobalEvent
//					"EditorApplicationQuit"
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class vp_EditorApplicationQuit : Editor
{

	static vp_EditorApplicationQuit()
	{
		vp_GlobalEvent.Register("EditorApplicationQuit", () => { EditorApplication.isPlaying = false; });
	}
			
}

