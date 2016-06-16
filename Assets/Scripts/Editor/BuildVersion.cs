using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System;

[InitializeOnLoad]
public class Build {

	static Build() {
		EditorApplication.update += RunOnce;
	}

	static void RunOnce() {
		EditorApplication.update -= RunOnce;
		ReadVersionAndIncrement();
	}

	static void ReadVersionAndIncrement() {
		if (!File.Exists("version")) {
			using (FileStream fs = File.Create("version")) {
				byte[] v = BitConverter.GetBytes(1);

				for (int i = 0; i < v.Length; i++) fs.WriteByte(v[i]);
			}
			return;
		}

		using (FileStream fs = File.Open("version", FileMode.Open)) {
			byte[] newBuildVersion = BitConverter.GetBytes(BitConverter.ToInt32(Util.ReadToEnd(fs), 0) + 1);
			fs.Write(newBuildVersion, 0, 4);
		}
	}

	static void BuildGame() {
		BuildPipeline.BuildPlayer(new string[] { "Assets/Scenes/DebugScene.unity" }, "Build/Windows/Cygnus.exe", BuildTarget.StandaloneWindows, BuildOptions.Development);
		File.Copy("version", "Build/Windows/version");
	}
}
