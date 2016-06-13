using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using SimpleJSON;

public struct Model {
	public string MagicNumber { get; set; }
	public int VersionNumber { get; set; }

	public ModelChunk MainChunk { get; set; }
	
	public Model(byte[] vox) {
		// MagicaVox standards: magicNumber = 4 bytes, versionNumber = 4 bytes

		// Set magic number; From byte 0 to byte 3 (4 bytes) -> cant use BitConverter since MagicNumber is big endian
		MagicNumber = "    ";
		StringBuilder builder = new StringBuilder(4);
		for (int i = 0; i < 4; i++) {
			builder[i] = Convert.ToChar(vox[i]);
		}
		MagicNumber = builder.ToString();

		// Set version number; From byte 4 to byte 7 (4 bytes)
		VersionNumber = BitConverter.ToInt32(vox, 4);

		

		MainChunk = new ModelChunk(vox, 8);
	}
}

public struct ModelChunk {

	// Header
	public int ChunkID { get; set; }
	public int ChunkSize { get; set; }
	public int ChildrenSize { get; set; }

	public byte[] Body { get; set; }

	public ModelChunk[] Children { get; set; }

	public int ActualChunkSize { get; set; }

	public ModelChunk(byte[] chunk, int startIndex) {

		ActualChunkSize = chunk.Length;

		int currentIndex = 0;
		ChunkID = BitConverter.ToInt32(chunk, currentIndex);

		ChunkSize = BitConverter.ToInt32(chunk, currentIndex += 4);

		ChildrenSize = BitConverter.ToInt32(chunk, currentIndex += 4);

		Body = new byte[ChunkSize];
		for (int i = 0; i < ChunkSize; i++) Body[i] = chunk[i + currentIndex];
		currentIndex += ChunkSize;

		List<ModelChunk> children = new List<ModelChunk>();
		while (currentIndex < ActualChunkSize) children.Add(new ModelChunk(chunk, currentIndex, out currentIndex));
		Children = children.ToArray();
	}

	public ModelChunk(byte[] chunk, int startIndex, out int newIndex) : this(chunk, startIndex) {
		newIndex = startIndex + ActualChunkSize;
	}
}


public struct VoxData {

	public string Name { get; set; }
	public VoxGroup[] Groups { get; set; }

	public Vector3[] GetVertices() {
		List<Vector3> verts = new List<Vector3>();

		for (int i = 0; i < Groups.Length; i++) {

			Vector3[] group_verts = Groups[i].GetVertices();
			foreach (Vector3 v in group_verts) verts.Add(v + Groups[i].Position);
		}

		return verts.ToArray();
	}

	public Color[] GetColors() {
		List<Color> cols = new List<Color>();

		for (int i = 0; i < Groups.Length; i++) {

			Color[] group_cols = Groups[i].GetColors();
			foreach (Color c in group_cols) cols.Add(c);
		}

		return cols.ToArray();
	}

	public int[] GetTriangles() {
		List<int> tris = new List<int>();

		int base_index = 0;
		for (int i = 0; i < Groups.Length; i++) {

			int[] group_tris = Groups[i].GetTriangles();
			foreach (int t in group_tris) tris.Add(t + base_index);
			// We have to make sure that the triangles for the next group are actually paired with the verts from that group
			// this is done by adjusting the value of each triangle to the base index of the group's verts.
			base_index += Groups[i].GetVertices().Length;
		}

		return tris.ToArray();
	}

	public bool EmptyOrInvalid() {

		try {
			int vert_len = GetVertices().Length;
			int tri_len = GetTriangles().Length;
			return vert_len == 0 || tri_len == 0 || tri_len % 3 != 0;
		}
		catch (Exception e) { return true; }
	}

	public void PassToFilter(MeshFilter filter) {
		if (filter.mesh != null && !EmptyOrInvalid()) {
			filter.mesh.Clear();

			filter.mesh.vertices = GetVertices();
			filter.mesh.colors = GetColors();
			filter.mesh.triangles = GetTriangles();

			filter.mesh.RecalculateNormals();

			return;
		}

		Debug.LogWarning("The mesh member in the filter that was passed in to_filter is null. Is the filter unassigned?");
	}

	public VoxData(string name, string[] data) {
		List<VoxGroup> group_list = new List<VoxGroup>();

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

				group_list.Add(new VoxGroup(group_data.ToArray()));
			}
		}

		this.Name = name;
		Groups = group_list.ToArray();

		DebugConsole.Log(string.Format("Created model with {0} groups.", Groups.Length), true);
	}
}

// New alias for Segments: "Group"
public struct VoxGroup {

	public Vector3 Position { get; set; }
	public string Name { get; set; }

	public VoxChunk[] Chunks { get; set; }

	public Vector3[] GetVertices() {
		List<Vector3> verts = new List<Vector3>();

		for (int i = 0; i < Chunks.Length; i++) {

			// we add the position of the vert's chunk to adjust it relative to the chunk position.
			foreach (Vector3 v in Chunks[i].Vertices) verts.Add(v + Chunks[i].Position);
		}

		return verts.ToArray();
	}

	public Color[] GetColors() {
		List<Color> cols = new List<Color>();

		for (int i = 0; i < Chunks.Length; i++) {

			foreach (Color c in Chunks[i].Colors) cols.Add(c);
		}

		return cols.ToArray();
	}

	public int[] GetTriangles() {
		List<int> tris = new List<int>();

		int base_index = 0;
		for (int i = 0; i < Chunks.Length; i++) {

			foreach (int t in Chunks[i].Triangles) tris.Add(t + base_index);
			// We have to make sure that the triangles for the next chunk are actually paired with the verts from that chunk
			// this is done by adjusting the value of each triangle to the base index of the chunk's verts.
			base_index += Chunks[i].Vertices.Length;
		}

		return tris.ToArray();
	}

	public void PassToFilter(MeshFilter filter) {
		if (filter.mesh != null) {
			filter.mesh.Clear();

			filter.mesh.vertices = GetVertices();
			filter.mesh.colors = GetColors();
			filter.mesh.triangles = GetTriangles();

			filter.mesh.RecalculateNormals();

			return;
		}

		Debug.LogWarning("The mesh member in the filter that was passed in to_filter is null. Is the filter unassigned?");
	}

	/*
	Group structure:
	[group=name,(x,y,z)]
	*/
	public VoxGroup(string[] data) {
		List<VoxChunk> chunkList = new List<VoxChunk>();

		string groupID = data[0].Split('=')[1];
		string[] groupPos = groupID.Split('(')[1].Split(')')[0].Split(',');

		Name = groupID.Split(',')[0];
		Position = new Vector3(float.Parse(groupPos[0]), float.Parse(groupPos[1]), float.Parse(groupPos[2]));

		for (int i = 1; i < data.Length; i++) {
			string line = data[i].ToLower();

			if (line.StartsWith("[chunk=")) {

				List<string> chunkData = new List<string>();

				// This fancy little block adds the first "[chunk=" line to the chunk_data list
				// then loops through the data until it finds another "[chunk=" line.
				// The loop adds each of those lines to the chunk_data list, excluding the next "[chunk=" line.
				//
				//		-TH 4/20/2016
				//
				chunkData.Add(line);
				int next_chunk;
				for (next_chunk = i + 1; next_chunk < data.Length; next_chunk++) {

					if (data[next_chunk].StartsWith("[chunk=")) break;
					chunkData.Add(data[next_chunk]);
				}


				chunkList.Add(new VoxChunk(chunkData.ToArray()));
			}
		}

		Chunks = chunkList.ToArray();

		DebugConsole.Log(string.Format("Created group with {0} chunks.", Chunks.Length), true);
	}
}

public struct VoxChunk {

	public Vector3 Position { get; set; }

	public Vector3[] Vertices { get; set; }
	public Color[] Colors { get; set; }
	public int[] Triangles { get; set; }

	/*
	Chunk data structure:
	[chunk=(x,y,z)]
	[vrts]
	[cols]
	[tris]
	*/
	public VoxChunk(string[] data) {

		Debug.Log("Parsing chunk data: \n" + data.to_line_string() + "\ntest test");

		List<Vector3> verts = new List<Vector3>();
		List<Color> cols = new List<Color>();
		List<int> tris = new List<int>();
		ModelSection model_section = ModelSection.VERTS;

		string[] chunkPos = data[0].Split('=')[1].Split(')')[0].Split('(')[1].Split(',');
		Position = new Vector3(float.Parse(chunkPos[0]), float.Parse(chunkPos[1]), float.Parse(chunkPos[2]));

		for (int i = 1; i < data.Length; i++) {

			string line = data[i].Trim();

			if (line == "[vrts]") model_section = ModelSection.VERTS;
			else if (line == "[cols]") model_section = ModelSection.COLS;
			else if (line == "[tris]") model_section = ModelSection.TRIS;
			else {
				switch (model_section) {

					case ModelSection.VERTS:

						string[] axes = line.Split(' ');
						verts.Add(new Vector3(float.Parse(axes[0]), float.Parse(axes[1]), float.Parse(axes[2])));

						break;
					case ModelSection.COLS:

						cols.Add(line.to_color());

						break;
					case ModelSection.TRIS:

						tris.Add(int.Parse(line));

						break;
				}
			}
		}

		Vertices = verts.ToArray();
		Colors = cols.ToArray();
		Triangles = tris.ToArray();

		DebugConsole.Log(string.Format("Created chunk with {0} vertices, {1} colors, and {2} triangles.", Vertices.Length, Colors.Length, Triangles.Length / 3), true);
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
	private List<ScdFlag> m_flags;
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

	public List<ScdFlag> flags {
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
	//
	// This entire function for parsing the JSON associated with an exported animation will be changed drastically
	// and also implemented later in the Voxify software designed specifically for 3d modeling and animating.
	//
	//		-TH 5/4/2016
	//
	//
	public Scd_Data(string json) {
		JSONNode node = JSONNode.Parse(json);
		name = node["name"];
		rate = node["name"].AsFloat;

		flags = new List<ScdFlag>();

		foreach (int flag in node["flags"].AsArray) {
			if (!flags.Contains((ScdFlag)flag)) flags.Add((ScdFlag)flag);
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

						frames[current_frame].interp = (InterpType)kframe["interp"].AsInt;

						switch (frames[current_frame].interp) {
							case InterpType.ASSIGN:

								if (flags.Contains(ScdFlag.CHANGES_POSITION)) {
									JSONNode position = kframe["position"];
									frames[current_frame].groups[current_group].delta_position = new Vector3(position["x"].AsFloat, position["y"].AsFloat, position["z"].AsFloat);
								}
								if (flags.Contains(ScdFlag.CHANGES_ROTATION)) {
									JSONNode rotation = kframe["rotation"];
									frames[current_frame].groups[current_group].rotation = new Vector3(rotation["x"].AsFloat, rotation["y"].AsFloat, rotation["z"].AsFloat);
								}
								if (flags.Contains(ScdFlag.CHANGES_SCALE)) {
									JSONNode scale = kframe["scale"];
									frames[current_frame].groups[current_group].rotation = new Vector3(scale["x"].AsFloat, scale["y"].AsFloat, scale["z"].AsFloat);
								}
								break;
							case InterpType.LINEAR: // use Lerp

								// We want to start at the previous frame, and go backwards until we hit the same group again
								for (int fcheck = current_frame - 1; fcheck > -1; fcheck--) {
									if (fcheck == 0) {
										if (flags.Contains(ScdFlag.CHANGES_POSITION)) {

											Vector3 prev_pos = new Vector3(0, 0, 0);
											JSONNode json_position = kframe["position"];
											Vector3 new_pos = new Vector3(json_position["x"].AsFloat, json_position["y"].AsFloat, json_position["z"].AsFloat);
											frames[current_frame].groups[current_group].delta_position = Vector3.Lerp(prev_pos, new_pos, completion);
										}
										if (flags.Contains(ScdFlag.CHANGES_ROTATION)) {

											Vector3 prev_rot = new Vector3(0, 0, 0);
											JSONNode json_rotation = kframe["rotation"];
											Vector3 new_rot = new Vector3(json_rotation["x"].AsFloat, json_rotation["y"].AsFloat, json_rotation["z"].AsFloat);
											frames[current_frame].groups[current_group].rotation = Vector3.Lerp(prev_rot, new_rot, completion);
										}
										if (flags.Contains(ScdFlag.CHANGES_SCALE)) {

											Vector3 prev_scale = new Vector3(0, 0, 0);
											JSONNode json_scale = kframe["scale"];
											Vector3 new_scale = new Vector3(json_scale["x"].AsFloat, json_scale["y"].AsFloat, json_scale["z"].AsFloat);
											frames[current_frame].groups[current_group].scale = Vector3.Lerp(prev_scale, new_scale, completion);
										}
									}
									else {
										foreach (Scd_Frame_Group group in frames[fcheck].groups) {
											if (group.group == frames[current_frame].groups[current_group].group) {
												if (flags.Contains(ScdFlag.CHANGES_POSITION)) {

													Vector3 prev_pos = group.delta_position;
													JSONNode json_position = kframe["position"];
													Vector3 new_pos = new Vector3(json_position["x"].AsFloat, json_position["y"].AsFloat, json_position["z"].AsFloat);
													frames[current_frame].groups[current_group].delta_position = Vector3.Lerp(Vector3.zero, new_pos, completion);
												}
												if (flags.Contains(ScdFlag.CHANGES_ROTATION)) {

													Vector3 prev_rot = group.rotation;
													JSONNode json_rotation = kframe["rotation"];
													Vector3 new_rot = new Vector3(json_rotation["x"].AsFloat, json_rotation["y"].AsFloat, json_rotation["z"].AsFloat);
													frames[current_frame].groups[current_group].rotation = Vector3.Lerp(prev_rot, new_rot, completion);
												}
												if (flags.Contains(ScdFlag.CHANGES_SCALE)) {

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
							case InterpType.SPHERICAL: // use Slerp instead of Lerp

								for (int fcheck = current_frame - 1; fcheck > -1; fcheck--) {
									if (fcheck == 0) {
										if (flags.Contains(ScdFlag.CHANGES_POSITION)) {

											Vector3 prev_pos = new Vector3(0, 0, 0);
											JSONNode json_position = kframe["position"];
											Vector3 new_pos = new Vector3(json_position["x"].AsFloat, json_position["y"].AsFloat, json_position["z"].AsFloat);
											frames[current_frame].groups[current_group].delta_position = Vector3.Slerp(prev_pos, new_pos, completion);
										}
										if (flags.Contains(ScdFlag.CHANGES_ROTATION)) {

											Vector3 prev_rot = new Vector3(0, 0, 0);
											JSONNode json_rotation = kframe["rotation"];
											Vector3 new_rot = new Vector3(json_rotation["x"].AsFloat, json_rotation["y"].AsFloat, json_rotation["z"].AsFloat);
											frames[current_frame].groups[current_group].rotation = Vector3.Slerp(prev_rot, new_rot, completion);
										}
										if (flags.Contains(ScdFlag.CHANGES_SCALE)) {

											Vector3 prev_scale = new Vector3(0, 0, 0);
											JSONNode json_scale = kframe["scale"];
											Vector3 new_scale = new Vector3(json_scale["x"].AsFloat, json_scale["y"].AsFloat, json_scale["z"].AsFloat);
											frames[current_frame].groups[current_group].scale = Vector3.Slerp(prev_scale, new_scale, completion);
										}
									}
									else {
										foreach (Scd_Frame_Group group in frames[fcheck].groups) {
											if (group.group == frames[current_frame].groups[current_group].group) {
												if (flags.Contains(ScdFlag.CHANGES_POSITION)) {

													Vector3 prev_pos = group.delta_position;
													JSONNode json_position = kframe["position"];
													Vector3 new_pos = new Vector3(json_position["x"].AsFloat, json_position["y"].AsFloat, json_position["z"].AsFloat);
													frames[current_frame].groups[current_group].delta_position = Vector3.Slerp(prev_pos, new_pos, completion);
												}
												if (flags.Contains(ScdFlag.CHANGES_ROTATION)) {

													Vector3 prev_rot = group.rotation;
													JSONNode json_rotation = kframe["rotation"];
													Vector3 new_rot = new Vector3(json_rotation["x"].AsFloat, json_rotation["y"].AsFloat, json_rotation["z"].AsFloat);
													frames[current_frame].groups[current_group].rotation = Vector3.Slerp(prev_rot, new_rot, completion);
												}
												if (flags.Contains(ScdFlag.CHANGES_SCALE)) {

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
	public InterpType interp;
	public Scd_Frame_Group[] groups;
}

public class Scd_Frame_Group {
	public string group;
	public int current_gframe;
	public int gframe_count;
	public Vector3 delta_position;
	public Vector3 rotation;
	public Vector3 scale;
}