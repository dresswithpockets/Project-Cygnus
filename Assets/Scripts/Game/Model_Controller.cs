using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model_Controller : MonoBehaviour {

	public List<GameObject> groups = new List<GameObject>();

	public MeshFilter filter;

	public bool do_data = false;
	public bool do_group = false;
	public Vox_Data data;
	public Vox_Group group;

	void Start() {
		filter = GetComponent<MeshFilter>();
		if (do_data && do_group) {
			DebugConsole.Log("Can't load meshes for both group and data. Using group mesh.", Debug_Warning_Level.WARNING);
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
				GameObject go = (GameObject)Instantiate(Game_Controller.instance.vox_prefab, transform.position + group.position, Quaternion.identity);

				go.GetComponent<Model_Controller>().load_group(group);

				go.transform.parent = transform;

				groups.Add(go);
			}
		}
		else {
			GameObject go = (GameObject)Instantiate(Game_Controller.instance.vox_prefab, transform.position, Quaternion.identity);

			go.GetComponent<Model_Controller>().load_data(data);

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
}
