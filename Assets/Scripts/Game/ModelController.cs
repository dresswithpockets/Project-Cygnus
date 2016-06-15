using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelController : MonoBehaviour {

	public List<GameObject> Groups { get; set; }

	public MeshFilter Filter;

	public bool DoData { get; set; }
	public bool DoGroup { get; set; }
	public VoxData Data { get; set; }
	public VoxGroup Group { get; set; }

	void Awake() {
		Groups = new List<GameObject>();
	}

	void Start() {
		Filter = GetComponent<MeshFilter>();
		if (DoData && DoGroup) {
			DebugConsole.Log("Can't load meshes for both group and data. Using group mesh.", DebugWarningLevel.WARNING);
		}
		else if (DoData) {
			Data.PassToFilter(Filter);
		}
		else if (DoGroup) {
			Group.PassToFilter(Filter);
		}

		DoData = false;
		DoGroup = false;
	}

	void Update() {

	}

	// Segmented is a flag, when true load_vox will create an individual model for each group or segment in the Vox_Data mesh.
	// Please note that models which haven't been set as segmented cannot be fully animated upon using Model_Controller;
	// Only the root node can be animated upon, which will animate the entire model collectively.
	//
	//		-TH 4/29/2016
	//
	internal void LoadVox(VoxData data, bool segmented = true) {
		if (data.EmptyOrInvalid())
			return;

		foreach (GameObject go in Groups) {
			Destroy(go);
		}

		Groups.Clear();

		if (segmented) {
			foreach (VoxGroup group in data.Groups) {
				// Add a group object which will be assigned the mesh of the current group being iterated over
				GameObject go = (GameObject)Instantiate(GameController.Instance.VoxPrefab, transform.position + group.Position, Quaternion.identity);

				go.GetComponent<ModelController>().LoadGroup(group);

				go.transform.parent = transform;

				Groups.Add(go);
			}
		}
		else {
			GameObject go = (GameObject)Instantiate(GameController.Instance.VoxPrefab, transform.position, Quaternion.identity);

			go.GetComponent<ModelController>().LoadData(data);

			go.transform.parent = transform;

			Groups.Add(go);
		}
	}

	internal void PlayAnimation(Scd_Data data) {
		// TODO: Write queue animations to be played.
	} 

	void LoadData(VoxData data) {
		DoData = true;
		this.Data = data;
	}

	void LoadGroup(VoxGroup group) {
		DoGroup = true;
		this.Group = group;
	}

	#region animation

	IEnumerator Animate(Scd_Data data) {
		float rate = data.rate;
		float start_time = Time.time;

		bool animate = true;
		while (animate) {

			// this only works since we're yielding inside of the loop and this is a coroutine.
			float time_passed = Mathf.Abs(Time.time - start_time);

			int frame = Mathf.CeilToInt(rate * time_passed);
			if (frame > data.frames.Length) break;

			foreach (Scd_Frame_Group group in data.frames[frame].groups) {
				ProcessGroupAnimation(group, data.flags);
			}

			yield return null;
		}
	}

	void ProcessGroupAnimation(Scd_Frame_Group group, List<ScdFlag> flags) {
		foreach (GameObject go in Groups) {
			ModelController mc = go.GetComponent<ModelController>();
			if (mc.Group.Name == group.group) {
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