using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD55.Game.Enemies {
	[CreateAssetMenu]
	public class EnemyWaveDescriptor : ScriptableObject {
		[Serializable]
		public class EnemyWaveItem {
			[SerializeField] protected AiCharacter characterPrefab;
			[SerializeField] protected AnimationCurve spawnRateCurve;

			public AiCharacter CharacterPrefab => characterPrefab;
			public AnimationCurve SpawnRateCurve => spawnRateCurve;
		}

		[SerializeField] protected EnemyWaveItem[] enemyWaveItems;
		[SerializeField] protected float spawnDistance = 10;

		public IEnumerable<EnemyWaveItem> EnemyWaveItems => enemyWaveItems;
		public float SpawnDistance => spawnDistance;
	}
}