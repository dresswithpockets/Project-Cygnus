using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

public static class Util {
	
	public static string color_to_hex(this Color color) {

		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
		return hex;
	}

	public static Color hex_to_color(this string hex) {

		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		if (hex.Length > 6) {
			return new Color32(r, g, b, byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber));
		}
		return new Color32(r, g, b, 255);
	}

	public static Type[] get_all_types_with_inheriting_type(this Assembly assembly, Type inheritingType) {
		List<Type> res = new List<Type>();

		foreach (Type t in assembly.GetTypes()) {
			if (t.BaseType == inheritingType) {
				res.Add(t);
			}
		}

		return res.ToArray();
	}

	public static bool is_weapon(this Item_Template item) {
		return typeof(Weapon_Template) == item.GetType();
	}

	public static bool is_material(this Item_Template item) {
		return typeof(Material_Template) == item.GetType();
	}

	public static bool is_pet_item(this Item_Template item) {
		return typeof(Pet_Item_Template) == item.GetType();
	}

	public static bool is_equipment(this Item_Template item) {
		return typeof(Equipment_Template) == item.GetType();
	}

	public static bool is_consumable(this Item_Template item) {
		return typeof(Consumable_Template) == item.GetType();
	}
}
