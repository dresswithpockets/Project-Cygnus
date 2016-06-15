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

	#region Stat Texts

	public Text ArmorValue;
	public Text ResiValue;
	public Text DexValue;
	public Text CritValue;
	public Text TempoValue;
	public Text RegValue;
	public Text LevelValue;
	public Text PowerValue;

	#endregion

	void Start() {
		m_Player = GetComponent<PlayerController>();
	}

	void Update() {
		HealthSlider.value = (m_Player.HP / m_Player.MaxHealth);
		HealthNumberText.text = Mathf.FloorToInt(m_Player.HP).ToString() + "/" + Mathf.FloorToInt(m_Player.MaxHealth).ToString();
		HealthFill.enabled = !Mathf.Approximately(m_Player.HP, 0f);

		MagicSlider.value = (m_Player.MP / m_Player.MaxMagic);
		MagicNumberText.text = Mathf.FloorToInt(m_Player.MP).ToString() + "/" + Mathf.FloorToInt(m_Player.MaxMagic).ToString();
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

		ArmorValue.text = m_Player.Armor.ToString() + " ";
		ResiValue.text = m_Player.MagicResistance.ToString() + " ";
		DexValue.text = m_Player.Dexterity.ToString() + " ";
		CritValue.text = m_Player.Crit.ToString() + " ";
		TempoValue.text = m_Player.Tempo.ToString() + " ";
		RegValue.text = m_Player.HealthRegenRate.ToString() + " ";
		LevelValue.text = m_Player.Level.ToString() + " ";
		PowerValue.text = m_Player.Power.ToString() + " ";
	}

	public void SetInteractText(string value, bool show = false) {
		InteractText.text = "[" + vp_Input.Instance.Buttons["Use"].ToString() + "] " + value;
		ToggleInteractText(show);
	}

	public void ToggleInteractText(bool show) {
		InteractText.enabled = show;
	}
}
