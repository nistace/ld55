using System.Collections.Generic;
using System.Linq;
using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game {
	public class CharacterManager : MonoBehaviour {
		private static CharacterManager CachedInstance { get; set; }
		public static CharacterManager Instance => CachedInstance ? CachedInstance : CachedInstance = FindObjectOfType<CharacterManager>(true);
		public static UnityEvent OnPlayerDied => Instance.Player.CharacterController.OnDied;
		public static UnityEvent<SummoningRecipe, float> OnSummoningCompletedWithAccuracy { get; } = new UnityEvent<SummoningRecipe, float>();
		public static UnityEvent OnEnemyKilled { get; } = new UnityEvent();

		[SerializeField] protected EnemyWaveDescriptor enemyWaveDescriptor;
		[SerializeField] protected PlayerController player;

		public PlayerController Player => player;
		private int NextAiToRefresh { get; set; }
		private int CurrentWaveIndex { get; set; } = -1;
		public float PlayingTime { get; private set; }
		private float CurrentWaveStartTime { get; set; }
		private float CurrentWaveTime => Time.time - CurrentWaveStartTime;
		private EnemyWaveDescriptor.Wave CurrentWave => enemyWaveDescriptor.GetWave(CurrentWaveIndex);
		private int CurrentWaveCoefficient => enemyWaveDescriptor.GetWaveCoefficient(CurrentWaveIndex);
		private List<AiCharacter> AllCharacters { get; } = new List<AiCharacter>();
		private Dictionary<AiCharacter, float> EnemySpawnProgress { get; } = new Dictionary<AiCharacter, float>();

		public void Start() {
			CombatGlobalParameters.Clear();
			CombatGlobalParameters.SubscribeTarget(player.CharacterController);
			enemyWaveDescriptor.AllDistinctSpawnableEnemies().ForEach(t => EnemySpawnProgress.Add(t, 0));

			player.Summoner.OnRecipeSummoned.AddListenerOnce(HandleRecipeSummoned);
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
			if (deadCharacter.Team == Team.Enemy) OnEnemyKilled.Invoke();
		}

		public void StartFirstWave() {
			CurrentWaveStartTime = Time.time;
			CurrentWaveIndex = 0;
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
			if (CurrentWaveIndex == -1) return;
			if (player.CharacterController.IsDead) return;
			if (!GameEnvironmentManager.Instance.SpawningAllowed) return;

			PlayingTime += Time.deltaTime;

			if (CurrentWaveTime > CurrentWave.Duration) {
				CurrentWaveIndex++;
				CurrentWaveStartTime = Time.time;
			}

			if (CurrentWave.Duration > 0) {
				foreach (var waveEnemy in CurrentWave.WaveEnemies) {
					var enemyPrefab = waveEnemy.CharacterPrefab;
					EnemySpawnProgress[enemyPrefab] += CurrentWaveCoefficient * waveEnemy.SpawnRateCurve.Evaluate(CurrentWaveTime / CurrentWave.Duration) * Time.deltaTime;
					while (EnemySpawnProgress[enemyPrefab] > 1) {
						Spawn(enemyPrefab, player.Position + Vector2.right.Rotate(Random.Range(0, 360)) * enemyWaveDescriptor.SpawnDistance, 1);
						EnemySpawnProgress[enemyPrefab]--;
					}
				}
			}
		}

		private void HandleRecipeSummoned(SummoningRecipe summonedRecipe, Vector2 position, float accuracy) {
			Spawn(summonedRecipe.SummoningsPrefabs, position, accuracy);
			OnSummoningCompletedWithAccuracy.Invoke(summonedRecipe, accuracy);
		}

		private void Spawn(IReadOnlyList<AiCharacter> prefabs, Vector2 position, float initialHealthRatio) {
			for (var index = 0; index < prefabs.Count; index++) {
				var prefab = prefabs[index];
				var offset = index switch {
					0 => Vector2.zero,
					< 7 => (Vector2)(Quaternion.Euler(0, 0, index * 60) * new Vector2(0, .2f)),
					_ => Vector2.zero
				};
				Spawn(prefab, position + offset, initialHealthRatio);
			}
		}

		private void Spawn(AiCharacter prefab, Vector2 position, float initialHealthRatio) {
			var character = Instantiate(prefab, position, Quaternion.identity, transform);
			character.TakeDamage(Mathf.FloorToInt((1 - initialHealthRatio) * character.CharacterController.Type.MaxHealth));
			AllCharacters.Add(character);
			CombatGlobalParameters.SubscribeTarget(character.CharacterController);
		}

		public void KillAllEnemies() {
			var allEnemies = AllCharacters.Where(t => t.CharacterController.Team == Team.Enemy).ToArray();
			allEnemies.ForEach(t => t.TakeDamage(90000));
		}
	}
}