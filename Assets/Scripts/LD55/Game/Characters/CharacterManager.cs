using System.Collections.Generic;
using NiUtils.Extensions;
using UnityEngine;

namespace LD55.Game {
	public class CharacterManager : MonoBehaviour {
		[SerializeField] protected PlayerController player;
		[SerializeField] protected List<MonsterController> monsters = new List<MonsterController>();
		[SerializeField] protected List<SummoningController> summonings = new List<SummoningController>();

		private int NextMonsterToUpdate { get; set; }
		private int NextSummoningToUpdate { get; set; }

		public void Start() {
			player.Summoner.OnRecipeSummoned.AddListenerOnce(HandleRecipeSummoned);
		}

		public void Update() {
			UpdateNextMonster();
			UpdateNextSummoning();
		}

		private void UpdateNextSummoning() => NextSummoningToUpdate = UpdateAiCharacter(summonings, NextSummoningToUpdate, monsters, null);
		private void UpdateNextMonster() => NextMonsterToUpdate = UpdateAiCharacter(monsters, NextMonsterToUpdate, summonings, player);

		private static int UpdateAiCharacter(IReadOnlyList<IAiCharacter> characters, int currentCharacterIndex, IEnumerable<ICharacterBrain> validTargets, ICharacterBrain playerAsValidTarget) {
			if (characters.Count < 1) return 0;
			currentCharacterIndex %= characters.Count;

			var currentCharacter = characters[currentCharacterIndex];
			currentCharacter.Target = default;
			var bestTargetCost = float.MaxValue;

			if (playerAsValidTarget != null) {
				currentCharacter.Target = playerAsValidTarget.CharacterController;
				bestTargetCost = (playerAsValidTarget.CharacterController.Position - currentCharacter.CharacterController.Position).sqrMagnitude;
			}

			foreach (var target in validTargets) {
				var targetCost = (target.CharacterController.Position - currentCharacter.CharacterController.Position).sqrMagnitude;
				if (bestTargetCost > targetCost) {
					currentCharacter.Target = target.CharacterController;
					bestTargetCost = targetCost;
				}
			}

			return currentCharacterIndex + 1;
		}

		private void HandleRecipeSummoned(SummoningRecipe summonedRecipe, Vector2 position) {
			summonings.Add(Instantiate(summonedRecipe.SummoningPrefab, position, Quaternion.identity, transform));
		}
	}
}