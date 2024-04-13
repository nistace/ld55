using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LD55.Menu.Ui {
	public class MenuUi : MonoBehaviour {
		[SerializeField] private Button startGameButton;

		public UnityEvent OnStartGameButtonClicked => startGameButton.onClick;
	}
}