using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu]
	public class SummoningRecipe : ScriptableObject {
		[SerializeField] protected string summoningLine;
		[SerializeField] protected AiCharacter summoningPrefab;
		[SerializeField] protected Sprite summoningSprite;

		public string SummoningLine => summoningLine;
		public AiCharacter SummoningPrefab => summoningPrefab;
		public Sprite SummoningSprite => summoningSprite;
	}
}