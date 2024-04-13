using UnityEngine;

namespace LD55.Game {
	public class MonsterController : MonoBehaviour, IAiCharacter {
		[SerializeField] protected CharacterController characterController;

		public CharacterController CharacterController => characterController;
		public CharacterController Target { get; set; }

		private void Update() {
			if (!Target) return;
			characterController.Move(Target.Position - characterController.Position);
		}
	}
}