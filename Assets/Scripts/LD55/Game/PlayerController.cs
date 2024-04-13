using LD55.Inputs;
using UnityEngine;

namespace LD55.Game {
	public class PlayerController : MonoBehaviour {
		[SerializeField] protected float speed = 1;

		private void Update() {
			transform.position += (Vector3)InputManager.Controls.Player.Movement.ReadValue<Vector2>() * (speed * Time.deltaTime);
		}
	}
}