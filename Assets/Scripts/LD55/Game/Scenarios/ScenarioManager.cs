using System;
using System.Collections;
using LD55.Game.Ui;
using LD55.Inputs;
using NiUtils.Audio;
using NiUtils.Extensions;
using UnityEngine;

namespace LD55.Game {
	public class ScenarioManager : MonoBehaviour {
		[SerializeField] protected GameEnvironmentType gameplayEnvironment;
		[SerializeField] protected GameEnvironmentType gameOverEnvironment;
		[SerializeField] protected GameUi ui;
		[SerializeField] protected ScenarioDescriptor scenarioDescriptor;
		[SerializeField] protected ScenarioVillageIdiot introVillageIdiot;
		[SerializeField] protected MudSpawner mudSpawner;

		private GameStatData GameStat { get; } = new GameStatData();
		private bool PortalLinePlayed { get; set; }

		private void Start() {
			CharacterManager.OnPlayerDied.AddListenerOnce(HandlePlayerDied);
			CharacterManager.OnSummoningCompletedWithAccuracy.AddListenerOnce(HandleSummoningCompletedWithAccuracy);
			CharacterManager.Instance.Player.Summoner.OnCommandFailedWithAccuracy.AddListenerOnce(HandleCommandFailedWithAccuracy);
			CharacterManager.OnEnemyKilled.AddListenerOnce(HandleEnemyKilled);
			PortalManager.OnPortalActivated.AddListenerOnce(HandlePortalActivated);
		}

		public void StartWithIntro() => StartCoroutine(PlayIntro());

		private IEnumerator PlayIntro() {
			mudSpawner.enabled = true;
			AudioManager.Music.ChangeClip(scenarioDescriptor.IntroMusic, true);
			var villageIdiotHasEntered = false;
			var villageIdiotHasExited = false;

			foreach (var scenarioStep in scenarioDescriptor.IntroSteps) {
				switch (scenarioStep.TheTrigger) {
					case ScenarioDescriptor.ScenarioStep.Trigger.Nothing: break;
					case ScenarioDescriptor.ScenarioStep.Trigger.VillageIdiotEnter:
						StartCoroutine(introVillageIdiot.PlayEntering(() => villageIdiotHasEntered = true));
						break;
					case ScenarioDescriptor.ScenarioStep.Trigger.RockSpawn:
						InputManager.Controls.Player.Enable();
						GameEnvironmentManager.Instance.ChangeEnvironmentType(gameplayEnvironment);
						MagicStoneManager.Instance.SpawnFirstMagicStone();
						AudioManager.Music.ChangeClip(scenarioDescriptor.GameplayMusic, false);
						break;
					case ScenarioDescriptor.ScenarioStep.Trigger.VillageIdiotExit:
						StartCoroutine(introVillageIdiot.PlayExiting(() => villageIdiotHasExited = true));
						break;
					case ScenarioDescriptor.ScenarioStep.Trigger.StopPlayingWithMud:
						mudSpawner.enabled = false;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				switch (scenarioStep.TheWaitFor) {
					case ScenarioDescriptor.ScenarioStep.WaitFor.Nothing: break;
					case ScenarioDescriptor.ScenarioStep.WaitFor.VillageIdiotInScene:
						while (!villageIdiotHasEntered) yield return null;
						break;
					case ScenarioDescriptor.ScenarioStep.WaitFor.RockTouched:
						ui.ScenarioText.Show(InputManager.ControllerSprites.InstructionToInteractWithRock);
						while (CharacterManager.Instance.Player.Summoner.Level == 0) yield return null;
						ui.ScenarioText.Hide();
						break;
					case ScenarioDescriptor.ScenarioStep.WaitFor.SomethingSummoned:
						ui.ScenarioText.Show(InputManager.ControllerSprites.InstructionToSummon);
						while (GameStat.CreaturesSummoned < 1) yield return null;
						ui.ScenarioText.Hide();
						InputManager.Controls.Player.Disable();
						break;
					case ScenarioDescriptor.ScenarioStep.WaitFor.VillageIdiotLeft:
						while (!villageIdiotHasExited) yield return null;
						break;
					default: throw new ArgumentOutOfRangeException();
				}

				yield return StartCoroutine(PlayLine(scenarioStep.Line));
				yield return new WaitForSeconds(0.5f);
			}

			InputManager.Controls.Player.Enable();
			MagicStoneManager.Instance.SetNextMagicSpawnAllowed(true);
			CharacterManager.Instance.StartFirstWave();
		}

		private IEnumerator PlayLine(ScenarioDescriptor.ClipAndText line) {
			var source = AudioManager.Voices.Play(line.Clip);
			source.priority = 2;
			ui.ScenarioText.Show(line.Text);
			yield return null;
			while (source.isPlaying) yield return null;
			ui.ScenarioText.Hide();
		}

		public void SkipIntro() {
			StartCoroutine(PlayLine(scenarioDescriptor.IntroSkippedLine));
			GameEnvironmentManager.Instance.ChangeEnvironmentType(gameplayEnvironment);
			MagicStoneManager.Instance.SpawnFirstMagicStone();
			MagicStoneManager.Instance.SetNextMagicSpawnAllowed(true);
			CharacterManager.Instance.StartFirstWave();
			AudioManager.Music.ChangeClip(scenarioDescriptor.GameplayMusic, false);
			InputManager.Controls.Player.Enable();
			mudSpawner.enabled = false;
		}

		private void HandlePortalActivated() {
			GameStat.HellReached = true;
			StartCoroutine(PlayInHell());
		}

		private IEnumerator PlayInHell() {
			for (var index = 0; index < scenarioDescriptor.HellLines.Count; index++) {
				var hellLine = scenarioDescriptor.HellLines[index];
				yield return StartCoroutine(PlayLine(hellLine));
				if (index == scenarioDescriptor.HellLines.Count - 2)
					yield return new WaitForSeconds(5);
			}

			ui.ExitHell.Show();

			var timeSpentInHell = 0f;
			var nextLineTime = timeSpentInHell + 6;
			var pissedPlayed = false;
			var hellExited = false;
			ui.ExitHell.OnExitHellClicked.AddListenerOnce(() => hellExited = true);
			while (!hellExited) {
				timeSpentInHell += Time.deltaTime;

				if (nextLineTime < timeSpentInHell) {
					if (!pissedPlayed && timeSpentInHell > 60) {
						StartCoroutine(PlayLine(scenarioDescriptor.PissedNarratorLine));
						pissedPlayed = true;
					}
					else {
						StartCoroutine(PlayLine(scenarioDescriptor.RandomHellLoopLines));
					}
					nextLineTime = timeSpentInHell + 12;
				}

				yield return null;
			}
			GameEnvironmentManager.Instance.ChangeEnvironmentType(gameplayEnvironment);
			ui.ExitHell.Hide();
		}

		private void HandleEnemyKilled() => GameStat.EnemiesKilled++;

		private void HandleSummoningCompletedWithAccuracy(SummoningRecipe recipe, float accuracy) {
			if (!PortalLinePlayed && recipe.IsPortal) {
				PortalLinePlayed = true;
				StartCoroutine(PlayLine(scenarioDescriptor.PortalSpawnedLine));
			}
			GameStat.CreaturesSummoned++;
			GameStat.AccuracyPerSummoning.Add(accuracy);
		}

		private void HandleCommandFailedWithAccuracy(float accuracy) => GameStat.AccuracyPerSummoning.Add(accuracy);

		private void HandlePlayerDied() {
			GameEnvironmentManager.Instance.ChangeEnvironmentType(gameOverEnvironment);
			StartCoroutine(PlayLine(scenarioDescriptor.GameOverLine));
			AudioManager.Music.ChangeClip(scenarioDescriptor.IntroMusic, false);
			GameStat.TimePlayed = CharacterManager.Instance.PlayingTime;
			StartCoroutine(ui.ShowGameOverScreen(GameStat, .5f));
			InputManager.Controls.Player.Disable();
		}
	}
}