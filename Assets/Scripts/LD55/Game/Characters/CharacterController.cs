using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game {
	public class CharacterController : MonoBehaviour, ICombatTarget {
		[SerializeField] protected CharacterType type;

		public CharacterType Type => type;
		public Vector2 Position => transform.position;
		private int CurrentHealth { get; set; }
		public bool IsDead => CurrentHealth <= 0;
		private Vector2 LastMovement { get; set; }
		public Vector2 LastMovementNormalized { get; private set; }

		public UnityEvent OnTookDamage { get; } = new UnityEvent();
		public UnityEvent OnDied { get; } = new UnityEvent();

		public static UnityEvent<CharacterController> OnAnyCharacterDied { get; } = new UnityEvent<CharacterController>();

		private void Start() {
			CurrentHealth = type.MaxHealth;
		}

		public void Move(Vector2 movement) {
			LastMovement = Vector3.ClampMagnitude(movement, 1) * (type.Speed * Time.deltaTime);
			LastMovementNormalized = LastMovement == Vector2.zero ? Vector2.zero : LastMovement.normalized;
			transform.position += (Vector3)LastMovement;
		}

		public void TakeDamage(int damage) {
			if (damage <= 0) return;
			if (CurrentHealth <= 0) return;
			CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
			OnTookDamage.Invoke();
			if (CurrentHealth == 0) {
				OnDied.Invoke();
				OnAnyCharacterDied.Invoke(this);
			}
		}
	}
}