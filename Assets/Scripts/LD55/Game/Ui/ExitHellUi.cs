using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitHellUi : MonoBehaviour {
	[SerializeField] protected Button exitHellButton;

	public UnityEvent OnExitHellClicked => exitHellButton.onClick;
	
	public void Show() {
		gameObject.SetActive(true);
		EventSystem.current.SetSelectedGameObject(exitHellButton.gameObject);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}
}