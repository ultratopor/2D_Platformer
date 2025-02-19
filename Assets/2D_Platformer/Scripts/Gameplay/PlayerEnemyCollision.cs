using Core;
using Model;
using Mechanics;

namespace Gameplay
{
    public class PlayerEnemyCollision : Event<PlayerEnemyCollision>
    {
        public EnemyController Enemy;
        public PlayerController Player;

        private PlatformerModel _model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var willHurtEnemy = Player.Bounds.center.y >= Enemy.Bounds.max.y;

            if (willHurtEnemy)
            {
                var enemyHealth = Enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.Decrement();
                    if (!enemyHealth.IsAlive)
                    {
                        Simulation.Schedule<EnemyDeath>().Enemy = Enemy;
                        Player.Bounce(2);
                    }
                    else
                    {
                        Player.Bounce(7);
                    }
                }
                else
                {
                    Simulation.Schedule<EnemyDeath>().Enemy = Enemy;
                    Player.Bounce(2);
                }
            }
            else
            {
                Simulation.Schedule<PlayerDeath>();
            }
        }
    }
}