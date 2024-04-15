using LD55.Game;
using LD55.Inputs;
using NiUtils.Audio;
using NiUtils.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LD55.Menu.Ui {
	public class MenuUi : MonoBehaviour {
		[SerializeField] protected ControlsSpriteLibrary controlsSpriteLibrary;
		[SerializeField] protected Selectable selectedByDefault;
		[SerializeField] protected Button inputUIButton;
		[SerializeField] protected ControlUi[] controlUis;
		[SerializeField] protected TMP_Text inputUiButtonText;
		[SerializeField] protected Button skipButton;
		[SerializeField] protected TMP_Text skipButtonText;
		[SerializeField] protected Slider musicSlider;
		[SerializeField] protected Slider sfxSlider;
		[SerializeField] protected Slider voiceSlider;
		[SerializeField] private Button startGameButton;
		[SerializeField] private Button quitButton;

		public UnityEvent OnStartGameButtonClicked => startGameButton.onClick;

		private void Start() {
			EventSystem.current.SetSelectedGameObject(selectedByDefault.gameObject);
			InputManager.ControllerSprites ??= controlsSpriteLibrary.KeyboardSprites;
			inputUIButton.onClick.AddListenerOnce(HandleInputUiButtonClicked);
			skipButton.onClick.AddListenerOnce(HandleSkipButtonClicked);
			musicSlider.onValueChanged.AddListenerOnce(HandleMusicSliderChanged);
			musicSlider.SetValueWithoutNotify(AudioManager.Music.volume);
			sfxSlider.onValueChanged.AddListenerOnce(HandleSfxSliderChanged);
			sfxSlider.SetValueWithoutNotify(AudioManager.Sfx.volume);
			voiceSlider.onValueChanged.AddListenerOnce(HandleVoiceSliderChanged);
			voiceSlider.SetValueWithoutNotify(AudioManager.Voices.volume);
			quitButton.onClick.AddListenerOnce(Application.Quit);
			RefreshControls();
			RefreshSkip();
		}

		private static void HandleMusicSliderChanged(float value) => AudioManager.Music.volume = value;
		private static void HandleSfxSliderChanged(float value) => AudioManager.Sfx.volume = value;
		private static void HandleVoiceSliderChanged(float value) => AudioManager.Voices.volume = value;

		private void HandleSkipButtonClicked() {
			GameSceneController.SkipIntro = !GameSceneController.SkipIntro;
			RefreshSkip();
		}

		private void RefreshSkip() {
			skipButtonText.text = "Skip Intro:" + (GameSceneController.SkipIntro ? "Yes" : "No");
		}

		private void HandleInputUiButtonClicked() {
			if (InputManager.ControllerSprites == controlsSpriteLibrary.PSSprites) InputManager.ControllerSprites = controlsSpriteLibrary.XboxSprites;
			else if (InputManager.ControllerSprites == controlsSpriteLibrary.XboxSprites) InputManager.ControllerSprites = controlsSpriteLibrary.KeyboardSprites;
			else if (InputManager.ControllerSprites == controlsSpriteLibrary.KeyboardSprites) InputManager.ControllerSprites = controlsSpriteLibrary.PSSprites;
			RefreshControls();
		}

		private void RefreshControls() {
			inputUiButtonText.text = $"Input UI: {InputManager.ControllerSprites.Name}";
			for (var index = 0; index < controlUis.Length; index++) {
				var controlUi = controlUis[index];
				controlUi.SetControl((char)('A' + index));
			}
		}
	}
}