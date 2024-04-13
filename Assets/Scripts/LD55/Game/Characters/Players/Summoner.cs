using System.Linq;
using LD55.Inputs;
using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LD55.Game {
	public class Summoner : MonoBehaviour {
		[SerializeField] protected SummoningRecipe[] recipes;
		[SerializeField] protected Transform summonLocation;
		[SerializeField] protected SpriteRenderer currentRecipeSummoningRenderer;

		public bool IsSummoning { get; private set; }
		public int Level { get; set; }
		private SummoningRecipe CurrentRecipe { get; set; }
		private string CurrentSummoningLine { get; set; }
		public UnityEvent<SummoningRecipe, Vector2> OnRecipeSummoned { get; } = new UnityEvent<SummoningRecipe, Vector2>();

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
			if (IsSummoning == summoning && !forceRefresh) return;
			IsSummoning = summoning;
			CurrentSummoningLine = string.Empty;
			summonLocation.gameObject.SetActive(IsSummoning);
			CurrentRecipe = default;
			currentRecipeSummoningRenderer.enabled = false;
			InputManager.Controls.Player.SummonA.SetPerformListenerOnce(HandleSummonAPressed, IsSummoning);
			InputManager.Controls.Player.SummonB.SetPerformListenerOnce(HandleSummonBPressed, IsSummoning);
			InputManager.Controls.Player.SummonC.SetPerformListenerOnce(HandleSummonCPressed, IsSummoning);
			InputManager.Controls.Player.SummonD.SetPerformListenerOnce(HandleSummonDPressed, IsSummoning);
			InputManager.Controls.Player.SummonE.SetPerformListenerOnce(HandleSummonEPressed, IsSummoning);
			InputManager.Controls.Player.SummonF.SetPerformListenerOnce(HandleSummonFPressed, IsSummoning);
			InputManager.Controls.Player.SummonG.SetPerformListenerOnce(HandleSummonGPressed, IsSummoning);
			InputManager.Controls.Player.SummonH.SetPerformListenerOnce(HandleSummonHPressed, IsSummoning);
		}

		private void RefreshBestMatchingRecipe() {
			var bestMatchingRecipeScore = 0;
			foreach (var recipe in recipes) {
				var recipeScore = GetRecipeScore(recipe.SummoningLine);
				if (recipeScore > bestMatchingRecipeScore) {
					CurrentRecipe = recipe;
					bestMatchingRecipeScore = recipeScore;
				}
			}
		}

		private int GetRecipeScore(string recipeLine) => CurrentSummoningLine.Where((t, i) => t == recipeLine[i]).Count();

		private void AppendSummoningCharacter(char additionalCharacter) {
			if (!IsSummoning) return;
			CurrentSummoningLine += additionalCharacter;
			if (CurrentSummoningLine.Length < 6) {
				RefreshBestMatchingRecipe();
			}
			else if (!CurrentRecipe) {
				CurrentSummoningLine = string.Empty;
			}

			currentRecipeSummoningRenderer.enabled = CurrentRecipe;
			if (CurrentRecipe) {
				currentRecipeSummoningRenderer.sprite = CurrentRecipe.SummoningSprite;
				if (CurrentRecipe.SummoningLine.Length == CurrentSummoningLine.Length) {
					OnRecipeSummoned.Invoke(CurrentRecipe, summonLocation.position);
					CurrentSummoningLine = string.Empty;
					currentRecipeSummoningRenderer.enabled = false;
					CurrentRecipe = null;
				}
			}
		}

		private void HandleSummonAPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('A');
		private void HandleSummonBPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('B');
		private void HandleSummonCPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('C');
		private void HandleSummonDPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('D');
		private void HandleSummonEPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('E');
		private void HandleSummonFPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('F');
		private void HandleSummonGPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('G');
		private void HandleSummonHPressed(InputAction.CallbackContext obj) => AppendSummoningCharacter('H');
	}
}