using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game.Portals {
	public class Portal : MonoBehaviour {
		[SerializeField] protected CharacterController characterController;
		public Vector2 Position => transform.position;

		public static UnityEvent<Portal> OnAnyDied { get; } = new UnityEvent<Portal>();
		public static UnityEvent<Portal> OnNewSpawned { get; } = new UnityEvent<Portal>();
		public bool IsDead => characterController.IsDead;

		private void Start() {
			characterController.OnDied.AddListenerOnce(HandleDied);
			OnNewSpawned.Invoke(this);
		}

		private void HandleDied() => OnAnyDied.Invoke(this);
	}
}