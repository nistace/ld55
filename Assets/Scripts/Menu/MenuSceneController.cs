using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu {
	public class MenuSceneController : MonoBehaviour {
		[SerializeField] private MenuUi ui;

		private void OnEnable() => SetListenersEnabled(true);
		private void OnDisable() => SetListenersEnabled(true);

		private void SetListenersEnabled(bool enabled) {
			if (!ui) return;
			ui.OnStartGameButtonClicked.SetListenerActive(HandleStartButtonClicked, enabled);
		}

		private static void HandleStartButtonClicked() => SceneManager.LoadSceneAsync("Game");
	}
}