using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldLabel : MonoBehaviour {

	private Text m_Label;

	public string Text {
		get {
			if (m_Label == null) m_Label = GetComponentInChildren<Text>();
			return m_Label.text;
		}
		set {
			if (m_Label == null) m_Label = GetComponentInChildren<Text>();
			m_Label.text = value;
		}
	}
}
