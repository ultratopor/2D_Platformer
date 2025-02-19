using Core;
using Mechanics;

namespace Gameplay
{
    public class EnemyDeath : Event<EnemyDeath>
    {
        public EnemyController Enemy;

        public override void Execute()
        {
            Enemy.Collider.enabled = false;
            Enemy.Control.enabled = false;
            if (Enemy.Audio && Enemy.Ouch)
                Enemy.Audio.PlayOneShot(Enemy.Ouch);
        }
    }
}