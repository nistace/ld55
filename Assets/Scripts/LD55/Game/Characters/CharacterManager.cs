using System.Collections.Generic;
using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game {
	public class CharacterManager : MonoBehaviour {
		[SerializeField] protected PlayerController player;
		[SerializeField] protected List<AiCharacter> enemies = new List<AiCharacter>();
		[SerializeField] protected List<AiCharacter> summonings = new List<AiCharacter>();

		private int NextMonsterToUpdate { get; set; }
		private int NextSummoningToUpdate { get; set; }

		public UnityEvent OnPlayerDied => player.CharacterController.OnDied;

		public void Start() {
			player.Summoner.OnRecipeSummoned.AddListenerOnce(HandleRecipeSummoned);
			CombatGlobalParameters.Clear();
			CombatGlobalParameters.SubscribeTargetOfTeam(CombatGlobalParameters.Team.Player, player.CharacterController);
			enemies.ForEach(t => CombatGlobalParameters.SubscribeTargetOfTeam(CombatGlobalParameters.Team.Enemy, t));
			summonings.ForEach(t => CombatGlobalParameters.SubscribeTargetOfTeam(CombatGlobalParameters.Team.Player, t));
		}

		private void OnEnable() {
			CharacterController.OnAnyCharacterDied.AddListenerOnce(HandleAnyCharacterDied);
		}

		private void OnDisable() {
			CharacterController.OnAnyCharacterDied.RemoveListener(HandleAnyCharacterDied);
		}

		private void HandleAnyCharacterDied(CharacterController deadCharacter) {
			CombatGlobalParameters.UnsubscribeTarget(deadCharacter);
			enemies.RemoveWhere(t => t.CharacterController == deadCharacter);
			summonings.RemoveWhere(t => t.CharacterController == deadCharacter);
			// TODO Handle dying in a proper way
		}

		public void Update() {
			UpdateNextEnemy();
			UpdateNextSummoning();
		}

		private void UpdateNextSummoning() => NextSummoningToUpdate = UpdateAiCharacter(summonings, NextSummoningToUpdate, enemies, null);
		private void UpdateNextEnemy() => NextMonsterToUpdate = UpdateAiCharacter(enemies, NextMonsterToUpdate, summonings, player);

		private static int UpdateAiCharacter(IReadOnlyList<AiCharacter> characters, int currentCharacterIndex, IEnumerable<ICombatTarget> validTargets, ICharacterBrain playerAsValidTarget) {
			if (characters.Count < 1) return 0;
			currentCharacterIndex %= characters.Count;

			var currentCharacter = characters[currentCharacterIndex];
			currentCharacter.ChangeTarget(default);
			var bestTargetCost = float.MaxValue;

			if (playerAsValidTarget != null) {
				currentCharacter.ChangeTarget(playerAsValidTarget.CharacterController);
				bestTargetCost = (playerAsValidTarget.CharacterController.Position - currentCharacter.CharacterController.Position).sqrMagnitude;
			}

			foreach (var target in validTargets) {
				var targetCost = (target.Position - currentCharacter.CharacterController.Position).sqrMagnitude;
				if (bestTargetCost > targetCost) {
					currentCharacter.ChangeTarget(target);
					bestTargetCost = targetCost;
				}
			}

			return currentCharacterIndex + 1;
		}

		private void HandleRecipeSummoned(SummoningRecipe summonedRecipe, Vector2 position) {
			var summoning = Instantiate(summonedRecipe.SummoningPrefab, position, Quaternion.identity, transform);
			summonings.Add(summoning);
			CombatGlobalParameters.SubscribeTargetOfTeam(CombatGlobalParameters.Team.Player, summoning);
		}
	}
}