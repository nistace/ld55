using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu]
	public class CharacterType : ScriptableObject {
		[SerializeField] protected string displayName;
		[SerializeField] protected string role;
		[SerializeField] protected int maxHealth = 50;
		[SerializeField] protected float speed = 1;
		[SerializeField] protected int armor;

		public string DisplayName => displayName;
		public string Role => role;
		public int MaxHealth => maxHealth;
		public float Speed => speed;
		public int Armor => armor;
	}
}