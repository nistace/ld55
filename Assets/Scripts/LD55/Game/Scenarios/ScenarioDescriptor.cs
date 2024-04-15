using System;
using System.Collections.Generic;
using NiUtils.Extensions;
using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu]
	public class ScenarioDescriptor : ScriptableObject {
		[Serializable] public class ClipAndText {
			[SerializeField] protected AudioClip clip;
			[SerializeField] protected string text;
			public AudioClip Clip => clip;
			public string Text => text;
		}

		[Serializable] public class ScenarioStep {
			public enum WaitFor {
				Nothing = 0,
				VillageIdiotInScene = 1,
				RockTouched = 2,
				SomethingSummoned = 3,
				VillageIdiotLeft = 4
			}

			public enum Trigger {
				Nothing = 0,
				VillageIdiotEnter = 1,
				RockSpawn = 2,
				VillageIdiotExit = 3,
				StopPlayingWithMud = 4
			}

			[SerializeField] protected WaitFor waitFor;
			[SerializeField] protected Trigger trigger;
			[SerializeField] protected ClipAndText line;

			public ClipAndText Line => line;
			public WaitFor TheWaitFor => waitFor;
			public Trigger TheTrigger => trigger;
		}

		[SerializeField] protected ScenarioStep[] introSteps;
		[SerializeField] protected ClipAndText introSkippedLine;
		[SerializeField] protected ClipAndText gameOverLine;
		[SerializeField] protected ClipAndText portalSpawnedLine;

		[SerializeField] protected ClipAndText[] hellLines;
		[SerializeField] protected ClipAndText[] hellLoopLines;
		[SerializeField] protected ClipAndText pissedNarratorLine;
		[SerializeField] protected AudioClip introMusic;
		[SerializeField] protected AudioClip gameplayMusic;

		public IEnumerable<ScenarioStep> IntroSteps => introSteps;
		public ClipAndText IntroSkippedLine => introSkippedLine;
		public ClipAndText GameOverLine => gameOverLine;
		public ClipAndText PortalSpawnedLine => portalSpawnedLine;
		public IReadOnlyList<ClipAndText> HellLines => hellLines;
		public ClipAndText RandomHellLoopLines => hellLoopLines.Random();
		public ClipAndText PissedNarratorLine => pissedNarratorLine;

		public AudioClip IntroMusic => introMusic;
		public AudioClip GameplayMusic => gameplayMusic;
	}
}