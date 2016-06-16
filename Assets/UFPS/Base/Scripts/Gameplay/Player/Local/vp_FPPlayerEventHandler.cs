/////////////////////////////////////////////////////////////////////////////////
//
//	vp_FPPlayerEventHandler.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	this class declares events for communication between behaviours
//					that make up a LOCAL, FIRST PERSON PLAYER.
//					
//					IMPORTANT: this class is NOT intended for use on a REMOTE player,
//					AI player etc., since such players have no access to input, camera
//					HUI, GUI or first person weapon systems. universal events (dealing
//					with physics and activities that are non-exclusive to a first person
//					player) can be found in the parent class: vp_PlayerEventHandler
//
///////////////////////////////////////////////////////////////////////////////// 

using System;
using UnityEngine;

public class vp_FPPlayerEventHandler : vp_PlayerEventHandler
{

	// these declarations determine which events are supported by the
	// player event handler. it is then up to external classes to fill
	// them up with delegates for communication.

	// TIPS:
	//  1) mouse-over on the event types (e.g. vp_Message) for usage info.
	//  2) to find the places where an event is SENT, you can do 'Find All
	// References' on the event in your IDE. if this is not available, you
	// can search the project for the event name preceded by '.' (.Reload)
	//  3) to find the methods that LISTEN to an event, search the project
	// for its name preceded by '_' (_Reload)


	// gui
	public vp_Message<vp_DamageInfo> HUDDamageFlash;
	public vp_Message<string> HUDText;
	public vp_Value<Texture> Crosshair;
	public vp_Value<Texture2D> CurrentAmmoIcon;

	// input
	public vp_Value<Vector2> InputSmoothLook;
	public vp_Value<Vector2> InputRawLook;
	public vp_Message<string, bool> InputGetButton;
	public vp_Message<string, bool> InputGetButtonUp;
	public vp_Message<string, bool> InputGetButtonDown;
	public vp_Value<bool> InputAllowGameplay;
	public vp_Value<bool> Pause;

	// camera
	public vp_Value<Vector3> CameraLookDirection;	// returns camera forward vector. NOTE: this will be different from 'HeadLookDirection' in 3rd person
	public vp_Message CameraToggle3rdPerson;
	public vp_Message<float> CameraGroundStomp;
	public vp_Message<float> CameraBombShake;
	public vp_Value<Vector3> CameraEarthQuakeForce;
	public vp_Activity<Vector3> CameraEarthQuake;

	// old inventory system
		// TIP: these events can be removed along with the old inventory system
	public vp_Value<string> CurrentWeaponClipType;
	public vp_Attempt<object> AddAmmo;
	public vp_Attempt RemoveClip;


}

