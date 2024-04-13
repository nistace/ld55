using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu]
	public class SummoningRecipe : ScriptableObject {
		[SerializeField] protected string summoningLine;
		[SerializeField] protected SummoningController summoningPrefab;
		[SerializeField] protected Sprite summoningSprite;

		public string SummoningLine => summoningLine;
		public SummoningController SummoningPrefab => summoningPrefab;
		public Sprite SummoningSprite => summoningSprite;
	}
}