/////////////////////////////////////////////////////////////////////////////////
//
//	vp_PoolManager.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	This class manages the pooling of objects. Pooling will only
//					occur if this is placed on a game object in the scene and enabled.
//					vp_Utility.Instantiate and vp_Utility.Destroy should be used
//					instead of Object.Instantiate and Object.Destroy because
//					those methods will manage the pooling automatically.
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class vp_PoolManager : MonoBehaviour
{

	/// <summary>
	/// A class that holds a few properties necessary for custom pooled objects
	/// </summary>
	[System.Serializable]
	public class vp_CustomPooledObject
	{
	
		public GameObject Prefab = null;	// prefab to check for pooling
		public int Buffer = 15;				// amount of objects to instantiate at start
		public int MaxAmount = 25;			// maximum amount of objects that will be pooled
	
	}

	public int MaxAmount = 25; 			// The max amount that will be pooled if not in the CustomPrefabs list
	public bool PoolOnDestroy = true;	// If an object is not pooled, but destroyed, it'll be added to the pool
    public List<GameObject> IgnoredPrefabs = new List<GameObject>();						// Prefabs in this list will not be pooled
    public List<vp_CustomPooledObject> CustomPrefabs = new List<vp_CustomPooledObject>();	// Specify settings for specific prefabs by adding to this list.
    
	protected Transform m_Transform; // The transform of this object. Used for parenting
    protected Dictionary<string, List<Object>> m_AvailableObjects = new Dictionary<string, List<Object>>();	// The pooled objects currently available.
    protected Dictionary<string, List<Object>> m_UsedObjects = new Dictionary<string, List<Object>>(); 		// Objects that are currently being used

	protected static vp_PoolManager m_Instance = null;
	public static vp_PoolManager Instance{ get{ return m_Instance; } }
    
    
    /// <summary>
    /// 
    /// </summary>
    protected virtual void Awake()
    {
    
    	m_Instance = this;
    	m_Transform = transform;
    
    }
    

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Start ()
    {

		foreach(vp_CustomPooledObject obj in CustomPrefabs)
        	AddObjects(obj.Prefab, Vector3.zero, Quaternion.identity, obj.Buffer);

    }
    
    
    /// <summary>
    /// Registers the pooling events
    /// </summary>
    protected virtual void OnEnable()
    {
    
    	vp_GlobalEventReturn<Object, Vector3, Quaternion, Object>.Register("vp_PoolManager Instantiate", InstantiateInternal);
    	vp_GlobalEvent<Object, float>.Register("vp_PoolManager Destroy", DestroyInternal);
    
    }
    
    
    /// <summary>
    /// Unregisters the pooling events
    /// </summary>
    protected virtual void OnDisable()
    {
    
    	vp_GlobalEventReturn<Object, Vector3, Quaternion, Object>.Unregister("vp_PoolManager Instantiate", InstantiateInternal);
    	vp_GlobalEvent<Object, float>.Unregister("vp_PoolManager Destroy", DestroyInternal);
    
    }
    
    
    /// <summary>
    /// Adds new object(s) for pooling
    /// </summary>
    public virtual void AddObjects( Object obj, Vector3 position, Quaternion rotation, int amount = 1 )
    {
    
    	if(obj == null)
    		return;
    
    	// add to the available objects dictionary if doesn't exist
		if(!m_AvailableObjects.ContainsKey(obj.name))
		{
			m_AvailableObjects.Add(obj.name, new List<Object>());
			m_UsedObjects.Add(obj.name, new List<Object>());
		}
		
		// create amount of objects
		for(int i=0; i<amount; i++)
		{
			GameObject newObj = GameObject.Instantiate(obj, position, rotation) as GameObject;
			newObj.name = obj.name;
			newObj.transform.parent = m_Transform;
			vp_Utility.Activate(newObj, false);
			m_AvailableObjects[obj.name].Add(newObj);
		}
    
    }
    
    
    /// <summary>
    /// Tries to look for an object already in the pool and
    /// returns it if found. If it's not found, it'll instantiate
    /// a new object and add it to the pool.
    /// </summary>
    protected virtual Object InstantiateInternal( Object original, Vector3 position, Quaternion rotation )
    {
    
    	// Check if this prefab is in the ignore list. If so, instantiate a new object
    	if(IgnoredPrefabs.FirstOrDefault(obj => obj.name == original.name || obj.name == original.name+"(Clone)") != null)
    		return Object.Instantiate(original, position, rotation) as Object;
    		
    	GameObject go = null;
    	List<Object> availableObjects = null;
    	List<Object> usedObjects = null;
    		
    	// Check if this object is already being pooled
    	if(m_AvailableObjects.TryGetValue(original.name, out availableObjects))
    	{
    		Retry:
    		
    		m_UsedObjects.TryGetValue(original.name, out usedObjects);
    	
    		// Check if the object has reached the default max amount
    		int objectCount = availableObjects.Count + usedObjects.Count;
	    	if(CustomPrefabs.FirstOrDefault(obj => obj.Prefab.name == original.name) == null && objectCount < MaxAmount && availableObjects.Count == 0)
	    		AddObjects(original, position, rotation);
    		
    		// if no objects are available, get a used object and retry
    		if(availableObjects.Count == 0)
    		{	
	    		go = usedObjects.FirstOrDefault() as GameObject;
	    		
	    		if(go == null)
	    		{
	    			usedObjects.Remove(go);
	    			goto Retry;
	    		}
	    		
	    		// deactivate the used object
	    		vp_Utility.Activate(go, false);
	    		
	    		// remove the object from used objects list
	    		usedObjects.Remove(go);
	    		
	    		// add it to the available objects list
	    		availableObjects.Add(go);
	    		
	    		// and try and instantiate again
	    		goto Retry;
    		}
    		
    		// get the first available object
			go = availableObjects.FirstOrDefault() as GameObject;
			
			// check if the object still exists
			if(go == null)
			{
				availableObjects.Remove(go);
				goto Retry;
			}
			
			// set the position and rotation
			go.transform.position = position;
			go.transform.rotation = rotation;
			
			// remove the object from the available list
			availableObjects.Remove(go);
			
			// add the object to the used list
			usedObjects.Add(go);
			
			// activate the object
			vp_Utility.Activate(go);
			
			// return the object
			return go;
    		
    	}
    	
    	// add a new object if this type of object isn't being pooled
    	AddObjects(original, position, rotation);
		
		// return the new object by calling this method again
		return InstantiateInternal(original, position, rotation);
    
    }
    
    
    /// <summary>
    /// Puts the object back into the pool if it's being pooled
    /// or destroys it if not.
    /// </summary>
    protected virtual void DestroyInternal( Object obj, float t )
    {
    
    	if(obj == null)
    		return;
    
    	// Check if this prefab is in the ignore list or if it is not pooled and destroy it if condition is met
    	if(IgnoredPrefabs.FirstOrDefault(o => o.name == obj.name || o.name == obj.name+"(Clone)") != null || (!m_AvailableObjects.ContainsKey(obj.name) && !PoolOnDestroy))
    	{
    		Object.Destroy(obj, t);
    		return;
    	}
    	
    	// handle timed destroy
    	if(t != 0)
    	{
    		vp_Timer.In(t, delegate { DestroyInternal(obj, 0); });
    		return;
    	}
    	
    	// if the object isn't being pooled, add it
    	if(!m_AvailableObjects.ContainsKey(obj.name))
    	{
    		AddObjects(obj, Vector3.zero, Quaternion.identity);
    		return;
    	}
    	
    	List<Object> availableObjects = null;
    	List<Object> usedObjects = null;
    	m_AvailableObjects.TryGetValue(obj.name, out availableObjects);
    	m_UsedObjects.TryGetValue(obj.name, out usedObjects);
    	
    	// get the object
    	GameObject go = usedObjects.FirstOrDefault(o => o.GetInstanceID() == obj.GetInstanceID()) as GameObject;
    	
    	if(go == null)
    		return;
    	
    	// parent the object back to pooling manager
    	go.transform.parent = m_Transform;
    	
    	// disable the object
    	vp_Utility.Activate(go, false);
    	
    	// remove the object from the used list
    	usedObjects.Remove(go);
    	
    	// add to the available objects list
    	availableObjects.Add(go);
    
    }

}