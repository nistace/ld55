using System.Collections.Generic;
using NiUtils.Extensions;

namespace LD55.Game {
	public class GameStatData {
		public float TimePlayed { get; set; }
		public int EnemiesKilled { get; set; }
		public int CreaturesSummoned { get; set; }
		public float SummoningAccuracy => AccuracyPerSummoning.Count > 0 ? AccuracyPerSummoning.Average() : 0;
		public List<float> AccuracyPerSummoning { get; } = new List<float>();
		public bool HellReached { get; set; }
	}
}