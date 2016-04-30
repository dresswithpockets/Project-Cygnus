using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;

public struct Vox_Data {
	
	public string name;
	public Vox_Group[] groups;

	public Vector3[] get_vertices() {
		List<Vector3> verts = new List<Vector3>();

		for (int i = 0; i < groups.Length; i++) {

			Vector3[] group_verts = groups[i].get_vertices();
			foreach (Vector3 v in group_verts) verts.Add(v + groups[i].position);
		}

		return verts.ToArray();
	}

	public Color[] get_colors() {
		List<Color> cols = new List<Color>();

		for (int i = 0; i < groups.Length; i++) {

			Color[] group_cols = groups[i].get_colors();
			foreach (Color c in group_cols) cols.Add(c);
		}

		return cols.ToArray();
	}

	public int[] get_triangles() {
		List<int> tris = new List<int>();

		int base_index = 0;
		for (int i = 0; i < groups.Length; i++) {

			int[] group_tris = groups[i].get_triangles();
			foreach (int t in group_tris) tris.Add(t + base_index);
			// We have to make sure that the triangles for the next group are actually paired with the verts from that group
			// this is done by adjusting the value of each triangle to the base index of the group's verts.
			base_index += groups[i].get_vertices().Length;
		}

		return tris.ToArray();
	}

	public bool empty_or_invalid() {

		try {
			int vert_len = get_vertices().Length;
			int tri_len = get_triangles().Length;
			return vert_len == 0 || tri_len == 0 || tri_len % 3 != 0;
		}
		catch (Exception e) { return true; }
	}

	public void to_filter(ref MeshFilter filter) {
		if (filter.mesh != null && !empty_or_invalid()) {
			filter.mesh.Clear();

			filter.mesh.vertices = get_vertices();
			filter.mesh.colors = get_colors();
			filter.mesh.triangles = get_triangles();

			filter.mesh.RecalculateNormals();
			
			return;
		}

		Debug.LogWarning("The mesh member in the filter that was passed in to_filter is null. Is the filter unassigned?");
	}

	public Vox_Data(string name, string[] data) {
		List<Vox_Group> group_list = new List<Vox_Group>();

		for (int i = 0; i < data.Length; i++) {
			string line = data[i].ToLower().Trim();

			DebugConsole.Log("Checking group scope: " + line, true);

			if (line.StartsWith("[group=") || line.StartsWith("[segment=")) {

				DebugConsole.Log("Entering group scope", true);

				List<string> group_data = new List<string>();

				// This fancy little block adds the first group ("[group=" or "[segment=") line to the chunk_data list
				// then loops through the data until it finds another group line.
				// The loop adds each of those lines to the group_data list, excluding the next group line.
				//
				//		-TH 4/20/2016
				//
				group_data.Add(line);
				DebugConsole.Log("Added line: " + line, true);
				int next_group;
				for (next_group = i + 1; next_group < data.Length; next_group++) {

					if (data[next_group].StartsWith("[segment=") || data[next_group].StartsWith("[group=")) break;
					group_data.Add(data[next_group]);
				}

				i = next_group;

				group_list.Add(new Vox_Group(group_data.ToArray()));
			}
		}

		this.name = name;
		groups = group_list.ToArray();

		DebugConsole.Log(string.Format("Created model with {0} groups.", groups.Length), true);
	}
}

// New alias for Segments: "Group"
public struct Vox_Group { 

	public Vector3 position;
	public string name;

	public Vox_Chunk[] chunks;

	public Vector3[] get_vertices() {
		List<Vector3> verts = new List<Vector3>();

		for (int i = 0; i < chunks.Length; i++) {

			// we add the position of the vert's chunk to adjust it relative to the chunk position.
			foreach (Vector3 v in chunks[i].vertices) verts.Add(v + chunks[i].position); 
		}

		return verts.ToArray();
	}

	public Color[] get_colors() {
		List<Color> cols = new List<Color>();

		for (int i = 0; i < chunks.Length; i++) {

			foreach (Color c in chunks[i].colors) cols.Add(c);
		}

		return cols.ToArray();
	}

	public int[] get_triangles() {
		List<int> tris = new List<int>();

		int base_index = 0;
		for (int i = 0; i < chunks.Length; i++) {

			foreach (int t in chunks[i].triangles) tris.Add(t + base_index);
			// We have to make sure that the triangles for the next chunk are actually paired with the verts from that chunk
			// this is done by adjusting the value of each triangle to the base index of the chunk's verts.
			base_index += chunks[i].vertices.Length; 
		}

		return tris.ToArray();
	}

	public void to_filter(ref MeshFilter filter) {
		if (filter.mesh != null) {
			filter.mesh.Clear();

			filter.mesh.vertices = get_vertices();
			filter.mesh.colors = get_colors();
			filter.mesh.triangles = get_triangles();

			filter.mesh.RecalculateNormals();

			return;
		}

		Debug.LogWarning("The mesh member in the filter that was passed in to_filter is null. Is the filter unassigned?");
	}

	/*
	Group structure:
	[group=name,(x,y,z)]
	*/
	public Vox_Group(string[] data) {
		List<Vox_Chunk> chunk_list = new List<Vox_Chunk>();

		string group_id = data[0].Split('=')[1];
		string[] group_pos = group_id.Split('(')[1].Split(')')[0].Split(',');

		name = group_id.Split(',')[0];
		position = new Vector3(float.Parse(group_pos[0]), float.Parse(group_pos[1]), float.Parse(group_pos[2]));

		for (int i = 1; i < data.Length; i++) {
			string line = data[i].ToLower();
			
			if (line.StartsWith("[chunk=")) {
				
				List<string> chunk_data = new List<string>();

				// This fancy little block adds the first "[chunk=" line to the chunk_data list
				// then loops through the data until it finds another "[chunk=" line.
				// The loop adds each of those lines to the chunk_data list, excluding the next "[chunk=" line.
				//
				//		-TH 4/20/2016
				//
				chunk_data.Add(line);
				int next_chunk;
				for (next_chunk = i + 1; next_chunk < data.Length; next_chunk++) {

					if (data[next_chunk].StartsWith("[chunk=")) break;
					chunk_data.Add(data[next_chunk]);
				}
				

				chunk_list.Add(new Vox_Chunk(chunk_data.ToArray()));
			}
		}

		chunks = chunk_list.ToArray();

		DebugConsole.Log(string.Format("Created group with {0} chunks.", chunks.Length), true);
	}
}

public struct Vox_Chunk {

	public Vector3 position;

	public Vector3[] vertices;
	public Color[] colors;
	public int[] triangles;

	/*
	Chunk data structure:
	[chunk=(x,y,z)]
	[vrts]
	[cols]
	[tris]
	*/
	public Vox_Chunk(string[] data) {

		Debug.Log("Parsing chunk data: \n" + data.to_line_string() + "\ntest test");

		List<Vector3> verts = new List<Vector3>();
		List<Color> cols = new List<Color>();
		List<int> tris = new List<int>();
		Model_Section model_section = Model_Section.VERTS;
		
		string[] chunk_pos = data[0].Split('=')[1].Split(')')[0].Split('(')[1].Split(',');
		position = new Vector3(float.Parse(chunk_pos[0]), float.Parse(chunk_pos[1]), float.Parse(chunk_pos[2]));

		for (int i = 1; i < data.Length; i++) {

			string line = data[i].Trim();
			
			if (line == "[vrts]") model_section = Model_Section.VERTS;
			else if (line == "[cols]") model_section = Model_Section.COLS;
			else if (line == "[tris]") model_section = Model_Section.TRIS;
			else {
				switch (model_section) {

					case Model_Section.VERTS:

						string[] axes = line.Split(' ');
						verts.Add(new Vector3(float.Parse(axes[0]), float.Parse(axes[1]), float.Parse(axes[2])));

						break;
					case Model_Section.COLS:

						cols.Add(line.to_color());

						break;
					case Model_Section.TRIS:

						tris.Add(int.Parse(line));

						break;
				}
			}
		}

		vertices = verts.ToArray();
		colors = cols.ToArray();
		triangles = tris.ToArray();

		DebugConsole.Log(string.Format("Created chunk with {0} vertices, {1} colors, and {2} triangles.", vertices.Length, colors.Length, triangles.Length / 3), true);
	}
}

// TODO: Implement SCD file structure and loading.
// Scd_Data is the structure implementation of SCD file types, which store information for Vox model animation.
/*

	Test SCD File structure:
	$rate=60, // identifies the framerate of the animation
	$flags=(CHANGES_POSITION,CHANGES_ROTATION,CHANGES_SCALE,), // identifies certain flags for the animation, anim flags can be found in Scd_Flags in Enums.cs
	$key=( // starts new keyframe
	$start=0, // identifies at what frame this keyframe starts.
	$frames=5, // identifies the length of this keyframe.
	$interp=LINEAR, // identifies the kind of interpolation this frame receives. interp types can be found in Interp_Type in Enums.cs
	$groups=[ // identifies which groups of the model are affected by this frame
	"group1",
	"group2",
	],
	$position=(0.0,4.0,9.0), // identifies the position that the groups will interp translate to. This position is relative to the position of each group, not to the model.
	$rotation=(0.0,180.0,0.0), // identifies the rotation that the groups will interp orient towards.
	$scale=(0.9,0.9,0.9), // identifies the new scale that the groups will interp scale to.
	)

	Simplified structure:
	rate=60,
	flags=(CHANGES_POSITION,CHANGES_ROTATION,CHANGES_SCALE,),
	key=(start=0,frames=5,interp=LINEAR,groups=["group1","group2",],position=(0.0,4.0,9.0),rotation=(0.0,180.0,0.0),scale=(0.9,0.9,0.9),),

	Switching to JSON because JSON is OP asf:
	{
		name: "Name of animation",
		flags: [
			"0", // Changes_Position
			"1", // Changes_Rotation
			"2" // Changes_Scale
		],
		keys: [
			{
				start: "0",
				frames: "5",
				interp: "1", // Assign = 0, Linear = 1, Spherical = 2
				groups: [
					"group1",
					"group2"
				],
				position: {
					x: "0.0",
					y: "4.0",
					z: "9.0"
				},
				rotation: {
					x: "0.0",
					y: "180.0",
					z: "0.0",
				},
				scale: {
					x: "0.9",
					y: "0.9",
					z: "0.0"
				}
			},
		]
	}
*/
public class Scd_Data {
	
	private string m_name;
	private float m_rate;
	private List<Scd_Flag> m_flags;
	private Scd_Frame[] m_frames;

	public string name {
		get {
			return m_name;
		}
		internal set {
			m_name = value;
		}
	}

	public float rate {
		get {
			return m_rate;
		}
		internal set {
			m_rate = value;
		}
	}

	public List<Scd_Flag> flags {
		get {
			return m_flags;
		}
		internal set {
			m_flags = value;
		}
	}

	public Scd_Frame[] frames {
		get {
			return m_frames;
		}
		internal set {
			m_frames = value;
		}
	}

	// It is important to note that all translations, reorientations and scales
	// are relative to the original position of the group rather than the world, at the start of the animation.
	// So the result of the JSON animation:
	//
	// position: {
	//	x: "0.0",
	//	y: "4.0",
	//	z: "9.0"
	// },
	//
	// would be the same as doing transform.Translate(new Vector3(0f, 4f, 9f), Space.Self);
	//
	//		-TH 4/29/2016
	//
	public Scd_Data(string json) {
		JSONNode node = JSONNode.Parse(json);
		name = node["name"];
		rate = node["name"].AsFloat;

		flags = new List<Scd_Flag>();

		foreach (int flag in node["flags"].AsArray) {
			if (!flags.Contains((Scd_Flag)flag)) flags.Add((Scd_Flag)flag);
		}

		int anim_length = 0;

		// I'm genuinely not sure how to explain this loop in plain english.
		foreach (JSONNode key in node["keys"].AsArray) {
			int current_length = (key["start"].AsInt + key["frames"].AsInt - 1);
			if (current_length > anim_length) anim_length = current_length;
		}

		frames = new Scd_Frame[anim_length];
		for (int i = 0; i < anim_length; i++) frames[i] = null;
		
		for (int current_frame = 0; current_frame < frames.Length; current_frame++) {
			// kframe is the current keyframe we're scoping into
			foreach (JSONNode kframe in node["keys"].AsArray) {
				// Is the current frame a frame capture by the key being iterated through.
				if (current_frame >= kframe["start"].AsInt && current_frame < (kframe["start"].AsInt + kframe["frames"].AsInt)) {
					frames[current_frame] = new Scd_Frame();
					frames[current_frame].groups = new Scd_Frame_Group[kframe["groups"].AsArray.Count];

					for (int current_group = 0; current_group < frames[current_frame].groups.Length; current_group++) {
						frames[current_frame].groups[current_group] = new Scd_Frame_Group();

						frames[current_frame].groups[current_group].group = kframe["groups"].AsArray[current_group];

						// Values used to determine a value completion from 0.0 to 1.0
						frames[current_frame].groups[current_group].gframe_count = kframe["frames"].AsInt;
						frames[current_frame].groups[current_group].current_gframe = Mathf.Abs(kframe["start"].AsInt - current_frame) + 1;
						float completion = frames[current_frame].groups[current_group].current_gframe / frames[current_frame].groups[current_group].gframe_count;

						switch ((Interp_Type)kframe["interp"].AsInt) {
							case Interp_Type.ASSIGN:

								if (flags.Contains(Scd_Flag.CHANGES_POSITION)) {
									JSONNode position = kframe["position"];
									frames[current_frame].groups[current_group].position = new Vector3(position["x"].AsFloat, position["y"].AsFloat, position["z"].AsFloat);
								}
								if (flags.Contains(Scd_Flag.CHANGES_ROTATION)) {
									JSONNode rotation = kframe["rotation"];
									frames[current_frame].groups[current_group].rotation = new Vector3(rotation["x"].AsFloat, rotation["y"].AsFloat, rotation["z"].AsFloat);
								}
								if (flags.Contains(Scd_Flag.CHANGES_SCALE)) {
									JSONNode scale = kframe["scale"];
									frames[current_frame].groups[current_group].rotation = new Vector3(scale["x"].AsFloat, scale["y"].AsFloat, scale["z"].AsFloat);
								}
								break;
							case Interp_Type.LINEAR: // use Lerp

								for (int fcheck = current_frame; fcheck > -1; fcheck--) {
									if (fcheck == 0) {
										if (flags.Contains(Scd_Flag.CHANGES_POSITION)) {

											Vector3 prev_pos = new Vector3(0, 0, 0);
											JSONNode json_position = kframe["position"];
											Vector3 new_pos = new Vector3(json_position["x"].AsFloat, json_position["y"].AsFloat, json_position["z"].AsFloat);
											frames[current_frame].groups[current_group].position = Vector3.Lerp(prev_pos, new_pos, completion);
                                        }
										if (flags.Contains(Scd_Flag.CHANGES_ROTATION)) {

											Vector3 prev_rot = new Vector3(0, 0, 0);
											JSONNode json_rotation = kframe["rotation"];
											Vector3 new_rot = new Vector3(json_rotation["x"].AsFloat, json_rotation["y"].AsFloat, json_rotation["z"].AsFloat);
											frames[current_frame].groups[current_group].rotation = Vector3.Lerp(prev_rot, new_rot, completion);
										}
										if (flags.Contains(Scd_Flag.CHANGES_SCALE)) {

											Vector3 prev_scale = new Vector3(0, 0, 0);
											JSONNode json_scale = kframe["scale"];
											Vector3 new_scale = new Vector3(json_scale["x"].AsFloat, json_scale["y"].AsFloat, json_scale["z"].AsFloat);
											frames[current_frame].groups[current_group].scale = Vector3.Lerp(prev_scale, new_scale, completion);
										}
									}
									else {
										foreach (Scd_Frame_Group group in frames[fcheck].groups) {
											if (group.group == frames[current_frame].groups[current_group].group) {
												if (flags.Contains(Scd_Flag.CHANGES_POSITION)) {

													Vector3 prev_pos = group.position;
													JSONNode json_position = kframe["position"];
													Vector3 new_pos = new Vector3(json_position["x"].AsFloat, json_position["y"].AsFloat, json_position["z"].AsFloat);
													frames[current_frame].groups[current_group].position = Vector3.Lerp(prev_pos, new_pos, completion);
												}
												if (flags.Contains(Scd_Flag.CHANGES_ROTATION)) {

													Vector3 prev_rot = group.rotation;
													JSONNode json_rotation = kframe["rotation"];
													Vector3 new_rot = new Vector3(json_rotation["x"].AsFloat, json_rotation["y"].AsFloat, json_rotation["z"].AsFloat);
													frames[current_frame].groups[current_group].rotation = Vector3.Lerp(prev_rot, new_rot, completion);
												}
												if (flags.Contains(Scd_Flag.CHANGES_SCALE)) {

													Vector3 prev_scale = group.scale;
													JSONNode json_scale = kframe["scale"];
													Vector3 new_scale = new Vector3(json_scale["x"].AsFloat, json_scale["y"].AsFloat, json_scale["z"].AsFloat);
													frames[current_frame].groups[current_group].scale = Vector3.Lerp(prev_scale, new_scale, completion);
												}
											}
										}
									}
								}
								break;
							case Interp_Type.SPHERICAL: // use Slerp instead of Lerp

								for (int fcheck = current_frame; fcheck > -1; fcheck--) {
									if (fcheck == 0) {
										if (flags.Contains(Scd_Flag.CHANGES_POSITION)) {

											Vector3 prev_pos = new Vector3(0, 0, 0);
											JSONNode json_position = kframe["position"];
											Vector3 new_pos = new Vector3(json_position["x"].AsFloat, json_position["y"].AsFloat, json_position["z"].AsFloat);
											frames[current_frame].groups[current_group].position = Vector3.Slerp(prev_pos, new_pos, completion);
										}
										if (flags.Contains(Scd_Flag.CHANGES_ROTATION)) {

											Vector3 prev_rot = new Vector3(0, 0, 0);
											JSONNode json_rotation = kframe["rotation"];
											Vector3 new_rot = new Vector3(json_rotation["x"].AsFloat, json_rotation["y"].AsFloat, json_rotation["z"].AsFloat);
											frames[current_frame].groups[current_group].rotation = Vector3.Slerp(prev_rot, new_rot, completion);
										}
										if (flags.Contains(Scd_Flag.CHANGES_SCALE)) {

											Vector3 prev_scale = new Vector3(0, 0, 0);
											JSONNode json_scale = kframe["scale"];
											Vector3 new_scale = new Vector3(json_scale["x"].AsFloat, json_scale["y"].AsFloat, json_scale["z"].AsFloat);
											frames[current_frame].groups[current_group].scale = Vector3.Slerp(prev_scale, new_scale, completion);
										}
									}
									else {
										foreach (Scd_Frame_Group group in frames[fcheck].groups) {
											if (group.group == frames[current_frame].groups[current_group].group) {
												if (flags.Contains(Scd_Flag.CHANGES_POSITION)) {

													Vector3 prev_pos = group.position;
													JSONNode json_position = kframe["position"];
													Vector3 new_pos = new Vector3(json_position["x"].AsFloat, json_position["y"].AsFloat, json_position["z"].AsFloat);
													frames[current_frame].groups[current_group].position = Vector3.Slerp(prev_pos, new_pos, completion);
												}
												if (flags.Contains(Scd_Flag.CHANGES_ROTATION)) {

													Vector3 prev_rot = group.rotation;
													JSONNode json_rotation = kframe["rotation"];
													Vector3 new_rot = new Vector3(json_rotation["x"].AsFloat, json_rotation["y"].AsFloat, json_rotation["z"].AsFloat);
													frames[current_frame].groups[current_group].rotation = Vector3.Slerp(prev_rot, new_rot, completion);
												}
												if (flags.Contains(Scd_Flag.CHANGES_SCALE)) {

													Vector3 prev_scale = group.scale;
													JSONNode json_scale = kframe["scale"];
													Vector3 new_scale = new Vector3(json_scale["x"].AsFloat, json_scale["y"].AsFloat, json_scale["z"].AsFloat);
													frames[current_frame].groups[current_group].scale = Vector3.Slerp(prev_scale, new_scale, completion);
												}
											}
										}
									}
								}
								break;
						}
					}
				}
			}
		}
	}
}

public class Scd_Frame {
	public Scd_Frame_Group[] groups;
}

public class Scd_Frame_Group {
	public string group;
	public int current_gframe;
	public int gframe_count;
	public Vector3 position;
	public Vector3 rotation;
	public Vector3 scale;
}