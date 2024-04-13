using System.Collections.Generic;
using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game {
	public class CharacterManager : MonoBehaviour {
		[SerializeField] protected EnemyWaveDescriptor enemyWaveDescriptor;
		[SerializeField] protected PlayerController player;

		public PlayerController Player => player;
		private int NextMonsterToUpdate { get; set; }
		private int NextSummoningToUpdate { get; set; }
		private float StartTime { get; set; }
		private float TimeSinceStart => Time.time - StartTime;
		private int CurrentWaveIndex { get; set; }
		private float CurrentWaveStartTime { get; set; }
		private float CurrentWaveTime => Time.time - CurrentWaveStartTime;
		private EnemyWaveDescriptor.Wave CurrentWave => enemyWaveDescriptor.GetWave(CurrentWaveIndex);
		private int CurrentWaveCoefficient => enemyWaveDescriptor.GetWaveCoefficient(CurrentWaveIndex);
		private Dictionary<Team, List<AiCharacter>> CharactersPerTeam { get; } = new Dictionary<Team, List<AiCharacter>>();
		private Dictionary<AiCharacter, float> EnemySpawnProgress { get; } = new Dictionary<AiCharacter, float>();

		public UnityEvent OnPlayerDied => player.CharacterController.OnDied;

		public void Start() {
			CharactersPerTeam.Add(Team.Player, new List<AiCharacter>());
			CharactersPerTeam.Add(Team.Enemy, new List<AiCharacter>());
			CombatGlobalParameters.Clear();
			CombatGlobalParameters.SubscribeTargetOfTeam(Team.Player, player.CharacterController);
			enemyWaveDescriptor.AllDistinctSpawnableEnemies().ForEach(t => EnemySpawnProgress.Add(t, 0));

			player.Summoner.OnRecipeSummoned.AddListenerOnce(HandleRecipeSummoned);

			CurrentWaveIndex = 0;
			CurrentWaveStartTime = Time.time;
			StartTime = Time.time;
		}

		private void OnEnable() {
			CharacterController.OnAnyCharacterDied.AddListenerOnce(HandleAnyCharacterDied);
		}

		private void OnDisable() {
			CharacterController.OnAnyCharacterDied.RemoveListener(HandleAnyCharacterDied);
		}

		private void HandleAnyCharacterDied(CharacterController deadCharacter) {
			CombatGlobalParameters.UnsubscribeTarget(deadCharacter);
			foreach (var characterSet in CharactersPerTeam.Values) {
				characterSet.RemoveWhere(t => t.CharacterController == deadCharacter);
			}
			// TODO Handle dying in a proper way
		}

		public void Update() {
			UpdateEnemyWave();
			UpdateNextEnemy();
			UpdateNextSummoning();
		}

		private void UpdateEnemyWave() {
			if (player.CharacterController.IsDead) return;

			if (CurrentWaveTime > CurrentWave.Duration) {
				CurrentWaveIndex++;
				CurrentWaveStartTime = Time.time;
			}

			if (CurrentWaveTime > 0) {
				foreach (var waveEnemy in CurrentWave.WaveEnemies) {
					var enemyPrefab = waveEnemy.CharacterPrefab;
					EnemySpawnProgress[enemyPrefab] += CurrentWaveCoefficient * waveEnemy.SpawnRateCurve.Evaluate(TimeSinceStart / CurrentWaveTime) * Time.deltaTime;
					while (EnemySpawnProgress[enemyPrefab] > 1) {
						SpawnEnemy(enemyPrefab);
						EnemySpawnProgress[enemyPrefab]--;
					}
				}
			}
		}

		private void UpdateNextSummoning() => NextSummoningToUpdate = UpdateAiCharacter(CharactersPerTeam[Team.Player], NextSummoningToUpdate, CharactersPerTeam[Team.Enemy], null);
		private void UpdateNextEnemy() => NextMonsterToUpdate = UpdateAiCharacter(CharactersPerTeam[Team.Enemy], NextMonsterToUpdate, CharactersPerTeam[Team.Player], player);

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

		private void HandleRecipeSummoned(SummoningRecipe summonedRecipe, Vector2 position) => Spawn(summonedRecipe.SummoningPrefab, position, Team.Player);
		private void SpawnEnemy(AiCharacter enemyPrefab) => Spawn(enemyPrefab, player.Position + Vector2.right.Rotate(Random.Range(0, 360)) * enemyWaveDescriptor.SpawnDistance, Team.Enemy);

		private void Spawn(AiCharacter prefab, Vector2 position, Team team) {
			var character = Instantiate(prefab, position, Quaternion.identity, transform);
			CharactersPerTeam[team].Add(character);
			CombatGlobalParameters.SubscribeTargetOfTeam(team, character);
		}
	}
}