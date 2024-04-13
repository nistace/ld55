using LD55.Inputs;
using UnityEngine;

namespace LD55.Game {
	[RequireComponent(typeof(CharacterController))]
	public class PlayerController : MonoBehaviour {
		[SerializeField] protected CharacterController characterController;

		public CharacterController CharacterController => characterController;

		private void Reset() {
			characterController = GetComponent<CharacterController>();
		}

		private void Update() {
			characterController.Move(InputManager.Controls.Player.Movement.ReadValue<Vector2>());
		}
	}
}