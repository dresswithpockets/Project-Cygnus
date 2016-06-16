/////////////////////////////////////////////////////////////////////////////////
//
//	vp_Input.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	This class handles mouse, keyboard and joystick input. All
//					UFPS input should run through this class to keep all input
//					in one centralized location.
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vp_Input : MonoBehaviour
{

	[System.Serializable]
	public class vp_InputAxis
	{
		public KeyCode Positive;
		public KeyCode Negative;
	}

	public int ControlType = 0;

	// buttons
	public Dictionary<string, KeyCode> Buttons = new Dictionary<string, KeyCode>();
	public List<string> ButtonKeys = new List<string>();
	public List<KeyCode> ButtonValues = new List<KeyCode>();
	
	// axis
	public Dictionary<string, vp_InputAxis> Axis = new Dictionary<string, vp_InputAxis>();
	public List<string> AxisKeys = new List<string>();
	public List<vp_InputAxis> AxisValues = new List<vp_InputAxis>();
	
	// Unity Input Axis
	public List<string> UnityAxis = new List<string>();
	
	// paths
	protected static string m_FolderPath = "UFPS/Base/Content/Resources/Input";
	protected static string m_PrefabPath = "Assets/UFPS/Base/Content/Resources/Input/vp_Input.prefab";
	
	
	public static bool mIsDirty = true;

	/// <summary>
	/// Retrieves an instance of the input manager
	/// </summary>
	protected static vp_Input m_Instance;
	static public vp_Input Instance
	{
		get
		{
			if (mIsDirty)
			{
				mIsDirty = false;
				
				if(m_Instance == null)
				{
					if(Application.isPlaying)
					{
						// if application is playing, load vp_Input from resources
						GameObject go = Resources.Load("Input/vp_Input") as GameObject;
						if(go == null)
						{
							m_Instance = new GameObject("vp_Input").AddComponent<vp_Input>();
						}
						else
						{
							m_Instance = go.GetComponent<vp_Input>();
							if(m_Instance == null)
								m_Instance = go.AddComponent<vp_Input>();
						}
					}
					m_Instance.SetupDefaults();
				}
			}
			return m_Instance;
		}
	}
	
	
	/// <summary>
	/// Creates the required prefab if one doesn't exist
	/// </summary>
	public static void CreateIfNoExist()
	{
	
#if UNITY_EDITOR
		
		GameObject go = UnityEditor.AssetDatabase.LoadAssetAtPath(m_PrefabPath, typeof(GameObject)) as GameObject;
		if(go == null)
		{
			// create a directory hierarchy to store the prefab if one doesn't exist
			if(!Application.isPlaying)
			{
				bool needsRefresh = false;
				string path = "";
				string[] folders = m_FolderPath.Split(new string[1]{ "/" }, System.StringSplitOptions.None);
				foreach(string folder in folders)
				{
					path += "/";
					if(!System.IO.Directory.Exists(Application.dataPath + path + folder))
					{
						needsRefresh = true;
						System.IO.Directory.CreateDirectory(Application.dataPath + path + folder);
					}
					path += folder;
				}
				if(needsRefresh)
					UnityEditor.AssetDatabase.Refresh();
			}
				
			go = new GameObject("vp_Input") as GameObject;
			go.AddComponent<vp_Input>();
			UnityEditor.PrefabUtility.CreatePrefab(m_PrefabPath, go);
			Object.DestroyImmediate(go);
		}
		else
		{
			if(go.GetComponent<vp_Input>() == null)
				go.AddComponent<vp_Input>();
		}
		
#endif
	
	}
	
	
	/// <summary>
	/// 
	/// </summary>
	protected virtual void Awake()
	{
	
		if(m_Instance == null)
			m_Instance = Instance;
	
	}
	
	
	/// <summary>
	/// Makes this instance dirty
	/// </summary>
	public virtual void SetDirty( bool dirty )
	{
		mIsDirty = dirty;
	}
	
	
	/// <summary>
	/// Setups the defaults input buttons and axes
	/// </summary>
	public virtual void SetupDefaults( string type = "" )
	{
	
		if(type == "" || type == "Buttons")
		{
			if(ButtonKeys.Count == 0)
			{
				AddButton("Attack", KeyCode.Mouse0);
				AddButton("SetNextWeapon", KeyCode.E);
				AddButton("SetPrevWeapon", KeyCode.Q);
				AddButton("ClearWeapon", KeyCode.Backspace);
				AddButton("Zoom", KeyCode.Mouse1);
				AddButton("Reload", KeyCode.R);
				AddButton("Jump", KeyCode.Space);
				AddButton("Crouch", KeyCode.C);
				AddButton("Run", KeyCode.LeftShift);
				AddButton("Interact", KeyCode.F);
				AddButton("Accept1", KeyCode.Return);
				AddButton("Accept2", KeyCode.KeypadEnter);
				AddButton("Pause", KeyCode.P);
				AddButton("Menu", KeyCode.Escape);
			}
		}
		
		if(type == "" || type == "Axis")
		{
			if(AxisKeys.Count == 0)
			{
				AddAxis("Vertical", KeyCode.W, KeyCode.S);
				AddAxis("Horizontal", KeyCode.D, KeyCode.A);
			}
		}
		
		if(type == "" || type == "UnityAxis")
		{
			if(UnityAxis.Count == 0)
			{
				AddUnityAxis("Mouse X");
				AddUnityAxis("Mouse Y");
			}
		}
		
		UpdateDictionaries();
	
	}
	
	
	/// <summary>
	/// Adds a button with specified keycode
	/// </summary>
	public virtual void AddButton( string n, KeyCode k = KeyCode.None )
	{
	
		if(ButtonKeys.Contains(n))
			ButtonValues[ButtonKeys.IndexOf(n)] = k;
		else
		{
			ButtonKeys.Add(n);
			ButtonValues.Add(k);
		}
	
	}

	
	/// <summary>
	/// Adds an axis with a positive and negative key
	/// </summary>
	public virtual void AddAxis( string n, KeyCode pk = KeyCode.None, KeyCode nk = KeyCode.None )
	{
	
		if(AxisKeys.Contains(n))
			AxisValues[AxisKeys.IndexOf(n)] = new vp_InputAxis{ Positive = pk, Negative = nk };
		else
		{
			AxisKeys.Add(n);
			AxisValues.Add(new vp_InputAxis{ Positive = pk, Negative = nk });
		}
	
	}
	
	
	/// <summary>
	/// Adds a unity axis.
	/// </summary>
	public virtual void AddUnityAxis( string n )
	{
	
		if(UnityAxis.Contains(n))
			UnityAxis[UnityAxis.IndexOf(n)] = n;
		else
		{
			UnityAxis.Add(n);
		}
	
	}
	
	
	/// <summary>
	/// Updates the input dictionaries
	/// </summary>
	public virtual void UpdateDictionaries()
	{
	
		if(!Application.isPlaying)
			return;
	
		Buttons.Clear();
		for(int i=0;i<ButtonKeys.Count;i++)
			Buttons.Add(ButtonKeys[i], ButtonValues[i]);
			
		Axis.Clear();
		for(int i=0;i<AxisKeys.Count;i++)
			Axis.Add(AxisKeys[i], new vp_InputAxis{ Positive = AxisValues[i].Positive, Negative = AxisValues[i].Negative});
	
	}
	
	
	/// <summary>
	/// handles keyboard, mouse and joystick input for
	/// any button state
	/// </summary>
	public static bool GetButtonAny( string button )
	{
	
		return Instance.DoGetButtonAny( button );
	
	}
	
	
	/// <summary>
	/// handles keyboard, mouse and joystick input for
	/// any button state
	/// </summary>
	public virtual bool DoGetButtonAny( string button )
	{
	
		if(Buttons.ContainsKey(button))
			return Input.GetKey( Buttons[button] ) || Input.GetKeyDown( Buttons[button] ) || Input.GetKeyUp( Buttons[button] );
			
		Debug.LogError("\""+button+"\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");	
		return false;
	
	}
	
	
	/// <summary>
	/// handles keyboard, mouse and joystick input while a button is held
	/// </summary>
	public static bool GetButton(string button)
	{
	
		return Instance.DoGetButton( button );
	
	}
	
	
	/// <summary>
	/// handles keyboard, mouse and joystick input while a button is held
	/// </summary>
	public virtual bool DoGetButton( string button )
	{
	
		if(Buttons.ContainsKey(button))
			return Input.GetKey( Buttons[button] );
			
		Debug.LogError("\""+button+"\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");	
		return false;
	
	}
	
	
	/// <summary>
	/// handles keyboard, mouse and joystick input for a button down event
	/// </summary>
	public static bool GetButtonDown(string button)
	{
	
		return Instance.DoGetButtonDown( button );
	
	}
	
	
	/// <summary>
	/// handles keyboard, mouse and joystick input for a button down event
	/// </summary>
	public virtual bool DoGetButtonDown( string button )
	{
	

		if(Buttons.ContainsKey(button))
			return Input.GetKeyDown( Buttons[button] );
			
		Debug.LogError("\""+button+"\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	
	}
	
	
	/// <summary>
	/// handles keyboard, mouse and joystick input when a button is released
	/// </summary>
	public static bool GetButtonUp(string button)
	{
	
		return Instance.DoGetButtonUp( button );
	
	}
	
	
	/// <summary>
	/// handles keyboard, mouse and joystick input when a button is released
	/// </summary>
	public virtual bool DoGetButtonUp( string button )
	{
	
		if(Buttons.ContainsKey(button))
			return Input.GetKeyUp( Buttons[button] );
			
		Debug.LogError("\""+button+"\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	
	}
	
	
	/// <summary>
	/// handles keyboard, mouse and joystick input for axes
	/// </summary>
	public static float GetAxisRaw(string axis)
	{
	
		return Instance.DoGetAxisRaw( axis );
	
	}
	
	
	/// <summary>
	/// handles keyboard, mouse and joystick input for axes
	/// </summary>
	public virtual float DoGetAxisRaw( string axis )
	{
	
		if(Axis.ContainsKey(axis) && ControlType == 0)
		{
			float val = 0;
			if( Input.GetKey( Axis[axis].Positive ) )
				val = 1;
			if( Input.GetKey( Axis[axis].Negative ) )
				val = -1;
			return val;
		}
		else if(UnityAxis.Contains(axis))
		{
			return Input.GetAxisRaw(axis);
		}
		else
		{
			Debug.LogError("\""+axis+"\" is not in VP Input Manager's Unity Axis. You must add it for this Axis to work.");
			return 0;
		}
	
	}
	

	/// <summary>
	/// Changes the key for a button. If save == true the key for
	/// that button will be saved for next runtime. Example usage:
	/// vp_Input.ChangeButtonKey("Jump", KeyCode.G);
	/// </summary>
	public static void ChangeButtonKey( string button, KeyCode keyCode, bool save = false )
	{

		if(!Instance.Buttons.ContainsKey(button))
		{
			Debug.LogWarning("The Button \"" + button + "\" Doesn't Exist");
			return;
		}
	
		if(save)
			Instance.ButtonValues[vp_Input.Instance.ButtonKeys.IndexOf(button)] = keyCode;
		
		Instance.Buttons[button] = keyCode;

	}


	/// <summary>
	/// Changes an input axis. If save == true the axis will be saved
	/// for next runtime
	/// </summary>
	public static void ChangeAxis(string n, KeyCode pk = KeyCode.None, KeyCode nk = KeyCode.None, bool save = false)
	{

		if (!Instance.AxisKeys.Contains(n))
		{
			Debug.LogWarning("The Axis \"" + n + "\" Doesn't Exist");
			return;
		}

		if (save)
			Instance.AxisValues[vp_Input.Instance.AxisKeys.IndexOf(n)] = new vp_InputAxis { Positive = pk, Negative = nk };

		Instance.Axis[n] = new vp_InputAxis { Positive = pk, Negative = nk };

	}
	

}
