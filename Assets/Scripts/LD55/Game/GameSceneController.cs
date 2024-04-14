using LD55.Game.Ui;
using LD55.Inputs;
using UnityEngine;

namespace LD55.Game {
	public class GameSceneController : MonoBehaviour {
		[SerializeField] protected ControlsSpriteLibrary controlsSpriteLibrary;
		[SerializeField] protected CharacterManager characterManager;
		[SerializeField] protected ScenarioManager scenarioManager;
		[SerializeField] protected GameUi ui;

		private void Start() {
			ProjectileManager.Clear();
			ui.Recipe.Init(characterManager.Player.Summoner);
			InputManager.ControllerSprites = controlsSpriteLibrary.PSSprites;
			scenarioManager.SkipIntro();
		}

		private void OnDisable() {
			InputManager.Controls.Player.Disable();
		}
	}
}