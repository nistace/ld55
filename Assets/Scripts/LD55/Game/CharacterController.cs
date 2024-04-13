using UnityEngine;

namespace LD55.Game {
	public class CharacterController : MonoBehaviour {
		[SerializeField] protected float maxHealth = 50;
		[SerializeField] protected float currentHealth = 50;
		[SerializeField] protected float speed = 1;

		public Vector2 Position => transform.position;
		private Vector2 LastMovement { get; set; }
		public Vector2 LastMovementNormalized { get; private set; }

		public void Move(Vector2 movement) {
			LastMovement = Vector3.ClampMagnitude(movement, 1) * (speed * Time.deltaTime);
			LastMovementNormalized = LastMovement == Vector2.zero ? Vector2.zero : LastMovement.normalized;
			transform.position += (Vector3)LastMovement;
		}
	}
}