/////////////////////////////////////////////////////////////////////////////////
//
//	vp_PlayerDamageHandler.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	a version of the vp_DamageHandler class extended for use with
//					vp_PlayerEventHandler
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vp_PlayerDamageHandler : vp_DamageHandler
{

	
	private vp_PlayerEventHandler m_Player = null;	// should never be referenced directly
	protected vp_PlayerEventHandler Player	// lazy initialization of the event handler field
	{
		get
		{
			if (m_Player == null)
				m_Player = transform.GetComponent<vp_PlayerEventHandler>();
			return m_Player;
		}
	}

	private vp_PlayerInventory m_Inventory = null;
	protected vp_PlayerInventory Inventory
	{
		get
		{
			if (m_Inventory == null)
				m_Inventory = transform.root.GetComponentInChildren<vp_PlayerInventory>();
			return m_Inventory;
		}
	}

	// falling damage
	public bool AllowFallDamage = true;
	public float FallImpactThreshold = .15f;
	public bool DeathOnFallImpactThreshold = false;
	public Vector2 FallImpactPitch = new Vector2(1.0f, 1.5f);	// random pitch range for fall impact sounds
	public List<AudioClip> FallImpactSounds = new List<AudioClip>();
	protected float m_FallImpactMultiplier = 2;
	protected bool m_InventoryWasEnabledAtStart = true;		// helper feature to facilitate developing with a temp-disabled inventory

	protected List<Collider> m_Colliders = null;

	protected List<Collider> Colliders
	{
		get
		{
			if (m_Colliders == null)
			{
				m_Colliders = new List<Collider>();
				foreach (Collider c in GetComponentsInChildren<Collider>())
				{
					if (c.gameObject.layer == vp_Layer.RemotePlayer)
					{
						m_Colliders.Add(c);
					}
				}
			}
			return m_Colliders;
		}
	}


	/// <summary>
	/// registers this component with the event handler (if any).
	/// NOTE: this is overriden by vp_FPPlayerEventHandler
	/// </summary>
	protected override void OnEnable()
	{

		if (Player != null)
			Player.Register(this);

	}


	/// <summary>
	/// unregisters this component from the event handler (if any).
	/// NOTE: this is overriden by vp_FPPlayerEventHandler
	/// </summary>
	protected override void OnDisable()
	{

		if (Player != null)
			Player.Unregister(this);

	}


	/// <summary>
	/// 
	/// </summary>
	void Start()
	{

		if (Inventory != null)
			m_InventoryWasEnabledAtStart = Inventory.enabled;


	}
	

	/// <summary>
	/// instantiates the player's death effect, clears the current
	/// weapon and activates the Dead activity
	/// </summary>
	public override void Die()
	{

		if (!enabled || !vp_Utility.IsActive(gameObject))
			return;

		if (m_Audio != null)
		{
			m_Audio.pitch = Time.timeScale;
			m_Audio.PlayOneShot(DeathSound);
		}

		foreach (GameObject o in DeathSpawnObjects)
		{
			if (o != null)
				vp_Utility.Instantiate(o, transform.position, transform.rotation);
		}

		foreach (Collider c in Colliders)
		{
			c.enabled = false;
		}

		if ((Inventory != null) && Inventory.enabled)
			Inventory.enabled = false;

		Player.SetWeapon.Argument = 0;
		Player.SetWeapon.Start();
		Player.Dead.Start();
		Player.Run.Stop();
		Player.Jump.Stop();
		Player.Crouch.Stop();
		Player.Zoom.Stop();
		Player.Attack.Stop();
		Player.Reload.Stop();
		Player.Climb.Stop();
		Player.Interact.Stop();

		// if we're the master in multiplayer, send kill event to other players
		if (vp_Gameplay.isMultiplayer && vp_Gameplay.isMaster)
		{
			//Debug.Log("sending kill event from master scene to vp_MasterClient");
			vp_GlobalEvent<Transform>.Send("TransmitKill", transform.root);
		}

	}


	/// <summary>
	/// 
	/// </summary>
	protected override void Reset()
	{

		base.Reset();
		
		if (!Application.isPlaying)
			return;

		Player.Dead.Stop();
		Player.Stop.Send();

		foreach (Collider c in Colliders)
		{
			c.enabled = true;
		}

		if ((Inventory != null) && !Inventory.enabled)
			Inventory.enabled = m_InventoryWasEnabledAtStart;

		if (m_Audio != null)
		{
			m_Audio.pitch = Time.timeScale;
			m_Audio.PlayOneShot(RespawnSound);
		}

	}


	/// <summary>
	/// gets or sets the current health 
	/// </summary>
	protected virtual float OnValue_Health
	{
		get
		{
			return CurrentHealth;
		}
		set
		{
			CurrentHealth = Mathf.Min(value, MaxHealth);	// health is not allowed to go above max, but negative health is allowed (for gibbing)
		}
	}


	/// <summary>
	/// gets the player's max health 
	/// </summary>
	protected virtual float OnValue_MaxHealth
	{
		get
		{
			return MaxHealth;
		}
	}


	/// <summary>
	/// applies falling damage to the player
	/// </summary>
	protected virtual void OnMessage_FallImpact(float impact)
	{

		if (Player.Dead.Active || !AllowFallDamage || impact <= FallImpactThreshold)
			return;

		vp_AudioUtility.PlayRandomSound(m_Audio, FallImpactSounds, FallImpactPitch);

		float damage = (float)Mathf.Abs((float)(DeathOnFallImpactThreshold ? MaxHealth : MaxHealth * impact));

		Damage(new vp_DamageInfo(damage, transform, transform, vp_DamageInfo.DamageType.Fall));

	}


}

