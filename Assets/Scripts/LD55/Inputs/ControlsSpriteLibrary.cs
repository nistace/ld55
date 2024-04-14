using System;
using UnityEngine;

namespace LD55.Inputs {
	[CreateAssetMenu]
	public class ControlsSpriteLibrary : ScriptableObject {
		[Serializable] public class ControllerSprites {
			[SerializeField] protected Sprite[] sprites;
			[SerializeField] protected bool displayKey;
			[SerializeField] protected string name;

			public Sprite this[char control] => sprites[control - 'A'];
			public string Name => name;
			public bool DisplayKey => displayKey;
		}

		[SerializeField] protected ControllerSprites keyboardSprites;
		[SerializeField] protected ControllerSprites xboxSprites;
		[SerializeField] protected ControllerSprites psSprites;

		public ControllerSprites KeyboardSprites => keyboardSprites;
		public ControllerSprites XboxSprites => xboxSprites;
		public ControllerSprites PSSprites => psSprites;
	}
}