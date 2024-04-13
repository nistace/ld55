using UnityEngine;

namespace LD55.Game
{
    public interface ICombatant : ICombatTarget
    {
        void Move(Vector2 targetPosition);
        void SetDelayBeforeNextAttack(float delayBeforeNextAttack);
        bool IsNextAttackReady();
    }
}