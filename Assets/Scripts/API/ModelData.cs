using UnityEngine;
using System;
using System.Collections.Generic;

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

		Debug.LogWarning("The mesh member in the filter that was passed in ToFilter is null. Is the filter unassigned?");
	}

	public Vox_Data(string name, string[] data) {
		List<Vox_Group> group_list = new List<Vox_Group>();

		for (int i = 0; i < data.Length; i++) {
			string line = data[i].ToLower().Trim();

			Debug.Log("Checking group scope: " + line);

			if (line.StartsWith("[group=") || line.StartsWith("[segment=")) {

				Debug.Log("Entering group scope");

				List<string> group_data = new List<string>();

				// This fancy little block adds the first group ("[group=" or "[segment=") line to the chunk_data list
				// then loops through the data until it finds another group line.
				// The loop adds each of those lines to the group_data list, excluding the next group line.
				//
				//		-TH 4/20/2016
				//
				group_data.Add(line);
				Debug.Log("Added line: " + line);
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

		Debug.Log(string.Format("Created model with {0} groups.", groups.Length));
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

		Debug.Log(string.Format("Created group with {0} chunks.", chunks.Length));
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

		Debug.Log(string.Format("Created chunk with {0} vertices, {1} colors, and {2} triangles.", vertices.Length, colors.Length, triangles.Length / 3));
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
*/
public struct Scd_Data {

	public string name;
	public float rate;
	public Scd_Flags flags;
	public Scd_Key_Collection[] frames;
}

public struct Scd_Key_Collection {

}

public struct Scd_Key {

}