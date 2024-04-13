using LD55.Inputs;
using UnityEngine;

namespace LD55.Game {
	public class GameSceneController : MonoBehaviour {
		private void Start() {
			InputManager.Controls.Player.Enable();
		}
	}
}