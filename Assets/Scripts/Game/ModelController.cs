using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelController : MonoBehaviour {

	public List<GameObject> groups = new List<GameObject>();

	public MeshFilter filter;

	public bool do_data = false;
	public bool do_group = false;
	public Vox_Data data;
	public Vox_Group group;

	void Start() {
		filter = GetComponent<MeshFilter>();
		if (do_data && do_group) {
			DebugConsole.Log("Can't load meshes for both group and data. Using group mesh.", DebugWarningLevel.WARNING);
		}
		else if (do_data) {
			data.to_filter(ref filter);
		}
		else if (do_group) {
			group.to_filter(ref filter);
		}

		do_data = false;
		do_group = false;
	}

	void Update() {

	}

	// Segmented is a flag, when true load_vox will create an individual model for each group or segment in the Vox_Data mesh.
	// Please note that models which haven't been set as segmented cannot be fully animated upon using Model_Controller;
	// Only the root node can be animated upon, which will animate the entire model collectively.
	//
	//		-TH 4/29/2016
	//
	internal void load_vox(Vox_Data data, bool segmented = true) {
		if (data.empty_or_invalid())
			return;

		foreach (GameObject go in groups) {
			Destroy(go);
		}

		groups.Clear();

		if (segmented) {
			foreach (Vox_Group group in data.groups) {
				// Add a group object which will be assigned the mesh of the current group being iterated over
				GameObject go = (GameObject)Instantiate(GameController.instance.vox_prefab, transform.position + group.position, Quaternion.identity);

				go.GetComponent<ModelController>().load_group(group);

				go.transform.parent = transform;

				groups.Add(go);
			}
		}
		else {
			GameObject go = (GameObject)Instantiate(GameController.instance.vox_prefab, transform.position, Quaternion.identity);

			go.GetComponent<ModelController>().load_data(data);

			go.transform.parent = transform;

			groups.Add(go);
		}
	}

	internal void play_anim(Scd_Data data) {
		// TODO: Write queue animations to be played.
	} 

	void load_data(Vox_Data data) {
		do_data = true;
		this.data = data;
	}

	void load_group(Vox_Group group) {
		do_group = true;
		this.group = group;
	}

	#region animation

	IEnumerator do_anim(Scd_Data data) {
		float rate = data.rate;
		float start_time = Time.time;

		bool animate = true;
		while (animate) {

			// this only works since we're yielding inside of the loop and this is a coroutine.
			float time_passed = Mathf.Abs(Time.time - start_time);

			int frame = Mathf.CeilToInt(rate * time_passed);
			if (frame > data.frames.Length) break;

			foreach (Scd_Frame_Group group in data.frames[frame].groups) {
				process_group(group, data.flags);
			}

			yield return null;
		}
	}

	void process_group(Scd_Frame_Group group, List<ScdFlag> flags) {
		foreach (GameObject go in groups) {
			ModelController mc = go.GetComponent<ModelController>();
			if (mc.group.name == group.group) {
				if (flags.Contains(ScdFlag.CHANGES_POSITION)) {

					// TODO: Recreate interp generation so that it does not overcompensate interpolation when adding.
					/*

					In detail: Since we are adding (+=) the new group.position to transform.position, we need to ensure that we don't add every single POSITION
					but instead, we add every delta position between frames. This change should be made in the calculations for frames in Scd_Data.

					e.g. group.delta_position is essentially a frame-by-frame velocity

					*/

					// TODO: Add code here for adding the new group.delta_position to the transform.position

				}
				if (flags.Contains(ScdFlag.CHANGES_ROTATION)) {

					go.transform.rotation = Quaternion.Euler(group.rotation);

				}
				if (flags.Contains(ScdFlag.CHANGES_SCALE)) {

					go.transform.localScale = group.scale;

				}
			}
		}
	}

	#endregion
}
