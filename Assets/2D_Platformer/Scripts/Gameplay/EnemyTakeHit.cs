using Mechanics;
using Core;

namespace Gameplay
{
    public class EnemyTakeHit : Event<EnemyTakeHit>
    {
        public EnemyController Enemy;
        public override void Execute()
        {
            Enemy.Control.Animator.SetTrigger("takeHit");
            // проигрывание звуков
        }
    }
}