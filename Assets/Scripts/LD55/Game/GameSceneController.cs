using LD55.Game.Ui;
using LD55.Inputs;
using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD55.Game {
	public class GameSceneController : MonoBehaviour {
		public static bool SkipIntro { get; set; }

		[SerializeField] protected ControlsSpriteLibrary controlsSpriteLibrary;
		[SerializeField] protected CharacterManager characterManager;
		[SerializeField] protected ScenarioManager scenarioManager;
		[SerializeField] protected GameUi ui;

		private void Awake() {
			InputManager.ControllerSprites ??= controlsSpriteLibrary.PSSprites;
		}

		private void Start() {
			ProjectileManager.Clear();
			ui.Recipe.Init(characterManager.Player.Summoner);
			ui.GameOver.OnQuitButtonClicked.AddListenerOnce(HandleQuitButtonClicked);
			if (SkipIntro) scenarioManager.SkipIntro();
			else scenarioManager.StartWithIntro();
		}

		private static void HandleQuitButtonClicked() => SceneManager.LoadScene("Menu");
		private void OnDisable() => InputManager.Controls.Player.Disable();
	}
}