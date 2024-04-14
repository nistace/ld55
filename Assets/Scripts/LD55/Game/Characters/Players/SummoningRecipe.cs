using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu]
	public class SummoningRecipe : ScriptableObject {
		[SerializeField] protected string summoningLine;
		[SerializeField] protected AiCharacter[] summoningsPrefabs;
		[SerializeField] protected Sprite summoningSprite;
		[SerializeField] protected string overrideDisplayName;
		[SerializeField] protected string overrideRole;

		public string SummoningLine => summoningLine;
		public string SummoningDisplayName => string.IsNullOrEmpty(overrideDisplayName) ? summoningsPrefabs[0].CharacterController.Type.DisplayName : overrideDisplayName;
		public string SummoningRole => string.IsNullOrEmpty(overrideRole) ?summoningsPrefabs[0].CharacterController.Type.Role: overrideDisplayName;
		public AiCharacter[] SummoningsPrefabs => summoningsPrefabs;
		public Sprite SummoningSprite => summoningSprite;
	}
}