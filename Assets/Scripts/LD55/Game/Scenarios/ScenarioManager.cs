using LD55.Game.Ui;
using LD55.Inputs;
using NiUtils.Extensions;
using UnityEngine;

namespace LD55.Game {
	public class ScenarioManager : MonoBehaviour {
		[SerializeField] protected GameEnvironmentType gameplayEnvironment;
		[SerializeField] protected GameUi ui;

		private GameStatData gameStat { get; } = new GameStatData();

		public void SkipIntro() {
			GameEnvironmentManager.Instance.ChangeEnvironmentType(gameplayEnvironment);
			CharacterManager.OnPlayerDied.AddListenerOnce(HandlePlayerDied);
			CharacterManager.OnSummoningCompletedWithAccuracy.AddListenerOnce(HandleSummoningCompletedWithAccuracy);
			CharacterManager.OnEnemyKilled.AddListenerOnce(HandleEnemyKilled);
			PortalManager.OnPortalActivated.AddListenerOnce(HandlePortalActivated);
			InputManager.Controls.Player.Enable();
		}

		private void HandlePortalActivated() => gameStat.HellReached = true;
		private void HandleEnemyKilled() => gameStat.EnemiesKilled++;
		private void HandleSummoningCompletedWithAccuracy(float accuracy) => gameStat.AccuracyPerSummoning.Add(accuracy);

		private void HandlePlayerDied() {
			gameStat.TimePlayed = CharacterManager.Instance.PlayingTime;
			StartCoroutine(ui.ShowGameOverScreen(gameStat, .5f));
			InputManager.Controls.Player.Disable();
		}
	}
}