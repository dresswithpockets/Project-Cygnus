/////////////////////////////////////////////////////////////////////////////////
//
//	vp_Explosion.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	death effect for exploding objects. applies damage and a
//					physical force to all rigidbodies and players within its
//					range, plays a sound and instantiates a list of gameobjects
//					for special effects (e.g. particle fx). destroys itself when
//					the sound has stopped playing
//
///////////////////////////////////////////////////////////////////////////////// 

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]

public class vp_Explosion : MonoBehaviour
{

	// gameplay
	public float Radius = 15.0f;					// any objects within radius will be affected by the explosion
	public float Force = 1000.0f;					// amount of positional force to apply to affected objects
	public float UpForce = 10.0f;					// how much to push affected objects up in the air
	public float Damage = 10;						// amount of damage to apply to objects via their 'Damage' method
	public bool AllowCover = false;					// if true, damage can only be done with line of sight between explosion center and target top or center
	public bool AllowMultiDamage = false;			// if true, this explosion can damage multiple sub-objects under the same root object at the same time. if false, only the first damage handler or 'Damage' method found will trigger
	public float CameraShake = 1.0f;				// how much of a shockwave impulse to apply to the camera
	public string DamageMessageName = "Damage";		// user defined name of damage method on affected object
														// TIP: this can be used to apply different types of damage, i.e
														// magical, freezing, poison, electric
	public bool RequireDamageHandler = true;		// if true (default) the target must have a vp_DamageHandler-derived component on it.
													// this is an optimization to avoid sending messages to large numbers of colliders.
													// if false, the explosion will broadcast a unity message 'Damage' with a float argument to any collider affected
	protected bool m_HaveExploded = false;			// when true, the explosion is flagged for removal / recycling
	
	// sound
	public AudioClip Sound = null;
	public float SoundMinPitch = 0.8f;				// random pitch range for explosion sound
	public float SoundMaxPitch = 1.2f;

	// fx
	public List <GameObject> FXPrefabs = new List<GameObject>();	// list of special effects objects to spawn
	
	// physics
	protected Ray m_Ray;
	protected RaycastHit m_RaycastHit;
	protected Collider m_TargetCollider = null;
	protected Transform m_TargetTransform = null;
	protected Rigidbody m_TargetRigidbody = null;
	protected float m_DistanceModifier = 0.0f;

	// a dictionary to make sure we don't damage the same object several times in a frame
	protected Dictionary<Transform, object> m_RootTransformsHitByThisExplosion = new Dictionary<Transform, object>(50);

	protected static vp_DamageHandler m_TargetDHandler = null;

	// --- misc properties ---

	protected float DistanceModifier
	{
		get
		{
			if (m_DistanceModifier == 0.0f)
				m_DistanceModifier = (1 - Vector3.Distance(Transform.position, m_TargetTransform.position) / Radius);
			return m_DistanceModifier;
		}
	}

	protected Transform m_Transform = null;
	protected Transform Transform
	{
		get
		{
			if (m_Transform == null)
				m_Transform = transform;
			return m_Transform;
		}
	}

	protected Transform m_Source = null;
	protected Transform Source
	{
		get
		{
			if (m_Source == null)
				m_Source = transform;
			return m_Source;
		}
		set
		{
			m_Source = value;
		}
	}

	protected Transform m_OriginalSource = null;
	protected Transform OriginalSource
	{
		get
		{
			if (m_OriginalSource == null)
				m_OriginalSource = transform;
			return m_OriginalSource;
		}
		set
		{
			m_OriginalSource = value;
		}
	}

	protected AudioSource m_Audio = null;
	protected AudioSource Audio
	{
		get
		{
			if (m_Audio == null)
				m_Audio = GetComponent<AudioSource>();
			return m_Audio;
		}
	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnEnable()
	{
		// register method to allow keeping track of who caused the explosion
		Source = transform;
		OriginalSource = null;
		vp_TargetEvent<Transform>.Register(transform, "SetSource", SetSource);
	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnDisable()
	{
		Source = null;
		OriginalSource = null;
		vp_TargetEvent<Transform>.Unregister(transform, "SetSource", SetSource);
	}


	/// <summary>
	/// 
	/// </summary>
	void Update()
	{

		// the explosion should be removed as soon as the sound has stopped playing.
		// NOTE: this implementation assumes that the sound is always longer in seconds
		// than the explosion effect. should be OK in most cases
		if (m_HaveExploded)
		{
			if (!Audio.isPlaying)
				vp_Utility.Destroy(gameObject);
			return;
		}

		DoExplode();

	}


	/// <summary>
	/// spawns effects, applies forces and damage to close by objects
	/// and flags the explosion for removal
	/// </summary>
	void DoExplode()
	{

		m_HaveExploded = true;

		// spawn effects gameobjects
		foreach (GameObject fx in FXPrefabs)
		{
			if (fx != null)
			{
#if UNITY_EDITOR
				Component[] c;
				c = fx.GetComponents<vp_Explosion>();	// OK from a performance perspective because it only occurs in editor
				if (c.Length > 0)
					Debug.LogError("Error: vp_Explosion->FXPrefab must not be a vp_Explosion (risk of infinite loop).");
				else
#endif
					vp_Utility.Instantiate(fx, Transform.position, Transform.rotation);
			}
		}

		// clear the list of affected objects in case this explosion has been pooled
		m_RootTransformsHitByThisExplosion.Clear();

		// apply shockwave to all rigidbodies and players within range, but
		// ignore small and walk-thru objects such as debris, triggers and water
		Collider[] colliders = Physics.OverlapSphere(Transform.position, Radius, vp_Layer.Mask.IgnoreWalkThru);
		foreach (Collider hit in colliders)
		{

			if (hit.gameObject.isStatic)
				continue;

			m_DistanceModifier = 0.0f;

			if ((hit != null) && (hit != this.GetComponent<Collider>()))
			{

				m_TargetCollider = hit;
				m_TargetTransform = hit.transform;

				// --- add camera shake ---
				AddUFPSCameraShake();

				// --- abort if we have no line of sight to target ---
				if (TargetInCover())
					continue;

				// --- try to add force ---
				m_TargetRigidbody = hit.GetComponent<Rigidbody>();
				if (m_TargetRigidbody != null)		// target has a rigidbody: apply force using Unity physics
					AddRigidbodyForce();
				else 								// target has no rigidbody. try and apply force using UFPS physics
					AddUFPSForce();

				// --- try to add damage ---

				if (!AllowMultiDamage)
				{
					// abort if this explosion has already affected the target collider's root object
					if (!m_RootTransformsHitByThisExplosion.ContainsKey(m_TargetCollider.transform.root))
					{
						m_RootTransformsHitByThisExplosion.Add(m_TargetCollider.transform.root, null);	// remember that we have processed this target
						DoDamage();
					}
				}
				else
				{
					DoDamage();
				}

				//Debug.Log(m_TargetTransform.name + Time.time);	// SNIPPET: to dump affected objects

			}

		}

		float modDmg = (DistanceModifier * Damage);
		CharacterController[] controllers = FindObjectsOfType<CharacterController>();
		foreach (CharacterController cc in controllers)
			if (cc.GetComponent<Pawn>() != null) cc.SendMessage("Damage", new DamageInfo(DamageType.PHYSICAL, DamageElement.NONE, (modDmg > 0f ? modDmg : 0f), null));

		// play explosion sound
		Audio.clip = Sound;
		Audio.pitch = Random.Range(SoundMinPitch, SoundMaxPitch) * Time.timeScale;
		if (!Audio.playOnAwake)
			Audio.Play();

	}


	/// <summary>
	/// 
	/// </summary>
	void DoDamage()
	{
		m_TargetDHandler = vp_DamageHandler.GetDamageHandlerOfCollider(m_TargetCollider);
		if (m_TargetDHandler != null)
		{
			// target has a known damagehandler -> send damage in UFPS format.
			// this works with targets that have a vp_DamageHandler (or derived) component
			m_TargetDHandler.Damage(new vp_DamageInfo((DistanceModifier * Damage), Source, OriginalSource, vp_DamageInfo.DamageType.Explosion));
		}
		else if (!RequireDamageHandler)
		{
			// target is known to have no damagehandler -> send damage the 'Unity way'.
			// this works with targets that have a custom script with the standard
			// method: "Damage(float damage)"
			m_TargetCollider.gameObject.BroadcastMessage(DamageMessageName, (DistanceModifier * Damage), SendMessageOptions.DontRequireReceiver);
		}

	}


	/// <summary>
	/// if taking cover is allowed this method will return true whenever
	/// the target is hidden (waist up) behind a solid object. it will
	/// return false if there is a clear line of sight between the
	/// explosion's center and the target's top OR center
	/// </summary>
	protected virtual bool TargetInCover()
	{

		if (!AllowCover)
			return false;

		m_Ray.origin = Transform.position;	// center of explosion

		// try to find a clear line of sight to target

		// --- middle / hips ---
		m_Ray.direction = (m_TargetCollider.bounds.center - Transform.position).normalized;
		if (Physics.Raycast(m_Ray, out m_RaycastHit, Radius + 1.0f) && (m_RaycastHit.collider == m_TargetCollider))
			return false;	// target's center / waist exposed

		// --- top / head ---
		m_Ray.direction = ((vp_3DUtility.HorizontalVector(m_TargetCollider.bounds.center) + (Vector3.up * (m_TargetCollider.bounds.max.y))) - Transform.position).normalized;
		if (Physics.Raycast(m_Ray, out m_RaycastHit, Radius + 1.0f) && (m_RaycastHit.collider == m_TargetCollider))
			return false;	// target's top / head exposed

		// SNIPPET: tracking explosion damage against feet can end up somewhat
		// unintuitive in-game. for example, getting killed by explosive force
		// reaching a player's feet along the ground under a car may be simply
		// confusing ("wait-what-happened"), but uncomment this to allow for it:
			// --- bottom / feet ---
			//m_Ray.direction = ((vp_3DUtility.HorizontalVector(m_TargetCollider.bounds.center) + (Vector3.up * m_TargetCollider.bounds.min.y)) - Transform.position).normalized;
			//if (Physics.Raycast(m_Ray, out m_RaycastHit, Radius + 1.0f) && (m_RaycastHit.collider == m_TargetCollider))
			//	return false;	// target's bottom / feet exposed

		// no line of sight found to target!
		return true;

	}


	/// <summary>
	/// applies force to a target by means of the Unity physics engine
	/// </summary>
	protected virtual void AddRigidbodyForce()
	{

		// TEMP: in UFPS multiplayer, we end up here with a remote player target.
		// this is not entirely correct, but works for now since it has no effect
		if (m_TargetRigidbody.isKinematic)
			return;

		// explosion up-force should only work on grounded objects, otherwise
		// object may acquire extreme speeds. also, this gives a more profound
		// effect of explosion force being deflected off the ground
		m_Ray.origin = m_TargetTransform.position;
		m_Ray.direction = -Vector3.up;
		if (!Physics.Raycast(m_Ray, out m_RaycastHit, 1))
			UpForce = 0.0f;

		// bash the found object
		m_TargetRigidbody.AddExplosionForce((Force / Time.timeScale) / vp_TimeUtility.AdjustedTimeScale, Transform.position, Radius, UpForce);
		
	}


	/// <summary>
	/// applies force to a target using UFPS camera & controller physics.
	/// this should only affect human players (local or remote) and possibly
	/// AI that use the UFPS body system
	/// </summary>
	protected virtual void AddUFPSForce()
	{

		// bash things that listen to the 'ForceImpact' message (e.g. players)
		vp_TargetEvent<Vector3>.Send(m_TargetTransform.root, "ForceImpact", (m_TargetTransform.position -
																				Transform.position).normalized *
																				Force * 0.001f * DistanceModifier);

	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void AddUFPSCameraShake()
	{

		// shake things that listen to the 'CameraBombShake' message (e.g. cameras)
		vp_TargetEvent<float>.Send(m_TargetTransform.root, "CameraBombShake", (DistanceModifier * CameraShake));

	}


	/// <summary>
	/// this is used as an event target to allow keeping track of who caused
	/// the explosion. 	NOTE: in the case of grenades, this gets called by the
	/// damagehandler of the grenade itself (upon death)
	/// </summary>
	public void SetSource(Transform source)
	{
		m_OriginalSource = source;		// who set off this explosion
	}


}

	