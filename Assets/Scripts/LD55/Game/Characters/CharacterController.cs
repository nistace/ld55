using UnityEngine;

namespace LD55.Game {
	public class CharacterController : MonoBehaviour {
		[SerializeField] protected CharacterType type;

		public CharacterType Type => type;
		public Vector2 Position => transform.position;
		private int CurrentHealth { get; set; }
		private Vector2 LastMovement { get; set; }
		public Vector2 LastMovementNormalized { get; private set; }

		private void Start() {
			CurrentHealth = type.MaxHealth;
		}

		public void Move(Vector2 movement) {
			LastMovement = Vector3.ClampMagnitude(movement, 1) * (type.Speed * Time.deltaTime);
			LastMovementNormalized = LastMovement == Vector2.zero ? Vector2.zero : LastMovement.normalized;
			transform.position += (Vector3)LastMovement;
		}
	}
}