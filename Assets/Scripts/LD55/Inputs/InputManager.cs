using System;
using NiUtils.Extensions;

namespace LD55.Inputs {
	public static class InputManager {
		public static Controls Controls { get; } = new Controls();
		public static ControlsSpriteLibrary.ControllerSprites ControllerSprites { get; set; }

		public static string GetKeyText(char control) {
			var inputAction = control switch {
				'A' => Controls.Player.SummonA,
				'B' => Controls.Player.SummonB,
				'C' => Controls.Player.SummonC,
				'D' => Controls.Player.SummonD,
				'E' => Controls.Player.SummonE,
				'F' => Controls.Player.SummonF,
				'G' => Controls.Player.SummonG,
				'H' => Controls.Player.SummonH,
				_ => throw new ArgumentOutOfRangeException(nameof(control), control, null)
			};
			return inputAction.GetControl(inputAction.GetNonCompositeBinding()).displayName;
		}
	}
}