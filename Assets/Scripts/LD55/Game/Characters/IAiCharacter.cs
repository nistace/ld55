namespace LD55.Game {
	public interface IAiCharacter : ICharacterBrain {
		CharacterController Target { get; set; }
	}
}