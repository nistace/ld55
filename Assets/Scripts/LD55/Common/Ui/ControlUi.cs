using LD55.Inputs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlUi : MonoBehaviour {
	[SerializeField] protected Image control;
	[SerializeField] protected TMP_Text keyText;
	[SerializeField] protected Image missImage;

	public Image Image => control;
	public TMP_Text KeyText => keyText;

	public void SetControl(char control) {
		this.control.sprite = InputManager.ControllerSprites[control];
		keyText.enabled = InputManager.ControllerSprites.DisplayKey;
		if (keyText.enabled) keyText.text = InputManager.GetKeyText(control);
	}

	public void MarkAsMissed(Color controlColor) {
		control.color = controlColor;
		missImage.enabled = true;
	}

	public void MarkAsNotMissed(Color controlColor) {
		control.color = controlColor;
		missImage.enabled = false;
	}
}