using UnityEngine;
using System.Collections;

public class ScalarController : MonoBehaviour {

	bool increaseScale = true;
	

	// Use this for initialization
	void Start () {
		for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).Translate(new Vector3(0.05f * i, .05f * i, 0f));
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.S)) {

			if (increaseScale) transform.localScale = new Vector3(2f, 2f, 2f);
			else transform.localScale = new Vector3(1f, 1f, 1f);

			increaseScale = !increaseScale;
		}
	}
}
