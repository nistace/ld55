using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu]
	public class SummoningRecipe : ScriptableObject {
		[SerializeField] protected string summoningLine;
		[SerializeField] protected AiCharacter summoningPrefab;
		[SerializeField] protected Sprite summoningSprite;

		public string SummoningLine => summoningLine;
		public string SummoningDisplayName => summoningPrefab.CharacterController.Type.DisplayName;
		public string SummoningRole => summoningPrefab.CharacterController.Type.Role;
		public AiCharacter SummoningPrefab => summoningPrefab;
		public Sprite SummoningSprite => summoningSprite;
	}
}