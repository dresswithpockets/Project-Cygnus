using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

public static class Util {

	internal static string to_hex(this Color color) {

		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
		return hex;
	}

	internal static Color to_color(this string hex) {

		try {
			byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
			if (hex.Length > 6) {
				return new Color(r, g, b, byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber));
			}
			return new Color(r, g, b, 255);
		}
		catch (ArgumentNullException ane) {

			Debug.LogError("Attempted to parse a null string to Color.\n\n ~Exception Below~ \n" + ane.Message);
			return Color.black;
		}
		catch (FormatException fe) {
			
			Debug.LogError("Attempted to parse malformed hex string:" + hex + " to Color.\n\n ~Exception Below~ \n" + fe.Message);
			return Color.black;
		}
	}

	internal static Type[] get_all_types_with_inheriting_type(this Assembly assembly, Type inherit_type) {

		List<Type> res = new List<Type>();

		foreach (Type t in assembly.GetTypes()) {

			if (t.BaseType == inherit_type) {

				res.Add(t);
			}
		}

		return res.ToArray();
	}

	internal static Dictionary<GameObject, Renderer[]> renderer_dict = new Dictionary<GameObject, Renderer[]>();

	// TODO: Figure out a way to hide and show meshes on game items.

	internal static void set_render(this MonoBehaviour mb, bool render) {
		/*Renderer[] renderers;
		if (mb.GetComponentsInChildren<Renderer>() == null) {
			renderers = renderer_dict[mb.gameObject];
		} else {
			renderers = mb.GetComponentsInChildren<Renderer>();
			if (!renderer_dict.ContainsKey(mb.gameObject)) {
				renderer_dict.Add(mb.gameObject, renderers);
			}
		}
		foreach (Renderer r in renderers) {
			r.enabled = render;
		}

		Renderer[] renderers = mb.GetComponentsInChildren<Renderer>(true);
		foreach (Renderer r in renderers) {
			r.enabled = render;
		}*/
	}

	internal static void set_render(this GameObject go, bool render) {
		/*Renderer[] renderers;
		if (go.GetComponentsInChildren<Renderer>() == null) {
			renderers = renderer_dict[go.gameObject];
		}
		else {
			renderers = go.GetComponentsInChildren<Renderer>();
			if (!renderer_dict.ContainsKey(go)) {
				renderer_dict.Add(go, renderers);
			}
		}
		foreach (Renderer r in renderers) {
			r.enabled = render;
		}

		Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
		foreach (Renderer r in renderers) {
			r.enabled = render;
		}*/
	}

	internal static string to_line_string(this string[] data) {
		string end = "";

		for (int i = 0; i < data.Length; i++) {
			if (i == data.Length - 1 && end.EndsWith("\n")) end = end.Substring(0, end.Length);
			else end += data[i] + "\n";
		}

		return end;
	}

	public static float SqrDistance(Vector3 a, Vector3 b) {
		return (a - b).sqrMagnitude;
	}
	
	public static bool Contains(this object[] array, object other) {
		foreach (object obj in array)
			if (obj == other) return true;

		return false;
	}

	public static void Remove(this object[] array, object other) {
		for (int i = 0; i < array.Length; i++)
			if (array[i] == other) array[i] = null;
	}
}
