using System.Collections.Generic;
using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game {
	public class CharacterManager : MonoBehaviour {
		
		private static CharacterManager CachedInstance { get; set; }
		public static CharacterManager Instance => CachedInstance ? CachedInstance : CachedInstance = FindObjectOfType<CharacterManager>(true);
		
		
		[SerializeField] protected EnemyWaveDescriptor enemyWaveDescriptor;
		[SerializeField] protected PlayerController player;

		public PlayerController Player => player;
		private int NextAiToRefresh { get; set; }
		private int CurrentWaveIndex { get; set; }
		private float CurrentWaveStartTime { get; set; }
		private float CurrentWaveTime => Time.time - CurrentWaveStartTime;
		private EnemyWaveDescriptor.Wave CurrentWave => enemyWaveDescriptor.GetWave(CurrentWaveIndex);
		private int CurrentWaveCoefficient => enemyWaveDescriptor.GetWaveCoefficient(CurrentWaveIndex);
		private List<AiCharacter> AllCharacters { get; } = new List<AiCharacter>();
		private Dictionary<AiCharacter, float> EnemySpawnProgress { get; } = new Dictionary<AiCharacter, float>();

		public UnityEvent OnPlayerDied => player.CharacterController.OnDied;

		public void Start() {
			CombatGlobalParameters.Clear();
			CombatGlobalParameters.SubscribeTarget(player.CharacterController);
			enemyWaveDescriptor.AllDistinctSpawnableEnemies().ForEach(t => EnemySpawnProgress.Add(t, 0));

			player.Summoner.OnRecipeSummoned.AddListenerOnce(HandleRecipeSummoned);

			CurrentWaveIndex = 0;
			CurrentWaveStartTime = Time.time;
		}

		private void OnEnable() {
			CharacterController.OnAnyCharacterDied.AddListenerOnce(HandleAnyCharacterDied);
		}

		private void OnDisable() {
			CharacterController.OnAnyCharacterDied.RemoveListener(HandleAnyCharacterDied);
		}

		private void HandleAnyCharacterDied(CharacterController deadCharacter) {
			CombatGlobalParameters.UnsubscribeTarget(deadCharacter);
			AllCharacters.RemoveWhere(t => t.CharacterController == deadCharacter);
			// TODO Handle dying in a proper way
		}

		public void Update() {
			UpdateEnemyWave();
			UpdateNextAiToRefresh();
		}

		private void UpdateNextAiToRefresh() {
			if (AllCharacters.Count == 0) return;
			NextAiToRefresh %= AllCharacters.Count;

			AllCharacters[NextAiToRefresh].IsAllowedToChangeTarget = true;

			NextAiToRefresh++;
		}

		private void UpdateEnemyWave() {
			if (player.CharacterController.IsDead) return;

			if (CurrentWaveTime > CurrentWave.Duration) {
				CurrentWaveIndex++;
				CurrentWaveStartTime = Time.time;
			}

			if (CurrentWave.Duration > 0) {
				foreach (var waveEnemy in CurrentWave.WaveEnemies) {
					var enemyPrefab = waveEnemy.CharacterPrefab;
					EnemySpawnProgress[enemyPrefab] += CurrentWaveCoefficient * waveEnemy.SpawnRateCurve.Evaluate(CurrentWaveTime / CurrentWave.Duration) * Time.deltaTime;
					while (EnemySpawnProgress[enemyPrefab] > 1) {
						Spawn(enemyPrefab, player.Position + Vector2.right.Rotate(Random.Range(0, 360)) * enemyWaveDescriptor.SpawnDistance);
						EnemySpawnProgress[enemyPrefab]--;
					}
				}
			}
		}

		private void HandleRecipeSummoned(SummoningRecipe summonedRecipe, Vector2 position) => Spawn(summonedRecipe.SummoningsPrefabs, position);

		private void Spawn(IReadOnlyList<AiCharacter> prefabs, Vector2 position) {
			for (var index = 0; index < prefabs.Count; index++) {
				var prefab = prefabs[index];
				var offset = index switch {
					0 => Vector2.zero,
					< 7 => (Vector2)(Quaternion.Euler(0, 0, index * 60) * new Vector2(0, .2f)),
					_ => Vector2.zero
				};
				Spawn(prefab, position + offset);
			}
		}

		private void Spawn(AiCharacter prefab, Vector2 position) {
			var character = Instantiate(prefab, position, Quaternion.identity, transform);
			AllCharacters.Add(character);
			CombatGlobalParameters.SubscribeTarget(character.CharacterController);
		}
	}
}