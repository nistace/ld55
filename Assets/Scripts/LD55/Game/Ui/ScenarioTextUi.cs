using TMPro;
using UnityEngine;

public class ScenarioTextUi : MonoBehaviour {
	[SerializeField] protected CanvasGroup canvasGroup;
	[SerializeField] protected TMP_Text text;

	private float TargetAlpha { get; set; }

	private void Start() {
		canvasGroup.alpha = 0;
	}

	public void Show(string text) {
		TargetAlpha = 1;
		this.text.text = text;
	}

	public void Hide() {
		TargetAlpha = 0;
	}

	private void Update() {
		canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, TargetAlpha, Time.deltaTime * 2);
	}
}