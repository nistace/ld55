using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuUi : MonoBehaviour {
	[SerializeField] private Button startGameButton;

	public UnityEvent OnStartGameButtonClicked => startGameButton.onClick;
}