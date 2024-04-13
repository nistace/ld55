using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu]
	public class CharacterType : ScriptableObject {
		[SerializeField] protected int maxHealth = 50;
		[SerializeField] protected float speed = 1;

		public int MaxHealth => maxHealth;
		public float Speed => speed;
	}
}