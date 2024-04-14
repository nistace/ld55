using NiUtils.Extensions;
using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu]
	public class CharacterType : ScriptableObject {
		[SerializeField] protected Sprite sprite;
		[SerializeField] protected Team team;
		[SerializeField] protected string displayName;
		[SerializeField] protected string role;
		[SerializeField] protected int maxHealth = 50;
		[SerializeField] protected float speed = 1;
		[SerializeField] protected int armor;
		[SerializeField] protected AudioClip[] damagedClip;
		[SerializeField] protected AudioClip[] deadClip;

		public Sprite Sprite => sprite;
		public Team Team => team;
		public string DisplayName => displayName;
		public string Role => role;
		public int MaxHealth => maxHealth;
		public float Speed => speed;
		public int Armor => armor;
		public AudioClip RandomDamagedClip => damagedClip.Random();
		public AudioClip RandomDeadClip => deadClip.Random();
	}
}