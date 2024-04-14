using System.Linq;
using LD55.Inputs;
using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LD55.Game {
	public class Summoner : MonoBehaviour {
		private const int LineLengthToLockRecipe = 3;
		private const float MissesRatioToCancelSummoning = .5f;

		[SerializeField] protected SummoningRecipe[] recipes;
		[SerializeField] protected Transform summonLocation;
		[SerializeField] protected SpriteRenderer currentRecipeSummoningRenderer;

		public bool IsSummoning { get; private set; }
		public int Level { get; set; }
		public int CurrentRecipeIndex { get; private set; }
		public SummoningRecipe CurrentRecipe => recipes[CurrentRecipeIndex];
		public string CurrentSummoningLine { get; private set; }
		public UnityEvent<SummoningRecipe, Vector2> OnRecipeSummoned { get; } = new UnityEvent<SummoningRecipe, Vector2>();
		public UnityEvent OnSummoningStateChanged { get; } = new UnityEvent();
		public UnityEvent OnSummoningCommandLineChanged { get; } = new UnityEvent();
		public int UnlockedRecipesCount => Mathf.Min(Level, recipes.Length);
		public bool IsCurrentRecipeLocked => CurrentSummoningLine.Length >= LineLengthToLockRecipe;

		private void Start() {
			RefreshSummoning(false, true);
		}

		private void OnEnable() {
			SetListenersEnabled(true);
		}

		private void OnDisable() {
			SetListenersEnabled(false);
		}

		private void SetListenersEnabled(bool enabled) {
			InputManager.Controls.Player.Summon.SetAnyListenerOnce(HandleSummonInputChanged, enabled);
		}

		private void HandleSummonInputChanged(InputAction.CallbackContext obj) => RefreshSummoning(obj.performed, false);

		private void RefreshSummoning(bool summoning, bool forceRefresh) {
			summoning &= UnlockedRecipesCount > 0;
			if (IsSummoning == summoning && !forceRefresh) return;
			IsSummoning = summoning;
			CurrentSummoningLine = string.Empty;
			summonLocation.gameObject.SetActive(IsSummoning);
			InputManager.Controls.Player.SummonA.SetPerformListenerOnce(HandleSummonAPressed, IsSummoning);
			InputManager.Controls.Player.SummonB.SetPerformListenerOnce(HandleSummonBPressed, IsSummoning);
			InputManager.Controls.Player.SummonC.SetPerformListenerOnce(HandleSummonCPressed, IsSummoning);
			InputManager.Controls.Player.SummonD.SetPerformListenerOnce(HandleSummonDPressed, IsSummoning);
			InputManager.Controls.Player.SummonE.SetPerformListenerOnce(HandleSummonEPressed, IsSummoning);
			InputManager.Controls.Player.SummonF.SetPerformListenerOnce(HandleSummonFPressed, IsSummoning);
			InputManager.Controls.Player.SummonG.SetPerformListenerOnce(HandleSummonGPressed, IsSummoning);
			InputManager.Controls.Player.SummonH.SetPerformListenerOnce(HandleSummonHPressed, IsSummoning);
			OnSummoningStateChanged.Invoke();
		}

		private void RefreshBestMatchingRecipe() {
			var bestMatchingRecipeScore = GetRecipeScore(CurrentRecipe.SummoningLine);
			for (var recipeIndex = 0; recipeIndex < recipes.Length; recipeIndex++) {
				if (recipeIndex == CurrentRecipeIndex) continue;
				var recipe = recipes[recipeIndex];
				var recipeScore = GetRecipeScore(recipe.SummoningLine);
				if (recipeScore > bestMatchingRecipeScore) {
					CurrentRecipeIndex = recipeIndex;
					bestMatchingRecipeScore = recipeScore;
				}
			}
		}

		private int GetRecipeScore(string recipeLine) => CurrentSummoningLine.Where((t, i) => t == recipeLine[i]).Count();

		private void AppendSummoningCharacter(char additionalCharacter) {
			if (!IsSummoning) return;
			CurrentSummoningLine += additionalCharacter;
			if (CurrentSummoningLine.Length <= LineLengthToLockRecipe) {
				RefreshBestMatchingRecipe();
			}
			else if ((float)(CurrentSummoningLine.Length - GetRecipeScore(CurrentRecipe.SummoningLine)) / CurrentSummoningLine.Length >= MissesRatioToCancelSummoning) {
				CurrentSummoningLine = string.Empty;
			}
			else if (CurrentRecipe.SummoningLine.Length == CurrentSummoningLine.Length) {
				var recipe = CurrentRecipe;
				CurrentSummoningLine = string.Empty;
				OnRecipeSummoned.Invoke(recipe, summonLocation.position);
			}

			currentRecipeSummoningRenderer.sprite = CurrentRecipe.SummoningSprite;
			OnSummoningCommandLineChanged.Invoke();
		}

		private void HandleSummonAPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('A');
		private void HandleSummonBPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('B');
		private void HandleSummonCPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('C');
		private void HandleSummonDPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('D');
		private void HandleSummonEPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('E');
		private void HandleSummonFPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('F');
		private void HandleSummonGPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('G');
		private void HandleSummonHPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('H');

		public int ClampRecipeIndex(int index) => Mathf.Clamp(index, 0, recipes.Length);

		public SummoningRecipe GetRecipe(int recipeIndex) => recipes[recipeIndex];

		public void ChangeCurrentRecipeIndex(int index) {
			if (IsCurrentRecipeLocked) return;
			CurrentRecipeIndex = index.PosMod(UnlockedRecipesCount);
			currentRecipeSummoningRenderer.sprite = CurrentRecipe.SummoningSprite;
		}
	}
}