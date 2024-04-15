using LD55.Inputs;
using UnityEngine;

namespace LD55.Game {
	[RequireComponent(typeof(CharacterController))]
	public class PlayerController : MonoBehaviour, ICharacterBrain {
		[SerializeField] protected CharacterController characterController;
		[SerializeField] protected Summoner summoner;

		public CharacterController CharacterController => characterController;
		public Summoner Summoner => summoner;

		private void Reset() {
			characterController = GetComponent<CharacterController>();
		}

		private void Update() {
			if (summoner.IsSummoning) return;
			characterController.Move(InputManager.Controls.Player.Movement.ReadValue<Vector2>());
		}

		public Vector2 Position => CharacterController.Position;
		public void TakeDamage(int damage) => characterController.TakeDamage(damage);

		public void LevelUp() {
			Summoner.LevelUp();
		}
	}
}