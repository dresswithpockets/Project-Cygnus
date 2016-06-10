using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIController : MonoBehaviour {

	private PlayerController m_Player;

	public Slider HealthSlider;
	public Image HealthFill;
	public Text HealthNumberText;

	public Slider MagicSlider;
	public Image MagicFill;
	public Text MagicNumberText;

	public GameObject StaminaObject;
	public Slider StaminaSlider;
	public Image StaminaFill;

	public Text InteractText;

	void Start() {
		m_Player = GetComponent<PlayerController>();
	}

	void Update() {
		HealthSlider.value = (m_Player.HP / m_Player.MaxHealth);
		HealthNumberText.text = Mathf.CeilToInt(m_Player.HP).ToString() + "/" + Mathf.CeilToInt(m_Player.MaxHealth).ToString();
		HealthFill.enabled = !Mathf.Approximately(m_Player.HP, 0f);

		MagicSlider.value = (m_Player.MP / m_Player.MaxMagic);
		MagicNumberText.text = Mathf.CeilToInt(m_Player.MP).ToString() + "/" + Mathf.CeilToInt(m_Player.MaxMagic).ToString();
		MagicFill.enabled = !Mathf.Approximately(m_Player.MP, 0f);

		if (m_Player.Stamina < m_Player.MaxStamina && !StaminaObject.activeInHierarchy) {
			StaminaObject.SetActive(true);
			StaminaSlider.value = (m_Player.Stamina / m_Player.MaxStamina);
		}
		else if (StaminaObject.activeInHierarchy) {
			if (!(m_Player.Stamina < m_Player.MaxStamina)) StaminaObject.SetActive(false);
			else StaminaSlider.value = (m_Player.Stamina / m_Player.MaxStamina);

			StaminaFill.enabled = !(Mathf.Approximately(m_Player.Stamina, 0f) || m_Player.Stamina < 0f);
		}
	}

	public void SetInteractText(string value, bool show = false) {
		InteractText.text = "[" + vp_Input.Instance.Buttons["Use"].ToString() + "] " + value;
		ToggleInteractText(show);
	}

	public void ToggleInteractText(bool show) {
		InteractText.enabled = show;
	}
}
