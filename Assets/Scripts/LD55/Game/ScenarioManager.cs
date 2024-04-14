using LD55.Inputs;
using UnityEngine;

namespace LD55.Game {
	public class ScenarioManager : MonoBehaviour {
		[SerializeField] protected GameEnvironmentType gameplayEnvironment;

		public void SkipIntro() {
			GameEnvironmentManager.Instance.ChangeEnvironmentType(gameplayEnvironment);
			CharacterManager.Instance.OnPlayerDied.AddListener(HandlePlayerDied);
			InputManager.Controls.Player.Enable();
		}

		private static void HandlePlayerDied() {
			// TODO
			InputManager.Controls.Player.Disable();
		}
	}
}