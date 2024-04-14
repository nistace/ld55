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

		private GameStatData GameStat { get; } = new GameStatData();
		private bool PortalLinePlayed { get; set; }

		private void Start() {
			CharacterManager.OnPlayerDied.AddListenerOnce(HandlePlayerDied);
			CharacterManager.OnSummoningCompletedWithAccuracy.AddListenerOnce(HandleSummoningCompletedWithAccuracy);
			CharacterManager.OnEnemyKilled.AddListenerOnce(HandleEnemyKilled);
			PortalManager.OnPortalActivated.AddListenerOnce(HandlePortalActivated);
		}

		public void StartWithIntro() => StartCoroutine(PlayIntro());

		private IEnumerator PlayIntro() {
			AudioManager.Music.ChangeClip(scenarioDescriptor.IntroMusic, true);
			foreach (var scenarioStep in scenarioDescriptor.IntroSteps) {
				switch (scenarioStep.TheTrigger) {
					case ScenarioDescriptor.ScenarioStep.Trigger.Nothing: break;
					case ScenarioDescriptor.ScenarioStep.Trigger.VillageIdiotEnter: // TODO
						break;
					case ScenarioDescriptor.ScenarioStep.Trigger.RockSpawn:
						InputManager.Controls.Player.Enable();
						GameEnvironmentManager.Instance.ChangeEnvironmentType(gameplayEnvironment);
						MagicStoneManager.Instance.SpawnFirstMagicStone();
						AudioManager.Music.ChangeClip(scenarioDescriptor.GameplayMusic, false);
						break;
					case ScenarioDescriptor.ScenarioStep.Trigger.VillageIdiotExit: // TODO
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				switch (scenarioStep.TheWaitFor) {
					case ScenarioDescriptor.ScenarioStep.WaitFor.Nothing: break;
					case ScenarioDescriptor.ScenarioStep.WaitFor.VillageIdiotInScene: // TODO
						break;
					case ScenarioDescriptor.ScenarioStep.WaitFor.RockTouched:
						while (CharacterManager.Instance.Player.Summoner.Level == 0) yield return null;
						break;
					case ScenarioDescriptor.ScenarioStep.WaitFor.SomethingSummoned:
						while (GameStat.CreaturesSummoned < 1) yield return null;
						InputManager.Controls.Player.Disable();
						break;
					case ScenarioDescriptor.ScenarioStep.WaitFor.VillageIdiotLeft: // TODO
						break;
					default: throw new ArgumentOutOfRangeException();
				}

				var source = AudioManager.Voices.Play(scenarioStep.Line.Clip);
				yield return null;
				while (source.isPlaying) yield return null;
				yield return new WaitForSeconds(0.5f);
			}

			InputManager.Controls.Player.Enable();
			MagicStoneManager.Instance.SetNextMagicSpawnAllowed(true);
			CharacterManager.Instance.StartFirstWave();
		}

		public void SkipIntro() {
			GameEnvironmentManager.Instance.ChangeEnvironmentType(gameplayEnvironment);
			AudioManager.Voices.Play(scenarioDescriptor.IntroSkippedLine.Clip);
			MagicStoneManager.Instance.SpawnFirstMagicStone();
			MagicStoneManager.Instance.SetNextMagicSpawnAllowed(true);
			CharacterManager.Instance.StartFirstWave();
			AudioManager.Music.ChangeClip(scenarioDescriptor.GameplayMusic, false);
			InputManager.Controls.Player.Enable();
		}

		private void HandlePortalActivated() => GameStat.HellReached = true;
		private void HandleEnemyKilled() => GameStat.EnemiesKilled++;

		private void HandleSummoningCompletedWithAccuracy(SummoningRecipe recipe, float accuracy) {
			if (!PortalLinePlayed && recipe.IsPortal) {
				PortalLinePlayed = true;
				AudioManager.Voices.Play(scenarioDescriptor.PortalSpawnedLine.Clip);
			}
			GameStat.AccuracyPerSummoning.Add(accuracy);
		}

		private void HandlePlayerDied() {
			GameEnvironmentManager.Instance.ChangeEnvironmentType(gameOverEnvironment);
			AudioManager.Voices.Play(scenarioDescriptor.GameOverLine.Clip);
			AudioManager.Music.ChangeClip(scenarioDescriptor.IntroMusic, false);
			GameStat.TimePlayed = CharacterManager.Instance.PlayingTime;
			StartCoroutine(ui.ShowGameOverScreen(GameStat, .5f));
			InputManager.Controls.Player.Disable();
		}
	}
}