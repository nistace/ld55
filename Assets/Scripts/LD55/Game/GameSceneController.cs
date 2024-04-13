using LD55.Game.Ui;
using LD55.Inputs;
using UnityEngine;

namespace LD55.Game {
	public class GameSceneController : MonoBehaviour {
		[SerializeField] protected ControlsSpriteLibrary controlsSpriteLibrary;
		[SerializeField] protected CharacterManager characterManager;
		[SerializeField] protected GameUi ui;

		private void OnEnable() {
			ui.Recipe.Init(characterManager.Player.Summoner);
			characterManager.OnPlayerDied.AddListener(HandlePlayerDied);
			InputManager.ControllerSprites = controlsSpriteLibrary.KeyboardSprites;
			InputManager.Controls.Player.Enable();
		}

		private static void HandlePlayerDied() {
			InputManager.Controls.Player.Disable();
		}

		private void OnDisable() {
			InputManager.Controls.Player.Disable();
		}
	}
}