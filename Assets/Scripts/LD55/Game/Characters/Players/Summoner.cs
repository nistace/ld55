﻿using System.Linq;
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
		[SerializeField] protected AudioClip[] clips;

		public bool IsSummoning { get; private set; }
		public int Level { get; private set; }
		public bool IsMaxLevel => Level >= recipes.Length;
		public int CurrentRecipeIndex { get; private set; }
		public SummoningRecipe CurrentRecipe => recipes[CurrentRecipeIndex];
		public string CurrentSummoningLine { get; private set; }
		public int UnlockedRecipesCount => Mathf.Min(Level, recipes.Length);
		public bool IsCurrentRecipeLocked => CurrentSummoningLine.Length >= LineLengthToLockRecipe;

		public UnityEvent<SummoningRecipe, Vector2, float> OnRecipeSummoned { get; } = new UnityEvent<SummoningRecipe, Vector2, float>();
		public UnityEvent<float> OnCommandFailedWithAccuracy { get; } = new UnityEvent<float>();
		public UnityEvent OnSummoningStateChanged { get; } = new UnityEvent();
		public UnityEvent OnSummoningCommandLineChanged { get; } = new UnityEvent();

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
			if (GameEnvironmentManager.Instance) GameEnvironmentManager.Instance.OnChangingEnvironment.SetListenerActive(HandleGameEnvironmentChanged, enabled);
			InputManager.Controls.Player.Summon.SetAnyListenerOnce(HandleSummonInputChanged, enabled);
		}

		private void HandleGameEnvironmentChanged() {
			RefreshSummoning(IsSummoning, false);
		}

		private void HandleSummonInputChanged(InputAction.CallbackContext obj) {
			RefreshSummoning(obj.performed, false);
		}

		private void RefreshSummoning(bool summoning, bool forceRefresh) {
			summoning &= UnlockedRecipesCount > 0;
			summoning &= GameEnvironmentManager.Instance.SpawningAllowed;
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
			for (var recipeIndex = 0; recipeIndex < Level; recipeIndex++) {
				if (recipeIndex == CurrentRecipeIndex) continue;
				var recipe = recipes[recipeIndex];
				var recipeScore = GetRecipeScore(recipe.SummoningLine);
				if (recipeScore > bestMatchingRecipeScore) {
					CurrentRecipeIndex = recipeIndex;
					bestMatchingRecipeScore = recipeScore;
				}
			}
		}

		private int GetRecipeScore(string recipeLine) {
			return CurrentSummoningLine.Where((t, i) => t == recipeLine[i]).Count();
		}

		private void AppendSummoningCharacter(char additionalCharacter) {
			if (!IsSummoning) return;
			CurrentSummoningLine += additionalCharacter;
			if (CurrentSummoningLine.Length <= LineLengthToLockRecipe) {
				RefreshBestMatchingRecipe();
			}
			else if ((float)(CurrentSummoningLine.Length - GetRecipeScore(CurrentRecipe.SummoningLine)) / CurrentSummoningLine.Length >= MissesRatioToCancelSummoning) {
				OnCommandFailedWithAccuracy.Invoke(1 - (float)(CurrentSummoningLine.Length - GetRecipeScore(CurrentRecipe.SummoningLine)) / CurrentSummoningLine.Length);
				CurrentSummoningLine = string.Empty;
			}
			else if (CurrentRecipe.SummoningLine.Length == CurrentSummoningLine.Length) {
				var recipe = CurrentRecipe;
				var accuracy = (float)GetRecipeScore(CurrentRecipe.SummoningLine) / recipe.SummoningLine.Length;
				CurrentSummoningLine = string.Empty;
				OnRecipeSummoned.Invoke(recipe, summonLocation.position, accuracy);
			}

			currentRecipeSummoningRenderer.sprite = CurrentRecipe.SummoningSprite;
			OnSummoningCommandLineChanged.Invoke();
		}

		private void HandleSummonAPressed(InputAction.CallbackContext obj) {
			GameAudio.PlaySfx(clips[0], transform.position);
			AppendSummoningCharacter('A');
		}

		private void HandleSummonBPressed(InputAction.CallbackContext obj) {
			GameAudio.PlaySfx(clips[1], transform.position);
			AppendSummoningCharacter('B');
		}

		private void HandleSummonCPressed(InputAction.CallbackContext obj) {
			GameAudio.PlaySfx(clips[2], transform.position);
			AppendSummoningCharacter('C');
		}

		private void HandleSummonDPressed(InputAction.CallbackContext obj) {
			GameAudio.PlaySfx(clips[3], transform.position);
			AppendSummoningCharacter('D');
		}

		private void HandleSummonEPressed(InputAction.CallbackContext obj) {
			GameAudio.PlaySfx(clips[4], transform.position);
			AppendSummoningCharacter('E');
		}

		private void HandleSummonFPressed(InputAction.CallbackContext obj) {
			GameAudio.PlaySfx(clips[5], transform.position);
			AppendSummoningCharacter('F');
		}

		private void HandleSummonGPressed(InputAction.CallbackContext obj) {
			GameAudio.PlaySfx(clips[6], transform.position);
			AppendSummoningCharacter('G');
		}

		private void HandleSummonHPressed(InputAction.CallbackContext obj) {
			GameAudio.PlaySfx(clips[7], transform.position);
			AppendSummoningCharacter('H');
		}

		public int ClampRecipeIndex(int index) {
			return Mathf.Clamp(index, 0, recipes.Length);
		}

		public SummoningRecipe GetRecipe(int recipeIndex) {
			return recipes[recipeIndex];
		}

		public void ChangeCurrentRecipeIndex(int index) {
			if (IsCurrentRecipeLocked) return;
			CurrentRecipeIndex = index.PosMod(UnlockedRecipesCount);
			currentRecipeSummoningRenderer.sprite = CurrentRecipe.SummoningSprite;
		}

		public void LevelUp() {
			Level++;
			CurrentRecipeIndex = Level.Clamp(0, UnlockedRecipesCount - 1);
		}
	}
}