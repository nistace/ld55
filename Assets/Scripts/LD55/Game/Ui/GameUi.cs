using UnityEngine;

namespace LD55.Game.Ui {
	public class GameUi : MonoBehaviour {
		[SerializeField] protected SummoningRecipeUi recipe;

		public SummoningRecipeUi Recipe => recipe;
	}
}