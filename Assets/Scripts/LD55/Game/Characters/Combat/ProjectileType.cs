using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu]
	public class ProjectileType : ScriptableObject {
		[SerializeField] protected float speed = 1;
		[SerializeField] protected AnimationCurve heightCurve = AnimationCurve.Constant(0, 0, 0);
		[SerializeField] protected int damage = 1;
		[SerializeField] protected bool rightToForward = true;
		[SerializeField] protected AudioClip shotClip;

		public float Speed => speed;
		public AnimationCurve HeightCurve => heightCurve;
		public int Damage => damage;
		public bool RightToForward => rightToForward;
		public AudioClip ShotClip => shotClip;
	}
}