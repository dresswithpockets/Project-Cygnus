﻿/////////////////////////////////////////////////////////////////////////////////
//
//	vp_AboutBox.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	a custom window for the main menu choice "About UFPS"
//
/////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using UnityEditor;

public class vp_AboutBox : EditorWindow
{

	static Vector2 m_DialogSize = new Vector2(260, 260);
	static string m_Version = "Version " + UFPSInfo.Version;
	static string m_Copyright = "© " + System.DateTime.Now.Year + " VisionPunk. All Rights Reserved.";
	public static Texture2D m_UFPSLogo = (Texture2D)Resources.Load("Icons/UFPS239x86", typeof(Texture2D));
	public static Texture2D m_VPLogo = (Texture2D)Resources.Load("Icons/VisionPunk99x62", typeof(Texture2D));
	protected static GUIStyle m_SmallTextStyle = null;


	/// <summary>
	/// 
	/// </summary>
	public static void Create()
	{

		vp_AboutBox msgBox = (vp_AboutBox)EditorWindow.GetWindow(typeof(vp_AboutBox), true);

		msgBox.titleContent.text = "About UFPS";
		
		msgBox.minSize = new Vector2(m_DialogSize.x, m_DialogSize.y);
		msgBox.maxSize = new Vector2(m_DialogSize.x + 1, m_DialogSize.y + 1);
		msgBox.position = new Rect(
			(Screen.currentResolution.width / 2) - (m_DialogSize.x / 2),
			(Screen.currentResolution.height / 2) - (m_DialogSize.y / 2),
			m_DialogSize.x,
			m_DialogSize.y);
		msgBox.Show();

	}


	/// <summary>
	/// 
	/// </summary>
	void OnGUI()
	{

		if (m_UFPSLogo != null)
			GUI.DrawTexture(new Rect(10, 10, m_UFPSLogo.width, m_UFPSLogo.height), m_UFPSLogo);

		GUILayout.BeginArea(new Rect(20, 105, Screen.width - 40, Screen.height - 40));
		GUI.backgroundColor = Color.clear;
		GUILayout.Label(m_Version);
		GUILayout.Label(m_Copyright + "\n\n", SmallTextStyle);
		if (GUILayout.Button("https://twitter.com/VisionPunk", vp_EditorGUIUtility.LinkStyle)) { Application.OpenURL("https://twitter.com/VisionPunk"); }
		if (GUILayout.Button("http://forum.visionpunk.com", vp_EditorGUIUtility.LinkStyle)) { Application.OpenURL("http://forum.visionpunk.com"); }
		GUI.color = Color.white;
		GUI.backgroundColor = Color.white;
		GUILayout.EndArea();


		if (m_UFPSLogo != null)
			GUI.DrawTexture(new Rect(150, 200, m_VPLogo.width, m_VPLogo.height), m_VPLogo);


	}


	/// <summary>
	/// 
	/// </summary>
	public static GUIStyle SmallTextStyle
	{
		get
		{
			if (m_SmallTextStyle == null)
			{
				m_SmallTextStyle = new GUIStyle("Label");
				m_SmallTextStyle.fontSize = 9;
			}
			return m_SmallTextStyle;
		}
	}
    

}

