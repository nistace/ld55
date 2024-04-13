using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu]
	public class EnemyWaveDescriptor : ScriptableObject {
		[Serializable] public class Wave {
			[SerializeField] protected float duration = 90;
			[SerializeField] protected WaveEnemy[] waveEnemies = { };

			public float Duration => duration;
			public IReadOnlyList<WaveEnemy> WaveEnemies => waveEnemies;
		}

		[Serializable]
		public class WaveEnemy {
			[SerializeField] protected AiCharacter characterPrefab;
			[SerializeField] protected AnimationCurve spawnRateCurve;

			public AiCharacter CharacterPrefab => characterPrefab;
			public AnimationCurve SpawnRateCurve => spawnRateCurve;
		}

		[SerializeField] protected Wave beforeFirstWave;
		[SerializeField] protected Wave[] waves;
		[SerializeField] protected Wave finalInfiniteWave;
		[SerializeField] protected float spawnDistance = 10;

		public float SpawnDistance => spawnDistance;
		public IEnumerable<Wave> Waves => waves;

		public Wave GetWave(int waveIndex) {
			if (waveIndex < 0) return beforeFirstWave;
			if (waveIndex >= waves.Length) return finalInfiniteWave;
			return waves[waveIndex];
		}

		public int GetWaveCoefficient(int waveIndex) => waveIndex < waves.Length ? 1 : waveIndex + 2 - waves.Length;

		public IEnumerable<AiCharacter> AllDistinctSpawnableEnemies() =>
			waves.Union(new[] { beforeFirstWave, finalInfiniteWave }).SelectMany(wave => wave.WaveEnemies.Select(enemyInWave => enemyInWave.CharacterPrefab)).Distinct();
	}
}