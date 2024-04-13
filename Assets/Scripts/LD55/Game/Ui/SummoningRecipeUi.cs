using LD55.Game;
using LD55.Inputs;
using NiUtils.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SummoningRecipeUi : MonoBehaviour {
	[SerializeField] protected CanvasGroup canvasGroup;
	[SerializeField] protected Image icon;
	[SerializeField] protected TMP_Text summoningName;
	[SerializeField] protected TMP_Text summoningRole;
	[SerializeField] protected SummoningLineControlUi[] controls;
	[SerializeField] protected Color summoningControlActiveColor = Color.white;
	[SerializeField] protected Color summoningControlInactiveColor = Color.white.With(a: .5f);
	[SerializeField] protected TMP_Text pageText;

	private Summoner Summoner { get; set; }
	private float TargetAlpha { get; set; }

	private void Reset() {
		controls = GetComponentsInChildren<SummoningLineControlUi>();
	}

	public void Init(Summoner summoner) {
		Summoner = summoner;
		canvasGroup.alpha = 0;
		Summoner.OnSummoningStateChanged.AddListenerOnce(Refresh);
		Refresh();
	}

	private void HandleRecipeSummoned(SummoningRecipe recipe, Vector2 position) => RefreshCurrentRecipeControls();

	private void HandleSummoningCommandLineChanged() {
		SetPage(Summoner.CurrentRecipeIndex);
		pageText.gameObject.SetActive(!Summoner.IsCurrentRecipeLocked);
		RefreshCurrentRecipeControls();
	}

	private void Refresh() {
		if (Summoner.IsSummoning) {
			gameObject.SetActive(Summoner.IsSummoning);
			SetPage(Summoner.CurrentRecipeIndex);
		}
		TargetAlpha = Summoner.IsSummoning ? 1 : 0;
		Summoner.OnSummoningCommandLineChanged.SetListenerActive(HandleSummoningCommandLineChanged, Summoner.IsSummoning);
		Summoner.OnRecipeSummoned.SetListenerActive(HandleRecipeSummoned, Summoner.IsSummoning);
		InputManager.Controls.Player.SummonPageNext.SetPerformListenerOnce(HandlePageNext, Summoner.IsSummoning);
		InputManager.Controls.Player.SummonPagePrevious.SetPerformListenerOnce(HandlePagePrevious, Summoner.IsSummoning);
	}

	private void HandlePageNext(InputAction.CallbackContext obj) => ChangePage(1);

	private void HandlePagePrevious(InputAction.CallbackContext obj) => ChangePage(-1);

	private void ChangePage(int delta) {
		if (Summoner.IsCurrentRecipeLocked) return;
		SetPage(Summoner.CurrentRecipeIndex + delta);
	}

	private void SetPage(int index) {
		if (!Summoner.IsCurrentRecipeLocked) {
			Summoner.ChangeCurrentRecipeIndex(index);
		}
		icon.sprite = Summoner.CurrentRecipe.SummoningSprite;
		summoningName.text = Summoner.CurrentRecipe.SummoningDisplayName;
		summoningRole.text = Summoner.CurrentRecipe.SummoningRole;
		pageText.text = $"Page {Summoner.CurrentRecipeIndex + 1} / {Summoner.UnlockedRecipesCount}";
		RefreshCurrentRecipeControls();
	}

	private void RefreshCurrentRecipeControls() {
		for (var i = 0; i < Summoner.CurrentSummoningLine.Length; ++i) {
			controls[i].gameObject.SetActive(true);
			controls[i].SetControl(Summoner.CurrentRecipe.SummoningLine[i]);
			if (Summoner.CurrentSummoningLine[i] == Summoner.CurrentRecipe.SummoningLine[i]) {
				controls[i].MarkAsNotMissed(summoningControlInactiveColor);
			}
			else {
				controls[i].MarkAsMissed(summoningControlInactiveColor);
			}
		}

		for (var i = Summoner.CurrentSummoningLine.Length; i < Summoner.CurrentRecipe.SummoningLine.Length; ++i) {
			controls[i].gameObject.SetActive(true);
			controls[i].SetControl(Summoner.CurrentRecipe.SummoningLine[i]);
			controls[i].MarkAsNotMissed(summoningControlActiveColor);
		}

		for (var i = Summoner.CurrentRecipe.SummoningLine.Length; i < controls.Length; ++i) {
			controls[i].gameObject.SetActive(false);
		}
	}

	private void Update() {
		canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, TargetAlpha, Time.deltaTime * 2);
		if (Mathf.Approximately(canvasGroup.alpha, 0) && Mathf.Approximately(TargetAlpha, 0)) gameObject.SetActive(false);
	}
}