﻿/////////////////////////////////////////////////////////////////////////////////
//
//	vp_ItemPickup.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	this component implements pick-up-able item records. it depends
//					on a defined ItemType, which it will transmit to the vp_Inventory
//					component of any gameobject it collides with (if such can be
//					found).
//					
///////////////////////////////////////////////////////////////////////////////// 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(SphereCollider))]


[System.Serializable]
public class vp_ItemPickup : MonoBehaviour
{

#if UNITY_EDITOR
	[vp_ItemID]
#endif
	public int ID;

#if UNITY_EDITOR
	[vp_ItemAmount]
#endif

	public int Amount;

	protected Type m_ItemType = null;
	protected Type ItemType
	{
		get
		{
#if UNITY_EDITOR
			if (m_Item.Type == null)
			{
				Debug.LogWarning(string.Format(MissingItemTypeError, this), gameObject);
				return null;
			}
			return m_Item.Type.GetType();
#else
			if (m_ItemType == null)
				m_ItemType = m_Item.Type.GetType();
			return m_ItemType;
#endif
		}
	}


	protected vp_ItemType m_ItemTypeObject = null;
	public vp_ItemType ItemTypeObject
	{
		get
		{
#if UNITY_EDITOR
			if (m_Item.Type == null)
			{
				Debug.LogWarning(string.Format(MissingItemTypeError, this), gameObject);
				return null;
			}
			return m_Item.Type;
#else
			if (m_ItemTypeObject == null)
				m_ItemTypeObject = m_Item.Type;
			return m_ItemTypeObject;
#endif
		}
	}

	protected AudioSource m_Audio = null;
	protected AudioSource Audio
	{
		get
		{
			if (m_Audio == null)
			{
				if (GetComponent<AudioSource>() == null)
					gameObject.AddComponent<AudioSource>();
				m_Audio = GetComponent<AudioSource>();
			}
			return m_Audio;
		}
	}

	protected Collider m_Collider = null;
	protected Collider Collider
	{
		get
		{
			if (m_Collider == null)
				m_Collider = GetComponent<Collider>();
			return m_Collider;
		}
	}

	protected Collider [] m_Colliders = null;
	protected Collider[] Colliders
	{
		get
		{
			if (m_Colliders == null)
				m_Colliders = GetComponents<Collider>();
			return m_Colliders;
		}
	}

	protected Renderer m_Renderer = null;
	protected Renderer Renderer
	{
		get
		{
			if (m_Renderer == null)
				m_Renderer = GetComponent<Renderer>();
			return m_Renderer;
		}
	}

	protected Rigidbody m_Rigidbody = null;
	protected Rigidbody Rigidbody
	{
		get
		{
			if (m_Rigidbody == null)
				m_Rigidbody = GetComponent<Rigidbody>();
			return m_Rigidbody;
		}
	}


	//////////////// 'Item' section ////////////////
	[System.Serializable]
	public class ItemSection
	{

		public vp_ItemType Type = null;

#if UNITY_EDITOR
		[vp_HelpBox(typeof(ItemSection), UnityEditor.MessageType.None, typeof(vp_ItemPickup), null, true)]
		public float helpbox;
#endif

	}
	[SerializeField]
	protected ItemSection m_Item;


	//////////////// 'Recipient Tags' section ////////////////
	[System.Serializable]
	public class RecipientTagsSection
	{
		public List<string> Tags = new List<string>();

#if UNITY_EDITOR
		[vp_HelpBox(typeof(RecipientTagsSection), UnityEditor.MessageType.None, typeof(vp_ItemPickup), null, true)]
		public float helpbox;
#endif
	}
	[SerializeField]
	protected RecipientTagsSection m_Recipient;

	//////////////// 'Sounds' section ////////////////
	[System.Serializable]
	public class SoundSection
	{
		public AudioClip PickupSound = null;		// player triggers the pickup
		public bool PickupSoundSlomo = true;
		public AudioClip PickupFailSound = null;	// player failed to pick up the item (i.e. ammo full)
		public bool FailSoundSlomo = true;
	}
	[SerializeField]
	protected SoundSection m_Sound;


	//////////////// 'Messages' section ////////////////
	[System.Serializable]
	public class MessageSection
	{
		public string SuccessSingle = "Picked up {2}.";
		public string SuccessMultiple = "Picked up {4} {1}s.";
		public string FailSingle = "Can't pick up {2} right now.";
		public string FailMultiple = "Can't pick up {4} {1}s right now.";
#if UNITY_EDITOR
		[vp_HelpBox(typeof(MessageSection), UnityEditor.MessageType.None, typeof(vp_ItemPickup), null, true)]
		public float helpbox;
#endif
	}
	[SerializeField]
	protected MessageSection m_Messages;

	// when this is true, the pickup has been triggered and will
	// disappear as soon as the pickup sound has finished playing
	protected bool m_Depleted = false;
	protected int m_PickedUpAmount;

	protected string MissingItemTypeError = "Warning: {0} has no ItemType object!";

	// this boolean prevents the trigger from playing fail sounds
	// continuously if the player is standing inside the trigger
	protected bool m_AlreadyFailed = false;

	static Dictionary<Collider, vp_Inventory> m_ColliderInventories = new Dictionary<Collider, vp_Inventory>();

	// SNIPPET: if you have the UFPS & Photon Cloud Multiplayer Starter Kit,
	// this code can be enabled to show debug-IDs floating above each pickup.
	// NOTE: you have to also uncomment 'm_NameTag' in Update

	/*
	protected vp_NameTag m_NameTag = null;
	public bool ShowID
	{
		set
		{
			if (value == true)
			{
				if (m_NameTag == null)
				{
					m_NameTag = gameObject.AddComponent<vp_NameTag>();
					m_NameTag.Text = "";
				}
				m_NameTag.enabled = true;
			}
			else if(m_NameTag != null)
				m_NameTag.enabled = false;
		}
	}
	*/


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Awake()
	{

		if (ItemType == typeof(vp_UnitType))
			Amount = Mathf.Max(1, Amount);

		// set the main collider of this gameobject to be a trigger
		Collider.isTrigger = true;
		
		if ((m_Sound.PickupSound != null) || (m_Sound.PickupFailSound != null))
		{
			Audio.clip = m_Sound.PickupSound;
			Audio.playOnAwake = false;
		}

	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Update()
	{

		// remove the pickup if it has been depleted and the pickup
		// sound has stopped playing
		if (m_Depleted && !Audio.isPlaying)
			SendMessage("Die", SendMessageOptions.DontRequireReceiver);

		// disable rigidbody collider once it has landed
		if (!m_Depleted && (Rigidbody != null) && Rigidbody.IsSleeping() && !Rigidbody.isKinematic)
		{
			vp_Timer.In(0.5f, delegate()	// allow some time for the pickup to touch down or it may pause floating
			{
				Rigidbody.isKinematic = true;
				foreach (Collider c in Colliders)
				{
					if (c.isTrigger)
						continue;
					c.enabled = false;
				}
			});
		}

		// SNIPPET: see note about 'm_NameTag' above
		//if ((m_NameTag != null) && m_NameTag.enabled && string.IsNullOrEmpty(m_NameTag.Text))
		//	m_NameTag.Text = ID.ToString();

	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnEnable()
	{

		// restore rigidbody + colliders
		if (Rigidbody != null)
		{
			Rigidbody.isKinematic = false;
			foreach (Collider c in Colliders)
			{
				if (c.isTrigger)
					continue;
				c.enabled = true;
			}
		}

		Renderer.enabled = true;
		m_Depleted = false;
		m_AlreadyFailed = false;

		vp_GlobalEvent<vp_ItemPickup>.Send("TransmitPickupRespawn", this);	// will only have effect in multiplayer

	}


	/// <summary>
	/// this is triggered when an object enters the collider
	/// </summary>
	protected virtual void OnTriggerEnter(Collider col)
	{

		if (ItemType == null)
			return;

		if (!vp_Gameplay.isMaster)
			return;

		if (!Collider.enabled)
			return;

		// // TODO:check for collider being a player here? otherwise it will scan for inventory on all floors on spawn

		TryGiveTo(col);

	}


	/// <summary>
	/// 
	/// </summary>
	public void TryGiveTo(Collider col)
	{

		// only do something if the trigger is still active
		if (m_Depleted)
			return;

		vp_Inventory inventory;
		if (!m_ColliderInventories.TryGetValue(col, out inventory))
		{
			inventory = vp_TargetEventReturn<vp_Inventory>.SendUpwards(col, "GetInventory");
			m_ColliderInventories.Add(col, inventory);
		}

		if (inventory == null)
			return;

		// see if the colliding object was a valid recipient
		if ((m_Recipient.Tags.Count > 0) && !m_Recipient.Tags.Contains(col.gameObject.tag))
			return;

		bool result = false;

		int prevAmount = vp_TargetEventReturn<vp_ItemType, int>.SendUpwards(col, "GetItemCount", m_Item.Type);


		if (ItemType == typeof(vp_ItemType))
			result = vp_TargetEventReturn<vp_ItemType, int, bool>.SendUpwards(col, "TryGiveItem", m_Item.Type, ID);
		else if (ItemType == typeof(vp_UnitBankType))
			result = vp_TargetEventReturn<vp_UnitBankType, int, int, bool>.SendUpwards(col, "TryGiveUnitBank", (m_Item.Type as vp_UnitBankType), Amount, ID);
		else if (ItemType == typeof(vp_UnitType))
			result = vp_TargetEventReturn<vp_UnitType, int, bool>.SendUpwards(col, "TryGiveUnits", (m_Item.Type as vp_UnitType), Amount);
		else if (ItemType.BaseType == typeof(vp_ItemType))
			result = vp_TargetEventReturn<vp_ItemType, int, bool>.SendUpwards(col, "TryGiveItem", m_Item.Type, ID);
		else if (ItemType.BaseType == typeof(vp_UnitBankType))
			result = vp_TargetEventReturn<vp_UnitBankType, int, int, bool>.SendUpwards(col, "TryGiveUnitBank", (m_Item.Type as vp_UnitBankType), Amount, ID);
		else if (ItemType.BaseType == typeof(vp_UnitType))
			result = vp_TargetEventReturn<vp_UnitType, int, bool>.SendUpwards(col, "TryGiveUnits", (m_Item.Type as vp_UnitType), Amount);

		if (result == true)
		{
			m_PickedUpAmount = (vp_TargetEventReturn<vp_ItemType, int>.SendUpwards(col, "GetItemCount", m_Item.Type) - prevAmount);	// calculate resulting amount given
			OnSuccess(col.transform);
		}
		else
		{
			OnFail(col.transform);
		}


	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnTriggerExit()
	{

		// reset fail status
		m_AlreadyFailed = false;

	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnSuccess(Transform recipient)
	{

		m_Depleted = true;

		if ((m_Sound.PickupSound != null)
			&& vp_Utility.IsActive(gameObject)
			&& Audio.enabled)
		{
			Audio.pitch = (m_Sound.PickupSoundSlomo ? Time.timeScale : 1.0f);
			Audio.Play();
		}

		Renderer.enabled = false;

		string msg = "";

		if ((m_PickedUpAmount < 2) || (ItemType == typeof(vp_UnitBankType)) || (ItemType.BaseType == typeof(vp_UnitBankType)))
			msg = string.Format(m_Messages.SuccessSingle, m_Item.Type.IndefiniteArticle, m_Item.Type.DisplayName, m_Item.Type.DisplayNameFull, m_Item.Type.Description, m_PickedUpAmount.ToString());
		else
			msg = string.Format(m_Messages.SuccessMultiple, m_Item.Type.IndefiniteArticle, m_Item.Type.DisplayName, m_Item.Type.DisplayNameFull, m_Item.Type.Description, m_PickedUpAmount.ToString());

		vp_GlobalEvent<Transform, string>.Send("HUDText", recipient, msg);	// // TODO:check if this is sent to local player (!)

		if (vp_Gameplay.isMultiplayer && vp_Gameplay.isMaster)
			vp_GlobalEvent<vp_ItemPickup, Transform>.Send("TransmitPickup", this, recipient);	// will only execute on the master in multiplayer


	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Die()
	{
		vp_Utility.Activate(gameObject, false);
	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnFail(Transform recipient)
	{

		if (!m_AlreadyFailed && m_Sound.PickupFailSound != null)
		{
			Audio.pitch = m_Sound.FailSoundSlomo ? Time.timeScale : 1.0f;
			Audio.PlayOneShot(m_Sound.PickupFailSound);
		}
		m_AlreadyFailed = true;

		string msg = "";

		if ((m_PickedUpAmount < 2) || (ItemType == typeof(vp_UnitBankType)) || (ItemType.BaseType == typeof(vp_UnitBankType)))
			msg = string.Format(m_Messages.FailSingle, m_Item.Type.IndefiniteArticle, m_Item.Type.DisplayName, m_Item.Type.DisplayNameFull, m_Item.Type.Description, Amount.ToString());
		else
			msg = string.Format(m_Messages.FailMultiple, m_Item.Type.IndefiniteArticle, m_Item.Type.DisplayName, m_Item.Type.DisplayNameFull, m_Item.Type.Description, Amount.ToString());

		vp_GlobalEvent<Transform, string>.Send("HUDText", recipient, msg);

	}


}

