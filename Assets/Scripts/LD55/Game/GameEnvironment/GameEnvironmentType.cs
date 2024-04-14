using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu]
	public class GameEnvironmentType : ScriptableObject {
		[SerializeField] protected Color background;
		[SerializeField] protected float transitionDuration = 5;
		[SerializeField] protected bool spawningAllowed = true;

		public Color Background => background;
		public bool SpawningAllowed => spawningAllowed;
		public float TransitionDuration => transitionDuration;
	}
}