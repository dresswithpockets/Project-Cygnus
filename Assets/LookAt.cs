using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

	public bool LookAtCamera;
	public Transform Target;

	void Update() {
		if (LookAtCamera) transform.LookAt(Camera.main.transform);
		else if (Target) transform.LookAt(Target);
	}
}
