using System.Collections;
using UnityEngine;

namespace LD55.Game.Ui {
	public class GameUi : MonoBehaviour {
		[SerializeField] protected SummoningRecipeUi recipe;
		[SerializeField] protected GameOverScreenUi gameOver;
		[SerializeField] protected ScenarioTextUi scenarioText;
		[SerializeField] protected ExitHellUi exitHell;
		[SerializeField] protected CanvasGroup gameOverCanvasGroup;
		[SerializeField] protected CanvasGroup gameplayCanvasGroup;

		public SummoningRecipeUi Recipe => recipe;
		public GameOverScreenUi GameOver => gameOver;
		public ScenarioTextUi ScenarioText => scenarioText;
		public ExitHellUi ExitHell => exitHell;

		private void Start() {
			gameOverCanvasGroup.alpha = 0;
			gameplayCanvasGroup.alpha = 1;
			gameOver.gameObject.SetActive(false);
			exitHell.Hide();
		}

		public IEnumerator ShowGameOverScreen(GameStatData gameStatData, float fadeTime) {
			gameOver.Show(gameStatData);
			for (var t = 0f; t < fadeTime; t += Time.deltaTime) {
				gameplayCanvasGroup.alpha = Mathf.MoveTowards(gameplayCanvasGroup.alpha, 0, Time.deltaTime);
				gameOverCanvasGroup.alpha = Mathf.MoveTowards(gameplayCanvasGroup.alpha, 1, Time.deltaTime);
				yield return null;
			}
			gameplayCanvasGroup.alpha = 0;
			gameOverCanvasGroup.alpha = 1;
		}
	}
}