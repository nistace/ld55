using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LD55.Game.Ui {
	public class GameOverScreenUi : MonoBehaviour {
		[SerializeField] protected TMP_Text timePlayedText;
		[SerializeField] protected TMP_Text enemiesKilledText;
		[SerializeField] protected TMP_Text creaturesSummonedText;
		[SerializeField] protected TMP_Text summoningAccuracyText;
		[SerializeField] protected TMP_Text hellReachedText;
		[SerializeField] protected Button quitButton;

		public UnityEvent OnQuitButtonClicked => quitButton.onClick;

		public void Show(GameStatData data) {
			EventSystem.current.SetSelectedGameObject(quitButton.gameObject);
			gameObject.SetActive(true);
			timePlayedText.text = $"You survived <b>{FormatTime(data.TimePlayed)}</b>.";
			enemiesKilledText.text = $"<b>{data.EnemiesKilled}</b> human beings died.";
			creaturesSummonedText.text = $"<b>{data.CreaturesSummoned}</b> summonings were completed.";
			summoningAccuracyText.text = $"You summoned with an average accuracy of <b>{data.SummoningAccuracy * 100:0.00}</b>%.";
			hellReachedText.text = data.HellReached ? "You <b>went through the portal</b> and came back to have more fun" : "You <b>didn't go through the portal</b>.";
		}

		private static string FormatTime(float dataTimePlayed) => $"{dataTimePlayed / 60:00}:{dataTimePlayed % 60:00}";
	}
}